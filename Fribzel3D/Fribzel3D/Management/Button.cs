using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Fribzel3D.Management
{


    /// <summary>
    /// A button! Can be a keyboard button or a mouse button.
    /// </summary>
    public class Button
    {
        private Keys key;

        public Keys Key
        {
            get { return key; }
        }
        private bool left;

        public bool Left
        {
            get { return left; }
        }
        private bool middle;

        public bool Middle
        {
            get { return middle; }
        }
        private bool right;

        public bool Right
        {
            get { return right; }
        }
        private bool side1;

        public bool Side1
        {
            get { return side1; }
        }
        private bool side2;

        public bool Side2
        {
            get { return side2; }
        }

        /// <summary>
        /// Create a generic button linked to a keyboard button
        /// </summary>
        /// <param name="key"></param>
        public Button(Keys key)
        {
            this.key = key;
        }

        /// <summary>
        /// Create a generic button linked to a mouse button
        /// </summary>
        /// <param name="button"></param>
        public Button(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left: left = true; break;
                case MouseButton.Middle: middle = true; break;
                case MouseButton.Right: right = true; break;
                case MouseButton.Side1: side1 = true; break;
                case MouseButton.Side2: side2 = true; break;
                default: break;
            }
        }

        public override string ToString()
        {
            if (key != Keys.None)
            {
                return key.ToString();
            }
            else if (left)
            {
                return "LeftMouse";
            }
            else if (right)
            {
                return "RightMouse";
            }
            else if (middle)
            {
                return "MiddleMouse";
            }
            else if (side1)
            {
                return "Side1Mouse";
            }
            else if (side2)
            {
                return "Side2Mouse";
            }
            return "<EMPTY>";
        }

        /// <summary>
        /// If this key is currently bound to anything
        /// </summary>
        public bool IsBound
        {
            get
            {
                return key != Keys.None || left || middle || right || side1 || side2;
            }
        }

        public bool IsDown()
        {
            if (Key != Keys.None && IM.IsKeyDown(Key))
            {
                return true;
            }
            if (Left && IM.IsLeftMouseDown())
            {
                return true;
            }
            if (Right && IM.IsRightMouseDown())
            {
                return true;
            }
            if (Middle && IM.IsMidMouseDown())
            {
                return true;
            }
            if (Side1 && IM.IsSide1MouseDown())
            {
                return true;
            }
            if (Side2 && IM.IsSide2MouseDown())
            {
                return true;
            }
            return false;
        }

        public bool IsUp()
        {
            if (Key != Keys.None && IM.IsKeyUp(Key))
            {
                return true;
            }
            if (Left && IM.IsLeftMouseUp())
            {
                return true;
            }
            if (Right && IM.IsRightMouseUp())
            {
                return true;
            }
            if (Middle && IM.IsMidMouseUp())
            {
                return true;
            }
            if (Side1 && IM.IsSide1MouseUp())
            {
                return true;
            }
            if (Side2 && IM.IsSide2MouseUp())
            {
                return true;
            }
            return false;
        }

        public bool IsPressed()
        {
            if (Key != Keys.None && IM.IsKeyPressed(Key))
            {
                return true;
            }
            if (Left && IM.IsLeftMousePressed())
            {
                return true;
            }
            if (Right && IM.IsRightMousePressed())
            {
                return true;
            }
            if (Middle && IM.IsMidMousePressed())
            {
                return true;
            }
            if (Side1 && IM.IsSide1MousePressed())
            {
                return true;
            }
            if (Side2 && IM.IsSide2MousePressed())
            {
                return true;
            }
            return false;
        }

        public bool IsReleased()
        {
            if (Key != Keys.None && IM.IsKeyReleased(Key))
            {
                return true;
            }
            if (Left && IM.IsLeftMouseReleased())
            {
                return true;
            }
            if (Right && IM.IsRightMouseReleased())
            {
                return true;
            }
            if (Middle && IM.IsMidMouseReleased())
            {
                return true;
            }
            if (Side1 && IM.IsSide1MouseReleased())
            {
                return true;
            }
            if (Side2 && IM.IsSide2MouseReleased())
            {
                return true;
            }
            return false;
        }
    }
}
