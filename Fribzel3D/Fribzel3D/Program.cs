using System;

namespace Fribzel3D
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Fribzel game = new Fribzel())
            {
                game.Run();
            }
        }
    }
#endif
}

