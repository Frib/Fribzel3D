using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Fribzel3D.Screens
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

        protected static int CycleIndex(int newValue, int max, int min = 0)
        {
            return newValue < 0 ? max : newValue > max ? min : newValue;
        }
    }
}
