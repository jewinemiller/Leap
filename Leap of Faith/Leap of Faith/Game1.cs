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

        Texture2D playerTexture, lightmask, background, bg2, bg3, cursor;
        Texture2D[] bgs;
        Rectangle[] rects;
        Background backObj;

        Effect lightEffect;
        RenderTarget2D scene, mask;
        //Make a player
        Player player;

        World world;
        Menu menu;
        double sizeFactor;

        //Torches
        Texture2D flameTexture;

        //Powerups
        Texture2D torchTexture;
        Vector2 torchPowerupPos;
        Powerup torchPowerup;

        //Platform textures
        Texture2D[] platformTextures;

        Random randomNumber = new Random();

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
            bgs = new Texture2D[3];
            rects = new Rectangle[bgs.Length];
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
            bg2 = Content.Load<Texture2D>("bg2");
            flameTexture = Content.Load<Texture2D>("Torches/torch1");
            cursor = Content.Load<Texture2D>("cursor");

            platformTextures = new Texture2D[17];
            platformTextures[0] = Content.Load<Texture2D>("Platforms/endcap_left");
            platformTextures[1] = Content.Load<Texture2D>("Platforms/endcap_right");

            for (int i = 0; i < platformTextures.Length - 3; i++)
            {
                platformTextures[i + 2] = Content.Load<Texture2D>("Platforms/segment" + Convert.ToString(i+1));
            }

            world.addPlatform(new Rectangle(100, 100, 150, 25), Content.Load<Texture2D>("Platform"), platformTextures, randomNumber);
            world.addPlatform(new Rectangle(300, 100, 150, 25), Content.Load<Texture2D>("Platform"), platformTextures, randomNumber);
            world.addPlatform(new Rectangle(550, 100, 150, 25), Content.Load<Texture2D>("Platform"), platformTextures, randomNumber);
            world.addPlatform(new Rectangle(800, 100, 150, 25), Content.Load<Texture2D>("Platform"), platformTextures, randomNumber);

            playerTexture = Content.Load<Texture2D>("dude");
            player = new Player(playerTexture, graphics, world, flameTexture);
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            var param = graphics.GraphicsDevice.PresentationParameters;
            scene = new RenderTarget2D(graphics.GraphicsDevice, param.BackBufferWidth, param.BackBufferHeight);
            mask = new RenderTarget2D(graphics.GraphicsDevice, param.BackBufferWidth, param.BackBufferHeight);

            menu = new MainMenu(Content, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));

            //Powerups
            torchTexture = Content.Load<Texture2D>("Torches/torch1");
            torchPowerupPos = new Vector2(0,0);
            torchPowerup = new Powerup(torchTexture, torchPowerupPos, 3);

            //bg2 = Content.Load<Texture2D>("bg2");
            //bg3 = Content.Load<Texture2D>("bg3");

            bgs[0] = background;
            bgs[1] = background;
            bgs[2] = background;

            rects[0] = new Rectangle(0, 0, background.Width, background.Height);
            rects[1] = new Rectangle(background.Width, 0, background.Width, background.Height);
            rects[2] = new Rectangle(background.Width, 0, background.Width, background.Height);

            backObj = new Background(rects);
            world.bg = backObj;
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
        MouseState currMouse, prevMouse;
        Vector2 mouseLoc; 

        protected override void Update(GameTime gameTime)
        {
            if (!menu.isActive)
            {
                currState = Keyboard.GetState();
                // Allows the game to exit
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    this.Exit();
                if (currState.IsKeyDown(Keys.Escape) && !prevState.IsKeyDown(Keys.Escape))
                {
                    menu = new PauseMenu(Content, new Rectangle(graphics.PreferredBackBufferWidth / 2 - 250, 0, 500, graphics.PreferredBackBufferHeight));
                }
                player.move(currState, prevState, torchPowerup);
                player.checkState();

                if (player.Location.Y >= graphics.PreferredBackBufferHeight - player.Body.Height)
                {
                    menu = new GameOver(Content, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), world, torchPowerup);
                }
                world.checkForTorches(player, torchPowerup);

                //Save our kbstate
                prevState = currState;
                sizeFactor = world.sizeFactor;
                rects = world.bg.rects;
                world.checkFallingPlatforms(3);

                /*if (currState.IsKeyDown(Keys.Right) || currState.IsKeyDown(Keys.D))
                {
                    world.movePlatforms(5);
                }*/
            }
            else
            {
                currMouse = Mouse.GetState();
                mouseLoc = new Vector2(currMouse.X, currMouse.Y);
                menu.Update(gameTime);
                prevMouse = currMouse; 
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
                  DrawScene(graphics.GraphicsDevice);
                DrawEffects(graphics.GraphicsDevice);

                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                lightEffect.Parameters["lightMask"].SetValue(mask);
                lightEffect.CurrentTechnique.Passes[0].Apply();
                spriteBatch.Draw(scene, new Vector2(0, 0), Color.White);
                spriteBatch.End();
            
            if(menu.isActive)
            {
                spriteBatch.Begin();
                menu.draw(spriteBatch, null);
                spriteBatch.Draw(cursor, mouseLoc, Color.Black);
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }

        private void DrawScene(GraphicsDevice device)
        {
            double offset = (lightmask.Width / 2) * sizeFactor;   
            device.SetRenderTarget(scene);
            device.Clear(Color.White);

            spriteBatch.Begin();
            player.display(spriteBatch);

            Rectangle bounds = new Rectangle(Convert.ToInt32(player.Location.X - offset + playerTexture.Width / 2), Convert.ToInt32(player.Location.Y - offset + playerTexture.Height / 2),
              Convert.ToInt32(lightmask.Width * sizeFactor), Convert.ToInt32(lightmask.Height * sizeFactor));

            foreach (Platform p in world.getPlatforms())
            {
                if (p.Bounds.Intersects(bounds))
                {
                    p.display(spriteBatch, torchPowerup, bounds);
                }
                for (int i = 0; i < player.NumTorches; i++)
                {
                    if (player.getTorch(i).IsThrown)
                    {
                        Rectangle tBound = new Rectangle(Convert.ToInt32(player.getTorch(i).Location.X - lightmask.Width / 2 - flameTexture.Width), Convert.ToInt32(player.getTorch(i).Location.Y - lightmask.Height / 2 - flameTexture.Height),
                        Convert.ToInt32(lightmask.Width * 1.25), Convert.ToInt32(lightmask.Height * 1.25));
                        if (tBound.Intersects(p.Bounds))
                        {
                            p.display(spriteBatch, torchPowerup, tBound);
                        }
                    }
                }
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
           
            for (int i = 0; i < bgs.Length; i++)
            {
                spriteBatch.Draw(bgs[i], rects[i], Color.White);
            }
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);

            //Draw mask around the player
            spriteBatch.Draw(lightmask, new Rectangle(Convert.ToInt32(player.Location.X - offset + playerTexture.Width / 2), Convert.ToInt32(player.Location.Y - offset + playerTexture.Height / 2),
                Convert.ToInt32(lightmask.Width * sizeFactor), Convert.ToInt32(lightmask.Height * sizeFactor)), Color.White);

            //Draw mask around torch
            for (int i = 0; i < player.NumTorches; i++)
            {
                if (player.getTorch(i).IsThrown == true)
                {
                    
                    spriteBatch.Draw(lightmask, new Rectangle(Convert.ToInt32(player.getTorch(i).Location.X - lightmask.Width / 2 - flameTexture.Width), Convert.ToInt32(player.getTorch(i).Location.Y - lightmask.Height / 2 - flameTexture.Height),
                    Convert.ToInt32(lightmask.Width * 1.25), Convert.ToInt32(lightmask.Height * 1.25)), Color.White);
                }
            }
            spriteBatch.End();

           
            device.SetRenderTarget(null);
        }
    }
}
