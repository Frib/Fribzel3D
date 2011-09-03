using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Fribzel3D
{
    /// <summary>
    /// Abstract screen for show/hide support and an update and draw
    /// </summary>
    public abstract class Screen
    {
        public virtual void Show() { }
        public virtual void Hide() { }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(GameTime gameTime) { }
    }

    /// <summary>
    /// Specifically made for configuring the game controls. Is dynamically built. 
    /// Hard-coded editing with arrows and enter/escape to prevent major screw-ups
    /// </summary>
    public class ControlScreen : Screen
    {
        private List<string> labels = new List<string>();
        private List<string> op1 = new List<string>();
        private List<string> op2 = new List<string>();
        private List<string> op3 = new List<string>();

        private int x = 0;
        private int y = 0;

        private bool IsSelecting;

        private Action backAction;

        public override void Show()
        {
            CreateControls();
            backAction = new Action(() => { RM.SaveConfig(); Game1.BaseGame.ShowScreen(SM.MainMenu()); });
            base.Show();
        }

        /// <summary>
        /// Creates all the key configurations to display
        /// </summary>
        private void CreateControls()
        {
            labels.Clear();
            op1.Clear();
            op2.Clear();
            op3.Clear();
            string[] buttons = RM.GetValidInputStrings();
            foreach (string s in buttons)
            {
                labels.Add(s);
                List<Button> buttonList = RM.GetButtons(s);

                if (buttonList.Count > 0)
                {
                    op1.Add(buttonList[0].ToString());
                }
                else
                {
                    op1.Add("<EMPTY>");
                }

                if (buttonList.Count > 1)
                {
                    op2.Add(buttonList[1].ToString());
                }
                else
                {
                    op2.Add("<EMPTY>");
                }

                if (buttonList.Count > 2)
                {
                    op3.Add(buttonList[2].ToString());
                }
                else
                {
                    op3.Add("<EMPTY>");
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsSelecting)
            {
                if (IM.IsKeyPressed(Keys.Escape))
                {
                    backAction();
                }

                if (IM.IsKeyPressed(Keys.Left))
                {
                    x--;
                    if (x < 0)
                    {
                        x = 2;
                    }
                }
                if (IM.IsKeyPressed(Keys.Right))
                {
                    x++;
                    if (x > 2)
                    {
                        x = 0;
                    }
                }
                if (IM.IsKeyPressed(Keys.Up))
                {
                    y--;
                    if (y < 0)
                    {
                        y = labels.Count - 1;
                    }
                }
                if (IM.IsKeyPressed(Keys.Down))
                {
                    y++;
                    if (y >= labels.Count)
                    {
                        y = 0;
                    }
                }
                if (IM.IsKeyPressed(Keys.Enter))
                {
                    IsSelecting = true;
                }
            }
            else
            {
                if (IM.IsKeyPressed(Keys.Escape))
                {
                    IsSelecting = false;
                    return;
                }
                Keys[] k = IM.GetPressedKeys();
                if (k.Length == 1)
                {
                    RM.InsertKey(labels[y], new Button(k[0]), x);
                    IsSelecting = false;
                    CreateControls();
                    return;
                }
                MouseButton mb = IM.GetPressedMouse();
                if (mb != MouseButton.None)
                {
                    RM.InsertKey(labels[y], new Button(mb), x);
                    IsSelecting = false;
                    CreateControls();
                    return;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            int offsetY = 80;
            int quarter = Game1.Width / 5;

            Game1.SpriteBatch.DrawString(RM.Font(Fonts.DefaultFont), "Use Arrows to select, press enter then a key/mousebutton to alter", new Vector2(32, 16), CM.MainMenuFont);
            Game1.SpriteBatch.DrawString(RM.Font(Fonts.DefaultFont), "You cannot (re)map enter or escape. Multibinds are possible", new Vector2(32, 40), CM.MainMenuSelected);

            for (int i = 0; i < labels.Count; i++)
            {
                Game1.SpriteBatch.DrawString(RM.Font(Fonts.DefaultFont), labels[i], new Vector2(quarter * 1, offsetY), CM.MainMenuExtra);

                // Not selected: Default color
                // Highlighted: Selected color
                // Highlighted and editing: Extra color
                Game1.SpriteBatch.DrawString(RM.Font(Fonts.DefaultFont), op1[i], new Vector2(quarter * 2, offsetY), y == i && x == 0 ? IsSelecting ? CM.MainMenuExtra : CM.MainMenuSelected : CM.MainMenuFont);
                Game1.SpriteBatch.DrawString(RM.Font(Fonts.DefaultFont), op2[i], new Vector2(quarter * 3, offsetY), y == i && x == 1 ? IsSelecting ? CM.MainMenuExtra : CM.MainMenuSelected : CM.MainMenuFont);
                Game1.SpriteBatch.DrawString(RM.Font(Fonts.DefaultFont), op3[i], new Vector2(quarter * 4, offsetY), y == i && x == 2 ? IsSelecting ? CM.MainMenuExtra : CM.MainMenuSelected : CM.MainMenuFont);

                offsetY += 24;
            }

            base.Draw(gameTime);
        }
    }

    /// <summary>
    /// Basic menu, allows for easy option adding, headers and footers.
    /// See ManagementHelpers for the screenmanager (SM) class
    /// </summary>
    public class MenuScreen : Screen
    {
        private List<MenuOption> options = new List<MenuOption>();
        private List<MenuOption> header = new List<MenuOption>();
        private List<MenuOption> footer = new List<MenuOption>();

        private int optionsHeight;
        private int headerHeight = 32;
        private int footerHeight = Game1.Height - 32;

        private int optionsStart;
        private int fontHeight;

        private int selectedOption;
        private Action backAction;

        /// <summary>
        /// Add a selectable menu options
        /// </summary>
        /// <param name="name">The name of the option displayed</param>
        /// <param name="action">The action that should be performed when pressed</param>
        public void AddMenuEntry(string name, Action action)
        {
            MenuOption mo = new MenuOption();
            mo.Action = action;
            mo.Name = name;
            mo.Size = RM.Font(Fonts.MenuFont).MeasureString(name);
            options.Add(mo);
            fontHeight = (int)mo.Size.Y + 8;
            optionsHeight += fontHeight;
        }

        /// <summary>
        /// Add header text
        /// </summary>
        /// <param name="text">The text</param>
        public void AddHeader(string text)
        {
            MenuOption mo = new MenuOption();
            mo.Name = text;
            mo.Size = RM.Font(Fonts.MenuFont).MeasureString(text);
            header.Add(mo);
        }

        /// <summary>
        /// Add footer text
        /// </summary>
        /// <param name="text">the text</param>
        public void AddFooter(string text)
        {
            MenuOption mo = new MenuOption();
            mo.Name = text;
            mo.Size = RM.Font(Fonts.MenuFont).MeasureString(text);
            footer.Add(mo);
        }

        /// <summary>
        /// Add an action for when escape is pressed
        /// </summary>
        /// <param name="action">The action</param>
        public void AddBackAction(Action action)
        {
            backAction = action;
        }

        public override void Show()
        {
            CalculatePositions();
            base.Show();
        }

        public override void Update(GameTime gameTime)
        {
            if (RM.IsPressed(Input.Up))
            {
                selectedOption -= 1;
            }
            if (RM.IsPressed(Input.Down))
            {
                selectedOption += 1;
            }
            if (selectedOption < 0)
            {
                selectedOption = options.Count - 1;
            }
            if (selectedOption >= options.Count)
            {
                selectedOption = 0;
            }
            if (RM.IsPressed(Input.Accept) && options.Count > selectedOption)
            {
                options[selectedOption].Action();
            }
            else if (RM.IsPressed(Input.Back) && backAction != null)
            {
                backAction();
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            for (int i = 0; i < header.Count; i++)
            {
                Game1.SpriteBatch.DrawString(RM.Font(Fonts.MenuFont), header[i].Name, header[i].Position, CM.MainMenuExtra);
            }

            for (int i = 0; i < options.Count; i++)
            {
                Game1.SpriteBatch.DrawString(RM.Font(Fonts.MenuFont), options[i].Name, options[i].Position, selectedOption == i ? CM.MainMenuSelected : CM.MainMenuFont);
            }

            for (int i = 0; i < footer.Count; i++)
            {
                Game1.SpriteBatch.DrawString(RM.Font(Fonts.MenuFont), footer[i].Name, footer[i].Position, CM.MainMenuExtra);
            }

            base.Draw(gameTime);
        }

        /// <summary>
        /// Calculate the positions for all the text
        /// </summary>
        private void CalculatePositions()
        {
            optionsStart = (Game1.Height - optionsHeight) / 2;
            for (int i = 0; i < options.Count; i++)
            {
                options[i].Position = new Vector2((Game1.Width - options[i].Size.X) / 2, optionsStart + (fontHeight * i));
            }

            for (int i = 0; i < header.Count; i++)
            {
                header[i].Position = new Vector2((Game1.Width - header[i].Size.X) / 2, headerHeight + (header[i].Size.Y * i));
            }

            // they are drawn from the bottom up, so reverse the list
            footer.Reverse();

            for (int i = 0; i < footer.Count; i++)
            {
                footer[i].Position = new Vector2((Game1.Width - footer[i].Size.X) / 2, footerHeight - (footer[i].Size.Y * (i + 1)));
            }
        }
    }

    public class MenuOption
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private Vector2 size;

        public Vector2 Size
        {
            get { return size; }
            set { size = value; }
        }
        private Vector2 position;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        private Action action;

        public Action Action
        {
            get { return action; }
            set { action = value; }
        }
    }
}
