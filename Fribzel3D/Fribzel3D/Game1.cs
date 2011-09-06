using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Globalization;
using Fribzel3D.Screens;
using Fribzel3D.Management;
using Fribzel3D.ResourceFiles;

namespace Fribzel3D
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Fribzel : Microsoft.Xna.Framework.Game
    {
        private static GraphicsDeviceManager graphics;

        /// <summary>
        /// The graphics device manager
        /// </summary>
        public static GraphicsDeviceManager Graphics
        {
            get { return Fribzel.graphics; }
        }
        private static SpriteBatch spriteBatch;

        /// <summary>
        /// The spritebatch
        /// </summary>
        public static SpriteBatch SpriteBatch
        {
            get { return Fribzel.spriteBatch; }
        }
        private static Fribzel game;

        /// <summary>
        /// this game
        /// </summary>
        public static Fribzel BaseGame
        {
            get { return Fribzel.game; }
        }

        /// <summary>
        /// The width of the viewport
        /// </summary>
        public static int Width 
        {
            get { return game.GraphicsDevice.Viewport.Width; }
        }

        /// <summary>
        /// The height of the viewport
        /// </summary>
        public static int Height
        {
            get { return game.GraphicsDevice.Viewport.Height; }
        }

        /// <summary>
        /// Is the window currently focused?
        /// </summary>
        public static bool HasFocus
        {
            get { return game.IsActive; }
        }

        Screen currentScreen;

        public Fribzel()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            game = this;            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            RM.ConfigureKeys();
            ShowScreen(ScreenManager.MainMenu());
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            RM.AddFont(Font.MenuFont, Content.Load<SpriteFont>("MenuFont"));
            RM.AddFont(Font.DefaultFont, Content.Load<SpriteFont>("DefaultFont"));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            IM.NewState();

            currentScreen.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(CM.Background);

            spriteBatch.Begin();
            currentScreen.Draw(gameTime);

            // debug stuff! allocated memory and framerate
            spriteBatch.DrawString(RM.Font(Font.DefaultFont), GC.GetTotalMemory(true).ToString(CultureInfo.InvariantCulture), new Vector2(0, 0), Color.White);
            SpriteBatch.DrawString(RM.Font(Font.DefaultFont), gameTime.ElapsedGameTime.TotalMilliseconds.ToString(CultureInfo.InvariantCulture), new Vector2(0, 32), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Hide the current screen (if any), then show the given screen
        /// </summary>
        /// <param name="screen"></param>
        public void ShowScreen(Screen screen)
        {
            if (screen != null)
            {
                SwitchScreen(screen);
            }
        }

        private void SwitchScreen(Screen screen)
        {
            if (currentScreen != null)
            {
                currentScreen.Hide();
            }
            currentScreen = screen;
            currentScreen.Show();
        }
    }
}
