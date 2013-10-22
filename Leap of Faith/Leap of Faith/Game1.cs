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

namespace Leap_of_Faith
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D playerTexture, lightmask, background;
        Effect lightEffect;
        RenderTarget2D scene, mask;
        //Make a player
        Player player;

        World world;

        double sizeFactor;

        //Torches
        Texture2D flameTexture;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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
            world = new World(graphics);
            sizeFactor = world.sizeFactor; 
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            lightmask = Content.Load<Texture2D>("lightmask");
            lightEffect = Content.Load<Effect>("lighting");
            background = Content.Load<Texture2D>("background");
            flameTexture = Content.Load<Texture2D>("torch");

            world.addPlatform(new Rectangle(100, 100, 150, 25),Content.Load<Texture2D>("Platform"));
            world.addPlatform(new Rectangle(300, 100, 150, 25), Content.Load<Texture2D>("Platform"));
            world.addPlatform(new Rectangle(550, 100, 150, 25), Content.Load<Texture2D>("Platform"));
            world.addPlatform(new Rectangle(800, 100, 150, 25), Content.Load<Texture2D>("Platform"));

            playerTexture = Content.Load<Texture2D>("dude");
            player = new Player(playerTexture, graphics, world, flameTexture);
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            var param = graphics.GraphicsDevice.PresentationParameters;
            scene = new RenderTarget2D(graphics.GraphicsDevice, param.BackBufferWidth, param.BackBufferHeight);
            mask = new RenderTarget2D(graphics.GraphicsDevice, param.BackBufferWidth, param.BackBufferHeight);


        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
        /// 

        KeyboardState prevState, currState; 

        protected override void Update(GameTime gameTime)
        {
            currState = Keyboard.GetState();
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            player.move(currState, prevState);
            player.checkState();

            //Save our kbstate
            prevState = currState;
            sizeFactor = world.sizeFactor;

            world.checkFallingPlatforms(3);
          
            /*if (currState.IsKeyDown(Keys.Right) || currState.IsKeyDown(Keys.D))
            {
                world.movePlatforms(5);
            }*/
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            DrawScene(graphics.GraphicsDevice);
            DrawEffects(graphics.GraphicsDevice);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            lightEffect.Parameters["lightMask"].SetValue(mask);
            lightEffect.CurrentTechnique.Passes[0].Apply();
            spriteBatch.Draw(scene, new Vector2(0, 0), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawScene(GraphicsDevice device)
        {
            device.SetRenderTarget(scene);
            device.Clear(Color.White);

            spriteBatch.Begin();
            player.display(spriteBatch);
            foreach (Platform p in world.getPlatforms())
            {
                spriteBatch.Draw(p.Texture, p.Bounds, Color.Black);
            }
            spriteBatch.End();
        }


        private void DrawEffects(GraphicsDevice device)
        {
            double offset = (lightmask.Width / 2) * sizeFactor;
            device.SetRenderTarget(mask);
            device.Clear(Color.Black);

            // Create a Black Background
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Vector2(0, 0), new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);

            //Draw mask around the player
            spriteBatch.Draw(lightmask, new Rectangle(Convert.ToInt32(player.Location.X - offset + playerTexture.Width / 2), Convert.ToInt32(player.Location.Y - offset + playerTexture.Height / 2),
                Convert.ToInt32(lightmask.Width * sizeFactor), Convert.ToInt32(lightmask.Height * sizeFactor)), Color.White);

            //Draw mask around torch
           /* for (int i = 0; i < player.NumTorches; i++)
            {
                if (player.getTorch(i).IsThrown == true)
                {
                    sizeFactor = 2;
                    spriteBatch.Draw(lightmask, new Rectangle(Convert.ToInt32(player.getTorch(i).Location.X + flameTexture.Width / 2), Convert.ToInt32(player.getTorch(i).Location.Y + flameTexture.Height / 2),
                    Convert.ToInt32(lightmask.Width * sizeFactor), Convert.ToInt32(lightmask.Height * sizeFactor)), Color.White);
                }
            }*/
            spriteBatch.End();

           
            device.SetRenderTarget(null);
        }
    }
}
