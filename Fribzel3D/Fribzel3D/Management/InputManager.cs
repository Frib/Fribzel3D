using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Fribzel3D.Management
{
    /// <summary>
    /// InputManager. Adds a variety of input helping things, mostly used by the resource manager.
    /// Also has helpful methods like mouse delta, and can force the mouse cursor in the center or within the window.
    /// Is updated at the start of every loop.
    /// </summary>
    public static class IM
    {
        private static MouseState previousMouse = Mouse.GetState();
        private static KeyboardState previousKeyboard = Keyboard.GetState();

        private static KeyboardState currentKeyboard = Keyboard.GetState();
        private static MouseState currentMouse = Mouse.GetState();

        private static Vector2 mouseDelta;

        /// <summary>
        /// Returns the relative mouse movement of this update.
        /// </summary>
        public static Vector2 MouseDelta
        {
            get { return mouseDelta; }
        }

        /// <summary>
        /// Returns the current mouse position
        /// </summary>
        public static Vector2 MousePos
        {
            get { return new Vector2(currentMouse.X, currentMouse.Y); }
        }

        /// <summary>
        /// Returns the scroll wheel difference since last update
        /// </summary>
        public static int ScrollDelta
        {
            get { return currentMouse.ScrollWheelValue - previousMouse.ScrollWheelValue; }
        }

        /// <summary>
        /// Returns all the keys that have been pressed this update (so not held down, but pressed)
        /// </summary>
        /// <returns></returns>
        public static Keys[] GetPressedKeys()
        {
            var pressed = currentKeyboard.GetPressedKeys().ToList();
            var valid = new List<Keys>();
            foreach (Keys k in pressed)
            {
                if (IsKeyPressed(k))
                {
                    valid.Add(k);
                }
            }
            return valid.ToArray();
        }

        /// <summary>
        /// Returns a mousebutton that has been pressed this update, if any.
        /// </summary>
        /// <returns></returns>
        public static MouseButton CurrentMousePressed
        {
            get
            {
                if (IsLeftMousePressed())
                {
                    return MouseButton.Left;
                }
                if (IsRightMousePressed())
                {
                    return MouseButton.Right;
                }
                if (IsMidMousePressed())
                {
                    return MouseButton.Middle;
                }
                if (IsSide1MousePressed())
                {
                    return MouseButton.Side1;
                }
                if (IsSide2MousePressed())
                {
                    return MouseButton.Side2;
                }
                return MouseButton.None;
            }
        }

        /// <summary>
        /// Called every update before everything. Recalculates deltas and stuff
        /// </summary>
        public static void NewState()
        {
            if (Fribzel.HasFocus)
            {
                previousKeyboard = currentKeyboard;
                previousMouse = currentMouse;
                currentKeyboard = Keyboard.GetState();
                currentMouse = Mouse.GetState();

                mouseDelta = new Vector2(currentMouse.X - previousMouse.X, currentMouse.Y - previousMouse.Y);

                ValidateMousePosition();
            }
        }

        private static void ValidateMousePosition()
        {
            if (SnapToCenter)
            {
                Mouse.SetPosition(Fribzel.Width / 2, Fribzel.Height / 2);
                currentMouse = Mouse.GetState();
            }
            else if (StayInWindow)
            {
                Mouse.SetPosition((int)Math.Max(0, Math.Min(Fribzel.Width, MousePos.X)), (int)Math.Max(Math.Min(Fribzel.Height, MousePos.Y), 0));
                currentMouse = Mouse.GetState();
            }
        }

        /// <summary>
        /// Required helper functions for keys and mousebuttons.
        /// Adds if they are held down, are up, or if they have been pressed or released this update.
        /// Requires the game window to be focused, else they always return false.
        /// </summary>
        #region keyboard
        public static bool IsKeyDown(Keys key)
        {
            if (Fribzel.HasFocus)
            {
                return currentKeyboard.IsKeyDown(key);
            }
            return false;
        }

        public static bool IsKeyUp(Keys key)
        {
            if (Fribzel.HasFocus)
            {
                return currentKeyboard.IsKeyUp(key);
            }
            return false;
        }

        public static bool IsKeyPressed(Keys key)
        {
            if (Fribzel.HasFocus)
            {
                return currentKeyboard.IsKeyDown(key) && previousKeyboard.IsKeyUp(key);
            }
            return false;
        }

        public static bool IsKeyReleased(Keys key)
        {
            if (Fribzel.HasFocus)
            {
                return currentKeyboard.IsKeyUp(key) && previousKeyboard.IsKeyDown(key);
            }
            return false;
        }
        #endregion
        #region leftmouse
        public static bool IsLeftMouseDown()
        {
            if (Fribzel.HasFocus)
            {
                return currentMouse.LeftButton == ButtonState.Pressed;
            }
            return false;
        }

        public static bool IsLeftMouseUp()
        {
            if (Fribzel.HasFocus)
            {
                return currentMouse.LeftButton == ButtonState.Released;
            }
            return false;
        }

        public static bool IsLeftMousePressed()
        {
            if (Fribzel.HasFocus)
            {
                return currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released;
            }
            return false;
        }

        public static bool IsLeftMouseReleased()
        {
            if (Fribzel.HasFocus)
            {
                return currentMouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed;
            }
            return false;
        }
        #endregion
        #region rightmouse
        public static bool IsRightMouseDown()
        {
            if (Fribzel.HasFocus)
            {
                return currentMouse.RightButton == ButtonState.Pressed;
            }
            return false;
        }

        public static bool IsRightMouseUp()
        {
            if (Fribzel.HasFocus)
            {
                return currentMouse.RightButton == ButtonState.Released;
            }
            return false;
        }

        public static bool IsRightMousePressed()
        {
            if (Fribzel.HasFocus)
            {
                return currentMouse.RightButton == ButtonState.Pressed && previousMouse.RightButton == ButtonState.Released;
            }
            return false;
        }

        public static bool IsRightMouseReleased()
        {
            if (Fribzel.HasFocus)
            {
                return currentMouse.RightButton == ButtonState.Released && previousMouse.RightButton == ButtonState.Pressed;
            }
            return false;
        }
        #endregion
        #region middlemouse
        public static bool IsMidMouseDown()
        {
            if (Fribzel.HasFocus)
            {
                return currentMouse.MiddleButton == ButtonState.Pressed;
            }
            return false;
        }

        public static bool IsMidMouseUp()
        {
            if (Fribzel.HasFocus)
            {
                return currentMouse.MiddleButton == ButtonState.Released;
            }
            return false;
        }

        public static bool IsMidMousePressed()
        {
            if (Fribzel.HasFocus)
            {
                return currentMouse.MiddleButton == ButtonState.Pressed && previousMouse.MiddleButton == ButtonState.Released;
            }
            return false;
        }

        public static bool IsMidMouseReleased()
        {
            if (Fribzel.HasFocus)
            {
                return currentMouse.MiddleButton == ButtonState.Released && previousMouse.MiddleButton == ButtonState.Pressed;
            }
            return false;
        }
        #endregion
        #region sidebutton1
        public static bool IsSide1MouseDown()
        {
            if (Fribzel.HasFocus)
            {
                return currentMouse.XButton1 == ButtonState.Pressed;
            }
            return false;
        }

        public static bool IsSide1MouseUp()
        {
            if (Fribzel.HasFocus)
            {
                return currentMouse.XButton1 == ButtonState.Released;
            }
            return false;
        }

        public static bool IsSide1MousePressed()
        {
            if (Fribzel.HasFocus)
            {
                return currentMouse.XButton1 == ButtonState.Pressed && previousMouse.XButton1 == ButtonState.Released;
            }
            return false;
        }

        public static bool IsSide1MouseReleased()
        {
            if (Fribzel.HasFocus)
            {
                return currentMouse.XButton1 == ButtonState.Released && previousMouse.XButton1 == ButtonState.Pressed;
            }
            return false;
        }
        #endregion
        #region sidebutton2
        public static bool IsSide2MouseDown()
        {
            if (Fribzel.HasFocus)
            {
                return currentMouse.XButton2 == ButtonState.Pressed;
            }
            return false;
        }

        public static bool IsSide2MouseUp()
        {
            if (Fribzel.HasFocus)
            {
                return currentMouse.XButton2 == ButtonState.Released;
            }
            return false;
        }

        public static bool IsSide2MousePressed()
        {
            if (Fribzel.HasFocus)
            {
                return currentMouse.XButton2 == ButtonState.Pressed && previousMouse.XButton2 == ButtonState.Released;
            }
            return false;
        }

        public static bool IsSide2MouseReleased()
        {
            if (Fribzel.HasFocus)
            {
                return currentMouse.XButton2 == ButtonState.Released && previousMouse.XButton2 == ButtonState.Pressed;
            }
            return false;
        }
        #endregion

        /// <summary>
        /// should the mouse snap to the center of the window?
        /// </summary>
        public static bool SnapToCenter { get; set; }

        /// <summary>
        /// Should the mouse stay within the borders of the window?
        /// </summary>
        public static bool StayInWindow { get; set; }
    }
}
