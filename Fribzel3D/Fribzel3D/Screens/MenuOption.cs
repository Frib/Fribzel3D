using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Fribzel3D.Screens
{
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
