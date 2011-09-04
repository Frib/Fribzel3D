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
        private List<InputAction> labels = new List<InputAction>();
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
            backAction = new Action(() => { RM.SaveConfig(); Fribzel.BaseGame.ShowScreen(ScreenManager.MainMenu()); });
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
            InputAction[] buttons = RM.GetValidInputStrings();
            foreach (InputAction s in buttons)
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
                y = CycleIndex(y - 1, labels.Count - 1);
            }
            if (IM.IsKeyPressed(Keys.Down))
            {
                y = CycleIndex(y + 1, labels.Count - 1);
            }
            if (IM.IsKeyPressed(Keys.Enter))
            {
                IsSelecting = true;
            }
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
                    RM.InsertKey(labels[y], new Button(Keys.None), x);
                }
                else
                {
                    RM.InsertKey(labels[y], new Button(k[0]), x);
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
                RM.InsertKey(labels[y], new Button(mb), x);
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
            int offsetY = 80;
            int quarter = Fribzel.Width / 5;

            SpriteBatch sb = Fribzel.SpriteBatch;
            SpriteFont sf = RM.Font(Font.DefaultFont);

            sb.DrawString(sf, Text.ConfigMenuNavigation, new Vector2(32, 16), CM.MainMenuFont);
            sb.DrawString(sf, Text.ConfigMenuEscapeEnter, new Vector2(32, 40), CM.MainMenuSelected);

            for (int i = 0; i < labels.Count; i++)
            {
                Fribzel.SpriteBatch.DrawString(sf, labels[i].ToString(), new Vector2(quarter * 1, offsetY), CM.MainMenuExtra);

                // Not selected: Default color
                // Highlighted: Selected color
                // Highlighted and editing: Extra color
                sb.DrawString(sf, op1[i], new Vector2(quarter * 2, offsetY), GetHightlightColor(0, i));
                sb.DrawString(sf, op2[i], new Vector2(quarter * 3, offsetY), GetHightlightColor(1, i));
                sb.DrawString(sf, op3[i], new Vector2(quarter * 4, offsetY), GetHightlightColor(2, i));

                offsetY += 24;
            }

            base.Draw(gameTime);
        }

        public Color GetHightlightColor(int col, int row)
        {
            return y == row && x == col ? IsSelecting ? CM.MainMenuExtra : CM.MainMenuSelected : CM.MainMenuFont;
        }
    }
}
