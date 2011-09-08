using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Fribzel3D.Management;
using Fribzel3D.ResourceFiles;

namespace Fribzel3D.Screens
{
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
        private int footerHeight = Fribzel.Height - 32;

        private int optionsStart;
        private int fontHeight;

        private int selectedOption;
        private Action backAction;

        private ControlMode controlMode;

        public MenuScreen(ControlMode controlMode)
        {
            this.controlMode = controlMode;
        }

        /// <summary>
        /// Add a selectable menu options
        /// </summary>
        /// <param name="name">The name of the option displayed</param>
        /// <param name="action">The action that should be performed when pressed</param>
        public void AddMenuEntry(string name, Action action)
        {
            MenuOption mo = new MenuOption(name, RM.Font(Font.MenuFont));
            mo.Action = action;
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
            MenuOption mo = new MenuOption(text, RM.Font(Font.MenuFont));
            header.Add(mo);
        }

        /// <summary>
        /// Add footer text
        /// </summary>
        /// <param name="text">the text</param>
        public void AddFooter(string text)
        {
            MenuOption mo = new MenuOption(text, RM.Font(Font.MenuFont));
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
            if (controlMode == ControlMode.Mouse || controlMode == ControlMode.KeyboardAndMouse)
            {
                IM.SnapToCenter = false;
                Fribzel.BaseGame.IsMouseVisible = true;
            }
            else
            {
                IM.SnapToCenter = true;
                Fribzel.BaseGame.IsMouseVisible = false;
            }
            CalculatePositions();
            base.Show();
        }

        public override void Update(GameTime gameTime)
        {
            HandleInputNavigation();

            if (CheckKeyboardAccept() || CheckMouseAccept())
            {
                options[selectedOption].Action();
            }
            else if (RM.IsPressed(InputAction.Back) && backAction != null)
            {
                backAction();
            }

            base.Update(gameTime);
        }

        private void HandleInputNavigation()
        {
            if (controlMode == ControlMode.Keyboard || controlMode == ControlMode.KeyboardAndMouse)
            {
                HandleKeyboardNavigation();
            }
            if (controlMode == ControlMode.KeyboardAndMouse && IM.MouseDelta.Length() != 0)
            {
                CalculateMouseSelection();
            }
            if (controlMode == ControlMode.Mouse)
            {
                if (!CalculateMouseSelection())
                {
                    selectedOption = -1;
                }
            }
        }

        private void HandleKeyboardNavigation()
        {
            if (RM.IsPressed(InputAction.Up))
            {
                selectedOption = CycleIndex(selectedOption - 1, options.Count - 1);
            }
            if (RM.IsPressed(InputAction.Down))
            {
                selectedOption = CycleIndex(selectedOption + 1, options.Count - 1);
            }
        }

        private bool CheckKeyboardAccept()
        {
            if (controlMode == ControlMode.Keyboard || controlMode == ControlMode.KeyboardAndMouse)
            {
                if (RM.IsPressed(InputAction.Accept))
                {
                    return IsSelectedEntryValid();
                }
            }
            return false;
        }

        private bool CheckMouseAccept()
        {
            if (controlMode == ControlMode.KeyboardAndMouse || controlMode == ControlMode.Mouse)
            {
                if (IM.IsLeftMousePressed() && CalculateMouseSelection())
                {
                    return IsSelectedEntryValid();
                }
            }
            return false;
        }

        private bool IsSelectedEntryValid()
        {
            return options.Count > selectedOption && selectedOption >= 0;
        }

        private bool CalculateMouseSelection()
        {
            Vector2 mousePos = IM.MousePos;
            for (int i = 0; i < options.Count; i++)
            {
                if (options[i].Intersects(mousePos))
                {
                    selectedOption = i;
                    return true;
                }
            }
            return false;
        }

        public override void Draw(GameTime gameTime)
        {
            for (int i = 0; i < header.Count; i++)
            {
                Fribzel.SpriteBatch.DrawString(RM.Font(Font.MenuFont), header[i].Name, header[i].Position, CM.MainMenuExtra);
            }

            for (int i = 0; i < options.Count; i++)
            {
                Fribzel.SpriteBatch.DrawString(RM.Font(Font.MenuFont), options[i].Name, options[i].Position, selectedOption == i ? CM.MainMenuSelected : CM.MainMenuFont);
            }

            for (int i = 0; i < footer.Count; i++)
            {
                Fribzel.SpriteBatch.DrawString(RM.Font(Font.MenuFont), footer[i].Name, footer[i].Position, CM.MainMenuExtra);
            }

            base.Draw(gameTime);
        }

        /// <summary>
        /// Calculate the positions for all the text
        /// </summary>
        private void CalculatePositions()
        {
            optionsStart = (Fribzel.Height - optionsHeight) / 2;
            for (int i = 0; i < options.Count; i++)
            {
                options[i].Position = new Vector2((Fribzel.Width - options[i].Size.X) / 2, optionsStart + (fontHeight * i));
            }

            for (int i = 0; i < header.Count; i++)
            {
                header[i].Position = new Vector2((Fribzel.Width - header[i].Size.X) / 2, headerHeight + (header[i].Size.Y * i));
            }

            // they are drawn from the bottom up, so reverse the list
            footer.Reverse();

            for (int i = 0; i < footer.Count; i++)
            {
                footer[i].Position = new Vector2((Fribzel.Width - footer[i].Size.X) / 2, footerHeight - (footer[i].Size.Y * (i + 1)));
            }
        }
    }
}
