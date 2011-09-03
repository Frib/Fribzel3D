using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Reflection;
using System.IO;
using System.Globalization;

namespace Fribzel3D
{
    /// <summary>
    /// ResourceManager, handling dictionaries for fonts, inputs, textures, etc
    /// </summary>
    public static class RM
    {
        private static Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();
        private static Dictionary<string, List<Button>> input = new Dictionary<string, List<Button>>();

        /// <summary>
        /// Add a font to the manager
        /// </summary>
        /// <param name="name">The name this font is referenced by</param>
        /// <param name="font">The font itself</param>
        public static void AddFont(string name, SpriteFont font)
        {
            fonts.Add(name, font);
        }

        /// <summary>
        /// Gets the specified font
        /// </summary>
        /// <param name="name">The name of the font</param>
        /// <returns></returns>
        public static SpriteFont Font(string name)
        {
            return fonts[name];
        }

        /// <summary>
        /// Initialize the input dictionary lists.
        /// </summary>
        static RM()
        {
            foreach (string name in GetValidInputStrings())
            {
                input.Add(name.ToUpperInvariant(), new List<Button>());
            }
        }

        /// <summary>
        /// Get an array with all the inputs that need to be bound
        /// </summary>
        /// <returns></returns>
        public static string[] GetValidInputStrings()
        {
            List<string> result = new List<string>();

            Type resourceType = Type.GetType("Fribzel3D.Input");
            PropertyInfo[] resourceProps = resourceType.GetProperties(
                BindingFlags.NonPublic |
                BindingFlags.Static |
                BindingFlags.GetProperty);

            foreach (PropertyInfo info in resourceProps)
            {
                if (info.Name != "ResourceManager" && info.Name != "Culture")
                {
                    result.Add(info.Name);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Add a button to a specified input
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="linkedButton"></param>
        public static void AddKey(string keyName, Button linkedButton)
        {
            keyName = keyName.ToUpperInvariant();
            if (string.IsNullOrWhiteSpace(keyName))
            {
                return;
            }
            if (input.ContainsKey(keyName))
            {
                input[keyName].Add(linkedButton);
            }
        }

        /// <summary>
        /// Insert a button to a specified input into the input list, potentially replacing an old one.
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="linkedButton"></param>
        /// <param name="position"></param>
        public static void InsertKey(string keyName, Button linkedButton, int position)
        {
            keyName = keyName.ToUpperInvariant();
            if (linkedButton == null || !input.ContainsKey(keyName))
            {
                return;
            }
            if (linkedButton.Key == Keys.Enter || linkedButton.Key == Keys.Escape)
            {
                return;
            }


            if (input[keyName].Count <= position)
            {
                AddKey(keyName, linkedButton);
            }
            else
            {
                if (input[keyName][position].Key != Keys.Escape && input[keyName][position].Key != Keys.Enter)
                {
                    input[keyName][position] = linkedButton;
                }
            }
        }

        /// <summary>
        /// Check if the specified input button is held down
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        internal static bool IsDown(string p)
        {
            p = p.ToUpperInvariant();
            foreach (Button i in input[p])
            {
                if (IM.IsKeyDown(i.Key))
                    return true;
                if (i.Left && IM.IsLeftMouseDown())
                {
                    return true;
                }
                if (i.Right && IM.IsRightMouseDown())
                {
                    return true;
                }
                if (i.Middle && IM.IsMidMouseDown())
                {
                    return true;
                }
                if (i.Side1 && IM.IsSide1MouseDown())
                {
                    return true;
                }
                if (i.Side2 && IM.IsSide2MouseDown())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Check if the specified input button is not held down
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        internal static bool IsUp(string p)
        {
            p = p.ToUpperInvariant();
            foreach (Button i in input[p])
            {
                if (IM.IsKeyUp(i.Key))
                    return true;
                if (i.Left && IM.IsLeftMouseUp())
                {
                    return true;
                }
                if (i.Right && IM.IsRightMouseUp())
                {
                    return true;
                }
                if (i.Middle && IM.IsMidMouseUp())
                {
                    return true;
                }
                if (i.Side1 && IM.IsSide1MouseUp())
                {
                    return true;
                }
                if (i.Side2 && IM.IsSide2MouseUp())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Check if the specified input button has been pressed this update
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        internal static bool IsPressed(string p)
        {
            p = p.ToUpperInvariant();
            foreach (Button i in input[p])
            {
                if (IM.IsKeyPressed(i.Key))
                    return true;
                if (i.Left && IM.IsLeftMousePressed())
                {
                    return true;
                }
                if (i.Right && IM.IsRightMousePressed())
                {
                    return true;
                }
                if (i.Middle && IM.IsMidMousePressed())
                {
                    return true;
                }
                if (i.Side1 && IM.IsSide1MousePressed())
                {
                    return true;
                }
                if (i.Side2 && IM.IsSide2MousePressed())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Check if the specified input button has been released this update
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        internal static bool IsReleased(string p)
        {
            p = p.ToUpperInvariant();
            foreach (Button i in input[p])
            {
                if (IM.IsKeyReleased(i.Key))
                    return true;
                if (i.Left && IM.IsLeftMouseReleased())
                {
                    return true;
                }
                if (i.Right && IM.IsRightMouseReleased())
                {
                    return true;
                }
                if (i.Middle && IM.IsMidMouseReleased())
                {
                    return true;
                }
                if (i.Side1 && IM.IsSide1MouseReleased())
                {
                    return true;
                }
                if (i.Side2 && IM.IsSide2MouseReleased())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Configure the keys from config text files.
        /// </summary>
        public static void ConfigureKeys()
        {
            if (!File.Exists("config.txt"))
            {
                if (File.Exists("defaultconfig.txt"))
                {
                    File.Copy("defaultconfig.txt", "config.txt", true);
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("//Autogenerated, might be missing some things");
                    sb.AppendLine("Up:Up,W");
                    sb.AppendLine("down:Down,S");
                    sb.AppendLine("left:Left,A");
                    sb.AppendLine("right:Right,D");
                    sb.AppendLine("back:escape");
                    sb.AppendLine("accept:enter");
                    File.WriteAllText("defaultconfig.txt", sb.ToString());

                    File.Copy("defaultconfig.txt", "config.txt", true);
                }
            }

            string[] lines = File.ReadAllLines("config.txt");
            foreach (string line in lines.Where((s) => !s.StartsWith("//", StringComparison.OrdinalIgnoreCase) && s.Contains(':')))
            {
                string key = new string(line.Split(':')[0].Where((char c) => { return !char.IsWhiteSpace(c); }).ToArray()).ToUpperInvariant();
                string values = new string(line.Split(':')[1].Where((char c) => { return !char.IsWhiteSpace(c); }).ToArray()).ToUpperInvariant();

                foreach (string val in values.Split(','))
                {
                    Button b;
                    if (val == "LEFTMOUSE")
                    {
                        b = new Button(MouseButton.Left);
                    }
                    else if (val == "MIDDLEMOUSE")
                    {
                        b = new Button(MouseButton.Middle);
                    }
                    else if (val == "RIGHTMOUSE")
                    {
                        b = new Button(MouseButton.Right);
                    }
                    else if (val == "SIDE1MOUSE")
                    {
                        b = new Button(MouseButton.Side1);
                    }
                    else if (val == "SIDE2MOUSE")
                    {
                        b = new Button(MouseButton.Side2);
                    }
                    else
                    {
                        Keys k = (Keys)Enum.Parse(typeof(Keys), val, true);
                        b = new Button(k);
                    }
                    if (b != null)
                    {
                        AddKey(key, b);
                    }
                }
            }
        }



        /// <summary>
        /// Get all the Buttons linked to the specified name
        /// </summary>
        /// <param name="name">The name for the buttons</param>
        /// <returns>The list with all the buttons</returns>
        public static List<Button> GetButtons(string name)
        {
            return input[name.ToUpperInvariant()];
        }

        /// <summary>
        /// Save the current key configuration to the config text file
        /// </summary>
        internal static void SaveConfig()
        {
            StringBuilder sb = new StringBuilder();
            if (File.Exists("config.txt"))
            {
                string[] lines = File.ReadAllLines("config.txt");
                foreach (string line in lines)
                {
                    if (line.StartsWith("//"))
                    {
                        sb.AppendLine(line);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            
            foreach (string s in input.Keys)
            {
                sb.Append(s + ":\t");
                bool added = false;
                foreach (Button b in input[s])
                {
                    if (b.IsBound)
                    {
                        if (added)
                        {
                            sb.Append(",\t");
                        }
                        added = true;
                        sb.Append(b.ToString());
                    }
                }
                sb.AppendLine("");
            }

            File.WriteAllText("config.txt", sb.ToString());
        }
    }

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
            if (left)
            {
                return "LeftMouse";
            }
            if (right)
            {
                return "RightMouse";
            }
            if (middle)
            {
                return "MiddleMouse";
            }
            if (side1)
            {
                return "Side1Mouse";
            }
            if (side2)
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
    }

    public enum MouseButton
    {
        Left, Middle, Right, Side1, Side2,
        None
    }

    /// <summary>
    /// ScreenManager. Add your screen creation stuff here
    /// </summary>
    public static class SM
    {
        public static Screen MainMenu()
        {
            MenuScreen ms = new MenuScreen();
            ms.AddMenuEntry("Play", new Action(() => { Game1.BaseGame.ShowScreen(SM.GameScreen()); }));
            ms.AddMenuEntry("Options", new Action(() => { Game1.BaseGame.ShowScreen(SM.ControlScreen()); }));
            ms.AddMenuEntry("Quit", new Action(() => { Game1.BaseGame.Exit(); }));

            ms.AddHeader("Use " + RM.GetButtons(Input.Up)[0].ToString() + " and " + RM.GetButtons(Input.Down)[0].ToString() + " to navigate");
            ms.AddHeader("Press " + RM.GetButtons(Input.Accept)[0].ToString() + " to accept and " + RM.GetButtons(Input.Back)[0].ToString() + " to go back");

            ms.AddFooter("Made by Frib");
            return ms;
        }

        public static Screen ControlScreen()
        {
            return new ControlScreen();
        }

        public static Screen GameScreen()
        {
            return new MenuScreen();
        }
    }

    /// <summary>
    /// ColorManager. Add colors here
    /// </summary>
    public static class CM
    {
        public static readonly Color Background = Color.Black;
        public static readonly Color MainMenuFont = Color.White;
        public static readonly Color MainMenuSelected = Color.Yellow;
        public static readonly Color MainMenuExtra = Color.Green;
    }

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
        public static MouseButton GetPressedMouse()
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

        /// <summary>
        /// Called every update before everything. Recalculates deltas and stuff
        /// </summary>
        public static void NewState()
        {
            if (Game1.HasFocus)
            {
                previousKeyboard = currentKeyboard;
                previousMouse = currentMouse;
                currentKeyboard = Keyboard.GetState();
                currentMouse = Mouse.GetState();

                mouseDelta = new Vector2(currentMouse.X - previousMouse.X, currentMouse.Y - previousMouse.Y);

                if (SnapToCenter)
                {
                    Mouse.SetPosition(Game1.Width / 2, Game1.Height / 2);
                    currentMouse = Mouse.GetState();
                }
                else if (StayInWindow)
                {
                    Mouse.SetPosition((int)Math.Max(0, Math.Min(Game1.Width, MousePos.X)), (int)Math.Max(Math.Min(Game1.Height, MousePos.Y), 0));
                    currentMouse = Mouse.GetState();
                }
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
            if (Game1.HasFocus)
            {
                return currentKeyboard.IsKeyDown(key);
            }
            return false;
        }

        public static bool IsKeyUp(Keys key)
        {
            if (Game1.HasFocus)
            {
                return currentKeyboard.IsKeyUp(key);
            }
            return false;
        }

        public static bool IsKeyPressed(Keys key)
        {
            if (Game1.HasFocus)
            {
                return currentKeyboard.IsKeyDown(key) && previousKeyboard.IsKeyUp(key);
            }
            return false;
        }

        public static bool IsKeyReleased(Keys key)
        {
            if (Game1.HasFocus)
            {
                return currentKeyboard.IsKeyUp(key) && previousKeyboard.IsKeyDown(key);
            }
            return false;
        }
        #endregion
        #region leftmouse
        public static bool IsLeftMouseDown()
        {
            if (Game1.HasFocus)
            {
                return currentMouse.LeftButton == ButtonState.Pressed;
            }
            return false;
        }

        public static bool IsLeftMouseUp()
        {
            if (Game1.HasFocus)
            {
                return currentMouse.LeftButton == ButtonState.Released;
            }
            return false;
        }

        public static bool IsLeftMousePressed()
        {
            if (Game1.HasFocus)
            {
                return currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released;
            }
            return false;
        }

        public static bool IsLeftMouseReleased()
        {
            if (Game1.HasFocus)
            {
                return currentMouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed;
            }
            return false;
        }
        #endregion
        #region rightmouse
        public static bool IsRightMouseDown()
        {
            if (Game1.HasFocus)
            {
                return currentMouse.RightButton == ButtonState.Pressed;
            }
            return false;
        }

        public static bool IsRightMouseUp()
        {
            if (Game1.HasFocus)
            {
                return currentMouse.RightButton == ButtonState.Released;
            }
            return false;
        }

        public static bool IsRightMousePressed()
        {
            if (Game1.HasFocus)
            {
                return currentMouse.RightButton == ButtonState.Pressed && previousMouse.RightButton == ButtonState.Released;
            }
            return false;
        }

        public static bool IsRightMouseReleased()
        {
            if (Game1.HasFocus)
            {
                return currentMouse.RightButton == ButtonState.Released && previousMouse.RightButton == ButtonState.Pressed;
            }
            return false;
        }
        #endregion
        #region middlemouse
        public static bool IsMidMouseDown()
        {
            if (Game1.HasFocus)
            {
                return currentMouse.MiddleButton == ButtonState.Pressed;
            }
            return false;
        }

        public static bool IsMidMouseUp()
        {
            if (Game1.HasFocus)
            {
                return currentMouse.MiddleButton == ButtonState.Released;
            }
            return false;
        }

        public static bool IsMidMousePressed()
        {
            if (Game1.HasFocus)
            {
                return currentMouse.MiddleButton == ButtonState.Pressed && previousMouse.MiddleButton == ButtonState.Released;
            }
            return false;
        }

        public static bool IsMidMouseReleased()
        {
            if (Game1.HasFocus)
            {
                return currentMouse.MiddleButton == ButtonState.Released && previousMouse.MiddleButton == ButtonState.Pressed;
            }
            return false;
        }
        #endregion
        #region sidebutton1
        public static bool IsSide1MouseDown()
        {
            if (Game1.HasFocus)
            {
                return currentMouse.XButton1 == ButtonState.Pressed;
            }
            return false;
        }

        public static bool IsSide1MouseUp()
        {
            if (Game1.HasFocus)
            {
                return currentMouse.XButton1 == ButtonState.Released;
            }
            return false;
        }

        public static bool IsSide1MousePressed()
        {
            if (Game1.HasFocus)
            {
                return currentMouse.XButton1 == ButtonState.Pressed && previousMouse.XButton1 == ButtonState.Released;
            }
            return false;
        }

        public static bool IsSide1MouseReleased()
        {
            if (Game1.HasFocus)
            {
                return currentMouse.XButton1 == ButtonState.Released && previousMouse.XButton1 == ButtonState.Pressed;
            }
            return false;
        }
        #endregion
        #region sidebutton2
        public static bool IsSide2MouseDown()
        {
            if (Game1.HasFocus)
            {
                return currentMouse.XButton2 == ButtonState.Pressed;
            }
            return false;
        }

        public static bool IsSide2MouseUp()
        {
            if (Game1.HasFocus)
            {
                return currentMouse.XButton2 == ButtonState.Released;
            }
            return false;
        }

        public static bool IsSide2MousePressed()
        {
            if (Game1.HasFocus)
            {
                return currentMouse.XButton2 == ButtonState.Pressed && previousMouse.XButton2 == ButtonState.Released;
            }
            return false;
        }

        public static bool IsSide2MouseReleased()
        {
            if (Game1.HasFocus)
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
