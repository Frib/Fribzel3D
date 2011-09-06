using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Fribzel3D.Management;
using Fribzel3D.ResourceFiles;

namespace Fribzel3D.Screens
{
    /// <summary>
    /// Specifically made for configuring the game controls. Is dynamically built. 
    /// Hard-coded editing with arrows and enter/escape to prevent major screw-ups
    /// </summary>
    public class ControlScreen : Screen
    {
        private List<InputAction> actions = new List<InputAction>();
        private List<MenuOption> labels = new List<MenuOption>();
        private List<MenuOption> op1 = new List<MenuOption>();
        private List<MenuOption> op2 = new List<MenuOption>();
        private List<MenuOption> op3 = new List<MenuOption>();

        private int x = 0;
        private int y = 0;

        private bool IsSelecting;

        private Action backAction;

        public ControlScreen(Screen previousScreen) : base()
        {
            backAction = new Action(() => { RM.SaveConfig(); Fribzel.BaseGame.ShowScreen(previousScreen); });
        }

        public override void Show()
        {
            CreateControls();
            Fribzel.BaseGame.IsMouseVisible = true;
            IM.SnapToCenter = false;
            base.Show();
        }

        /// <summary>
        /// Creates all the key configurations to display
        /// </summary>
        private void CreateControls()
        {
            actions.Clear();
            labels.Clear();
            op1.Clear();
            op2.Clear();
            op3.Clear();
            InputAction[] buttons = RM.GetValidInputActions();
            foreach (InputAction ia in buttons)
            {
                AppendInputActions(ia);
            }
            CalculatePositions();
        }

        private void CalculatePositions()
        {
            int offsetY = 80;
            int quarter = Fribzel.Width / 5;
            var font = RM.Font(Font.DefaultFont);
            Vector2 minSize = new Vector2(font.MeasureString("<EMPTY>").X, 0);

            for (int i = 0; i < actions.Count; i++)
            {
                labels[i].Position = new Vector2(quarter, offsetY);
                op1[i].Position = new Vector2(quarter * 2, offsetY);
                op2[i].Position = new Vector2(quarter * 3, offsetY);
                op3[i].Position = new Vector2(quarter * 4, offsetY);

                labels[i].Size = Vector2.Max(font.MeasureString(labels[i].Name), minSize);
                op1[i].Size = Vector2.Max(font.MeasureString(op1[i].Name), minSize);
                op2[i].Size = Vector2.Max(font.MeasureString(op2[i].Name), minSize);
                op3[i].Size = Vector2.Max(font.MeasureString(op3[i].Name), minSize);

                offsetY += 24;
            }
        }

        private void AppendInputActions(InputAction ia)
        {
            actions.Add(ia);
            labels.Add(new MenuOption() { Name = ia.ToString() });

            List<Button> buttonList = RM.GetButtons(ia);

            if (buttonList.Count > 0)
            {
                op1.Add(new MenuOption() { Name = buttonList[0].ToString() });
            }
            else
            {
                op1.Add(new MenuOption() { Name = "<EMPTY>" });
            }

            if (buttonList.Count > 1)
            {
                op2.Add(new MenuOption() { Name = buttonList[1].ToString() });
            }
            else
            {
                op2.Add(new MenuOption() { Name = "<EMPTY>" });
            }

            if (buttonList.Count > 2)
            {
                op3.Add(new MenuOption() { Name = buttonList[2].ToString() });
            }
            else
            {
                op3.Add(new MenuOption() { Name = "<EMPTY>" });
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (IsSelecting)
            {
                UpdateRebinding();
            }
            else
            {
                UpdateNavigating();
            }
        }

        private void UpdateNavigating()
        {
            bool hoversOverButton = HandleMouseInput();
            HandleKeyboardInput();

            if (hoversOverButton && IM.IsLeftMousePressed())
            {
                IsSelecting = true;
            }

            if (IM.IsKeyPressed(Keys.Enter))
            {
                IsSelecting = true;
            }
        }

        private void HandleKeyboardInput()
        {
            if (IM.IsKeyPressed(Keys.Escape))
            {
                backAction();
            }
            if (IM.IsKeyPressed(Keys.Left))
            {
                x = CycleIndex(x - 1, 2);
            }
            if (IM.IsKeyPressed(Keys.Right))
            {
                x = CycleIndex(x + 1, 2);
            }
            if (IM.IsKeyPressed(Keys.Up))
            {
                y = CycleIndex(y - 1, actions.Count - 1);
            }
            if (IM.IsKeyPressed(Keys.Down))
            {
                y = CycleIndex(y + 1, actions.Count - 1);
            }
        }

        private bool HandleMouseInput()
        {
            bool movedMouse = IM.MouseDelta.Length() > 0;
            Vector2 mousePos = IM.MousePos;
            for (int i = 0; i < actions.Count; i++)
            {
                if (op1[i].Intersects(mousePos))
                {
                    if (movedMouse)
                    {
                        y = i;
                        x = 0;
                    }
                    return true;
                }
                else if (op2[i].Intersects(mousePos))
                {
                    if (movedMouse)
                    {
                        y = i;
                        x = 1;
                    }
                    return true;
                }
                else if (op3[i].Intersects(mousePos))
                {
                    if (movedMouse)
                    {
                        y = i;
                        x = 2;
                    }
                    return true;
                }
            }
            return false;
        }

        private void UpdateRebinding()
        {
            if (IM.IsKeyPressed(Keys.Escape))
            {
                IsSelecting = false;
                return;
            }
            if (!RebindKey())
            {
                RebindMouse();
            }
        }

        private bool RebindKey()
        {
            Keys[] k = IM.GetPressedKeys();
            if (k.Length == 1)
            {
                if (k[0] == Keys.Back)
                {
                    RM.InsertKey(actions[y], new Button(Keys.None), x);
                }
                else
                {
                    RM.InsertKey(actions[y], new Button(k[0]), x);
                }
                SelectedValidKey();
                return true;
            }
            return false;
        }

        private bool RebindMouse()
        {
            MouseButton mb = IM.CurrentMousePressed;
            if (mb != MouseButton.None)
            {
                RM.InsertKey(actions[y], new Button(mb), x);
                SelectedValidKey();
                return true;
            }
            return false;
        }

        private void SelectedValidKey()
        {
            IsSelecting = false;
            CreateControls();
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sb = Fribzel.SpriteBatch;
            SpriteFont sf = RM.Font(Font.DefaultFont);

            sb.DrawString(sf, Text.ConfigMenuNavigation, new Vector2(32, 16), CM.MainMenuFont);
            sb.DrawString(sf, Text.ConfigMenuEscapeEnter, new Vector2(32, 40), CM.MainMenuSelected);

            for (int i = 0; i < actions.Count; i++)
            {
                sb.DrawString(sf, labels[i].Name, labels[i].Position, CM.MainMenuExtra);
                sb.DrawString(sf, op1[i].Name, op1[i].Position, GetHightlightColor(0, i));
                sb.DrawString(sf, op2[i].Name, op2[i].Position, GetHightlightColor(1, i));
                sb.DrawString(sf, op3[i].Name, op3[i].Position, GetHightlightColor(2, i));
            }

            base.Draw(gameTime);
        }

        private Color GetHightlightColor(int col, int row)
        {
            // Not selected: Default color
            // Highlighted: Selected color
            // Highlighted and editing: Extra color
            return y == row && x == col ? IsSelecting ? CM.MainMenuExtra : CM.MainMenuSelected : CM.MainMenuFont;
        }
    }
}
