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

        Texture2D playerTexture;

        //Make a player
        Player player;

        World world;
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
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            world.addPlatform(new Rectangle(100, 100, 150, 25),Content.Load<Texture2D>("Platform"));
            world.addPlatform(new Rectangle(300, 100, 150, 25), Content.Load<Texture2D>("Platform"));
            world.addPlatform(new Rectangle(550, 100, 150, 25), Content.Load<Texture2D>("Platform"));
            world.addPlatform(new Rectangle(800, 100, 150, 25), Content.Load<Texture2D>("Platform"));

            playerTexture = Content.Load<Texture2D>("dude");
            player = new Player(playerTexture, graphics, world);
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
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

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            player.display(spriteBatch);
            foreach (Platform p in world.getPlatforms())
            {
                spriteBatch.Draw(p.Texture, p.Bounds, Color.Black);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
