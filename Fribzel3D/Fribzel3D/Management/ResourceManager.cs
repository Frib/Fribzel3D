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

namespace Fribzel3D.Management
{
    /// <summary>
    /// ResourceManager, handling dictionaries for fonts, inputs, textures, etc
    /// </summary>
    public static class RM
    {
        private static Dictionary<Font, SpriteFont> fonts = new Dictionary<Font, SpriteFont>();
        private static Dictionary<InputAction, List<Button>> input = new Dictionary<InputAction, List<Button>>();

        /// <summary>
        /// Add a font to the manager
        /// </summary>
        /// <param name="name">The name this font is referenced by</param>
        /// <param name="font">The font itself</param>
        public static void AddFont(Font name, SpriteFont font)
        {
            fonts.Add(name, font);
        }

        /// <summary>
        /// Gets the specified font
        /// </summary>
        /// <param name="name">The name of the font</param>
        /// <returns></returns>
        public static SpriteFont Font(Font name)
        {
            return fonts[name];
        }

        /// <summary>
        /// Initialize the input dictionary lists.
        /// </summary>
        static RM()
        {
            foreach (InputAction ia in GetValidInputActions())
            {
                input.Add(ia, new List<Button>());
            }
        }

        /// <summary>
        /// Get an array with all the inputs that need to be bound
        /// </summary>
        /// <returns></returns>
        public static InputAction[] GetValidInputActions()
        {
            List<InputAction> result = new List<InputAction>();

            foreach (string s in Enum.GetNames(typeof(InputAction)))
            {
                result.Add((InputAction)Enum.Parse(typeof(InputAction), s));
            }

            return result.ToArray();
        }

        /// <summary>
        /// Add a button to a specified input
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="linkedButton"></param>
        public static void AddKey(InputAction ia, Button linkedButton)
        {
            input[ia].Add(linkedButton);
        }

        /// <summary>
        /// Insert a button to a specified input into the input list, potentially replacing an old one.
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="linkedButton"></param>
        /// <param name="position"></param>
        public static void InsertKey(InputAction ia, Button linkedButton, int position)
        {
            if (linkedButton == null || linkedButton.Key == Keys.Enter || linkedButton.Key == Keys.Escape)
            {
                return;
            }
            if (input[ia].Count <= position)
            {
                AddKey(ia, linkedButton);
            }
            else
            {
                if (input[ia][position].Key != Keys.Escape && input[ia][position].Key != Keys.Enter)
                {
                    input[ia][position] = linkedButton;
                }
            }
        }

        /// <summary>
        /// Check if the specified input button is held down
        /// </summary>
        /// <param name="ia"></param>
        /// <returns></returns>
        internal static bool IsDown(InputAction ia)
        {
            foreach (Button b in input[ia])
            {
                if (b.IsDown())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Check if the specified input button is not held down
        /// </summary>
        /// <param name="ia"></param>
        /// <returns></returns>
        internal static bool IsUp(InputAction ia)
        {
            foreach (Button b in input[ia])
            {
                if (b.IsUp())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Check if the specified input button has been pressed this update
        /// </summary>
        /// <param name="ia"></param>
        /// <returns></returns>
        internal static bool IsPressed(InputAction ia)
        {
            foreach (Button b in input[ia])
            {
                if (b.IsPressed())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Check if the specified input button has been released this update
        /// </summary>
        /// <param name="ia"></param>
        /// <returns></returns>
        internal static bool IsReleased(InputAction ia)
        {
            foreach (Button b in input[ia])
            {
                if (b.IsReleased())
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
            ValidateConfigFiles();

            string[] lines = File.ReadAllLines("config.txt");
            foreach (string line in lines.Where((s) => !s.StartsWith("//", StringComparison.OrdinalIgnoreCase) && s.Contains(':')))
            {
                string key = new string(line.Split(':')[0].Where((char c) => { return !char.IsWhiteSpace(c); }).ToArray()).ToUpperInvariant();
                string values = new string(line.Split(':')[1].Where((char c) => { return !char.IsWhiteSpace(c); }).ToArray()).ToUpperInvariant();

                foreach (string val in values.Split(','))
                {
                    AddButton(key, val);
                }
            }
        }

        private static void AddButton(string inputaction, string button)
        {
            Button b = CreateButtonFromText(button);
            if (b != null && b.IsBound)
            {
                InputAction ia;
                if (Enum.TryParse<InputAction>(inputaction, true, out ia))
                {
                    AddKey(ia, b);
                }
            }
        }

        private static Button CreateButtonFromText(string val)
        {
            switch (val)
            {
                case ("LEFTMOUSE"): return new Button(MouseButton.Left);
                case ("MIDDLEMOUSE"): return new Button(MouseButton.Middle);
                case ("RIGHTMOUSE"): return new Button(MouseButton.Right);
                case ("SIDE1MOUSE"): return new Button(MouseButton.Side1);
                case ("SIDE2MOUSE"): return new Button(MouseButton.Side2);
                default: break;
            }
            try
            {
                Keys k = (Keys)Enum.Parse(typeof(Keys), val, true);
                return new Button(k);
            }
            catch (ArgumentException)
            {
                return new Button(Keys.None);
            }
        }

        private static void ValidateConfigFiles()
        {
            if (!File.Exists("config.txt"))
            {
                if (File.Exists("defaultconfig.txt"))
                {
                    File.Copy("defaultconfig.txt", "config.txt", true);
                }
                else
                {
                    CreateDefaultConfigFiles();
                }
            }
        }

        private static void CreateDefaultConfigFiles()
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

        /// <summary>
        /// Get all the Buttons linked to the specified name
        /// </summary>
        /// <param name="name">The name for the buttons</param>
        /// <returns>The list with all the buttons</returns>
        internal static List<Button> GetButtons(InputAction ia)
        {
            return input[ia];
        }

        internal static string GetFirstMappedButton(InputAction ia)
        {
            if (input[ia].Count > 0)
            {
                return input[ia][0].ToString();
            }
            return "<EMPTY>";
        }

        /// <summary>
        /// Save the current key configuration to the config text file
        /// </summary>
        internal static void SaveConfig()
        {
            StringBuilder sb = new StringBuilder();
            if (File.Exists("config.txt"))
            {
                AppendCommentsFromConfigFile(sb);
            }

            foreach (InputAction ia in input.Keys)
            {
                sb.Append(ia + ":\t");
                bool added = false;
                foreach (Button b in input[ia])
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

        private static void AppendCommentsFromConfigFile(StringBuilder sb)
        {
            string[] lines = File.ReadAllLines("config.txt");
            foreach (string line in lines)
            {
                if (line.StartsWith("//", StringComparison.OrdinalIgnoreCase))
                {
                    sb.AppendLine(line);
                }
                else
                {
                    break;
                }
            }
        }
    }
}
