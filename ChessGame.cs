using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Chess.FSM;
using System.Collections.Generic;
using Chess.Views;
using Chess.Controllers;

namespace Chess
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class ChessGame : Game
    {
        GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private RenderTarget2D screen;

        private int _WIDTH = 256;
        private int _HEIGHT = 128;
        private int scale = 8;

        private List<BaseView> views = new List<BaseView>();
        private List<BaseController> controllers = new List<BaseController>();

        public ChessGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = _WIDTH * scale;
            graphics.PreferredBackBufferHeight = _HEIGHT * scale;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            IsMouseVisible = true;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            screen = new RenderTarget2D(GraphicsDevice, _WIDTH, _HEIGHT);

            // Add all views, in correct order
            views.Add(new BoardView(Content, spriteBatch));
            views.Add(new PiecesView(Content, spriteBatch));

            base.Initialize();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(screen);
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            foreach (var view in views)
            {
                view.Draw();
            }

            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin();
            spriteBatch.Draw(screen, new Rectangle(0, 0, _WIDTH * scale, _HEIGHT * scale), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
