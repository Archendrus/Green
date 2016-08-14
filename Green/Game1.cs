using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace Green
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteBatch targetBatch;
        RenderTarget2D target;
        Vector2 Scale;
        Rectangle screenRectangle;

        KeyboardState oldState;

        TileMap tileMap;

        SpriteFont font;

        List<Goo> gooList;
        Texture2D gooTexture;

        List<Sprite> chargeList;
        Texture2D chargeTexture;

        BoxManager boxManager;
        HumanManager humanManager;

        Texture2D fgMachine;
        Sprite fgMachineSp;

        Viewport newViewport;
        Camera2D camera;

        float endingElapsed;

        int score;

        enum GameState
        {
            Game,
            Ending
        }

        GameState currentState;

        // DEBUG
        Texture2D pixel;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 400  *2;
            graphics.PreferredBackBufferHeight = 240 * 2;

            // Fullscreen
            //graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            //graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            IsFixedTimeStep = false;

            // Fullscreen
            //graphics.HardwareModeSwitch = false;
            //graphics.IsFullScreen = true;
            //graphics.ApplyChanges();


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
            // TODO: Add your initialization logic here
            screenRectangle = new Rectangle(
                0, 0, 400, 240);

            newViewport = new Viewport(0, 0, 400, 240);
            GraphicsDevice.Viewport = newViewport;

            camera = new Camera2D(GraphicsDevice.Viewport);

            Scale = new Vector2(1, 1);

            score = 0;

            currentState = GameState.Game;


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            targetBatch = new SpriteBatch(GraphicsDevice);
            target = new RenderTarget2D(GraphicsDevice, 400, 240);

            font = Content.Load<SpriteFont>("emulogic");

            // TODO: use this.Content to load your game content here
            Texture2D temp;

            temp = Content.Load<Texture2D>("tiles");
            tileMap = new TileMap(temp, Scale);

            boxManager = new BoxManager(this.Content, Scale, screenRectangle);

            gooTexture = Content.Load<Texture2D>("goo");
            gooList = new List<Goo>();

            chargeTexture = Content.Load<Texture2D>("charge");
            chargeList = new List<Sprite>();


            // DEBUG
            pixel = new Texture2D(GraphicsDevice,1,1,false,SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White });


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
            KeyboardState newState = Keyboard.GetState();
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (currentState)
            {
                case GameState.Game:
                    {
                        GameStateUpdate(gameTime, newState);
                        break;
                    }
                case GameState.Ending:
                    {
                        GameStateUpdate(gameTime, newState);
                        EndingStateUpdate(gameTime);
                        break;
                    }
            }

            oldState = newState;
            base.Update(gameTime);
        }

        private void GameStateUpdate(GameTime gameTime, KeyboardState newState)
        {
            // fire goo
            if (newState.IsKeyDown(Keys.Down) && oldState.IsKeyUp(Keys.Down))
            {
                if (chargeList.Count > 0)
                {
                    gooList.Add(new Goo(gooTexture, new Vector2(192, 48 + 240), Scale, chargeList.Count));
                    chargeList.Clear();
                }
            }

            // charge machine
            if (newState.IsKeyDown(Keys.Up) && oldState.IsKeyUp(Keys.Up))
            {
                if (chargeList.Count < 5)
                {
                    float xPos = 160 + chargeList.Count * 16;
                    chargeList.Add(new Sprite(chargeTexture, new Vector2(xPos, 0 + 240), Scale));
                }
            }


            if (newState.IsKeyDown(Keys.X))
            {
                ChangeState(GameState.Ending);
                //camera.Position -= new Vector2(0, 250) * elapsed;
            }

            //tileMap.Update(gameTime);

            // Move boxes
            boxManager.Update(gameTime, ref score);

            // Update goos
            for (int i = 0; i < gooList.Count; i++)
            {
                gooList[i].Update(gameTime);

                // Check collision with boxes
                for (int j = 0; j < boxManager.Boxes.Count; j++)
                {
                    if (gooList[i].HitBox.Intersects(boxManager.Boxes[j].HitBox))
                    {
                        boxManager.FillBox(boxManager.Boxes[j], gooList[i].Charges);
                        gooList[i].Kill();
                    }
                }

                // Check collision with belt
                if (gooList[i].Position.Y > 144 + 240 && gooList[i].IsAlive)
                {
                    gooList[i].Kill();
                }
            }

            // Cleanup dead goos
            for (int i = 0; i < gooList.Count; i++)
            {
                if (!gooList[i].IsAlive)
                {
                    gooList.Remove(gooList[i]);
                }
            }

            // Win condtion
            if (score == 20 && currentState != GameState.Ending)
            {
                ChangeState(GameState.Ending);
            }
        }

        private void EndingStateUpdate(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            endingElapsed += elapsed;
            float delay = 3f;

            if (endingElapsed > delay)
            {
                humanManager.Update(gameTime);
                if (camera.Position.Y > 0)
                {
                    camera.Position -= new Vector2(0, 25) * elapsed;
                }
            }

            Console.WriteLine(camera.Position);

        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(target);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            var viewMatrix = camera.GetViewMatrix();
            // TODO: Add your drawing code here
            // Draw to screen
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise,
                null,
                viewMatrix);

            // Draw map
            tileMap.Draw(spriteBatch);

            // Draw goos
            for (int i = 0; i < gooList.Count; i++)
            {
                gooList[i].Draw(spriteBatch);
                //spriteBatch.Draw(pixel, gooList[i].HitBox, Color.White);
            }

            // Draw boxes
            boxManager.Draw(spriteBatch, pixel);

            if (currentState == GameState.Ending)
            {
                humanManager.Draw(spriteBatch);
                fgMachineSp.Draw(spriteBatch);
            }

            // Draw charges
            for (int i = 0; i < chargeList.Count; i++)
            {
                chargeList[i].Draw(spriteBatch);
            }

            spriteBatch.DrawString(font,score + "/20", new Vector2(300, 0 + 240), Color.White);

            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            targetBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise);
            
            // Fullscreen draw
            //targetBatch.Draw(target, new Rectangle(0, 0, GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height), Color.White);
            targetBatch.Draw(target, new Rectangle(0, 0, 400 * 2, 240 * 2), Color.White);

            targetBatch.End();

            base.Draw(gameTime);
        }

        private void ChangeState(GameState newState)
        {
            switch (newState)
            {
                case GameState.Game:
                    {
                        // Empty for now
                        break;
                    }
                case GameState.Ending:
                    {
                        fgMachine = Content.Load<Texture2D>("fgMachine");
                        fgMachineSp = new Sprite(fgMachine, new Vector2(192,192), Scale);
                        humanManager = new HumanManager(Content);
                        break;
                    }
            }

            currentState = newState;
        }

    }
}
