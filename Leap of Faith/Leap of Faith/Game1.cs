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
        SpriteFont font;
        Vector2 fontLocation;
        int fontTime;

        Texture2D playerTexture, lightmask, bgHolder, cursor;
        List<Texture2D> backgrounds;
        List<Texture2D> rocks;
        //Texture2D[] bgs;
        Rectangle[] rects;
        Rectangle[] rockrects;
        Background backObj;
        Background rockObj;

        Song bgmusic;

        Effect lightEffect;
        RenderTarget2D scene, mask;
        //Make a player
        Player player;
        List<Texture2D> playerAnimation;
        Texture2D pHolder;

        Animation playerAnim;
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
            world = new World(graphics, this);
            sizeFactor = world.sizeFactor;
            backgrounds = new List<Texture2D>();
            playerAnimation = new List<Texture2D>();
            rocks = new List<Texture2D>();
            //bgs = new Texture2D[3];
            //rects = new Rectangle[bgs.Length];
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
            //background = Content.Load<Texture2D>("background");
            //bg2 = Content.Load<Texture2D>("bg2");
            flameTexture = Content.Load<Texture2D>("Torches/torch1");
            cursor = Content.Load<Texture2D>("cursor");

            bgHolder = Content.Load<Texture2D>("wally");
            backgrounds.Add(bgHolder);
            bgHolder = Content.Load<Texture2D>("wally2");
            backgrounds.Add(bgHolder);
            bgHolder = Content.Load<Texture2D>("wally");
            backgrounds.Add(bgHolder);
            bgHolder = Content.Load<Texture2D>("wally2");
            backgrounds.Add(bgHolder);

            bgHolder = Content.Load<Texture2D>("rocky1");
            rocks.Add(bgHolder);
            bgHolder = Content.Load<Texture2D>("rocky2");
            rocks.Add(bgHolder);
            bgHolder = Content.Load<Texture2D>("rocky3");
            rocks.Add(bgHolder);

            font = Content.Load<SpriteFont>("Font");
            fontLocation = new Vector2(0.0f, graphics.GraphicsDevice.Viewport.Height-25);
            fontTime = 0;

            bgmusic = Content.Load<Song>("Audio/MP3s/song2");

            platformTextures = new Texture2D[17];
            platformTextures[0] = Content.Load<Texture2D>("Platforms/endcap_left");
            platformTextures[1] = Content.Load<Texture2D>("Platforms/endcap_right");

            for (int i = 0; i < platformTextures.Length - 3; i++)
            {
                platformTextures[i + 2] = Content.Load<Texture2D>("Platforms/segment" + Convert.ToString(i+1));
            }

            pHolder = Content.Load<Texture2D>("running/lofwalking10001");
            playerAnimation.Add(pHolder);
            pHolder = Content.Load<Texture2D>("running/lofwalking10002");
            playerAnimation.Add(pHolder);
            pHolder = Content.Load<Texture2D>("running/lofwalking10003");
            playerAnimation.Add(pHolder);
            pHolder = Content.Load<Texture2D>("running/lofwalking10004");
            playerAnimation.Add(pHolder);
            pHolder = Content.Load<Texture2D>("running/lofwalking10005");
            playerAnimation.Add(pHolder);
            pHolder = Content.Load<Texture2D>("running/lofwalking10006");
            playerAnimation.Add(pHolder);
            pHolder = Content.Load<Texture2D>("running/lofwalking10007");
            playerAnimation.Add(pHolder);
            pHolder = Content.Load<Texture2D>("running/lofwalking10008");
            playerAnimation.Add(pHolder);
            pHolder = Content.Load<Texture2D>("running/lofwalking10009");
            playerAnimation.Add(pHolder);
            pHolder = Content.Load<Texture2D>("running/lofwalking10010");
            playerAnimation.Add(pHolder);
            pHolder = Content.Load<Texture2D>("running/lofwalking10011");
            playerAnimation.Add(pHolder);
            pHolder = Content.Load<Texture2D>("running/lofwalking10012");
            playerAnimation.Add(pHolder);

            playerAnim = new Animation(1, playerAnimation);

            world.addPlatform(new Rectangle(100, 200, 150, 25), Content.Load<Texture2D>("Platform"), platformTextures, randomNumber);
            world.addPlatform(new Rectangle(300, 200, 150, 25), Content.Load<Texture2D>("Platform"), platformTextures, randomNumber);
            world.addPlatform(new Rectangle(550, 150, 150, 25), Content.Load<Texture2D>("Platform"), platformTextures, randomNumber);
            world.addPlatform(new Rectangle(800, 175, 150, 25), Content.Load<Texture2D>("Platform"), platformTextures, randomNumber);

            playerTexture = Content.Load<Texture2D>("dude");
            player = new Player(playerTexture, graphics, world, flameTexture);
            player.animCycle = playerAnim;
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            var param = graphics.GraphicsDevice.PresentationParameters;
            scene = new RenderTarget2D(graphics.GraphicsDevice, param.BackBufferWidth, param.BackBufferHeight);
            mask = new RenderTarget2D(graphics.GraphicsDevice, param.BackBufferWidth, param.BackBufferHeight);

            menu = new MainMenu(Content, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));

            MediaPlayer.Play(bgmusic);
            MediaPlayer.IsRepeating = true;

            //Powerups
            torchTexture = Content.Load<Texture2D>("Torches/torch1");
            torchPowerupPos = new Vector2(0,0);
            torchPowerup = new Powerup(torchTexture, torchPowerupPos, 3);

            //bg2 = Content.Load<Texture2D>("bg2");
            //bg3 = Content.Load<Texture2D>("bg3");

            /*bgs[0] = background;
            bgs[1] = background;
            bgs[2] = background;

            rects[0] = new Rectangle(0, 0, background.Width, background.Height);
            rects[1] = new Rectangle(background.Width, 0, background.Width, background.Height);
            rects[2] = new Rectangle(background.Width, 0, background.Width, background.Height);
            */
            rects = new Rectangle[backgrounds.Count];
            rockrects = new Rectangle[rocks.Count];
            rects[0] = new Rectangle(0, 0, backgrounds[0].Width, backgrounds[0].Height);
            rockrects[0] = new Rectangle(0, 0, rocks[0].Width, rocks[0].Height);

            for (int i = 1; i < rects.Length; i++)
            {
                rects[i] = new Rectangle((rects[i - 1].X + rects[i - 1].Width), 0, backgrounds[i].Width, backgrounds[i].Height);
            }

            for (int i = 1; i < rockrects.Length; i++)
            {
                rockrects[i] = new Rectangle((rockrects[i - 1].X + rockrects[i - 1].Width), 0, rocks[i].Width, rocks[i].Height);
            }

            backObj = new Background(rects);
            world.bg = backObj;
            rockObj = new Background(rockrects);
            world.rocks = rockObj;
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
                world.shrinkLight();

                fontTime++;

                currState = Keyboard.GetState();
                // Allows the game to exit
                if (currState.IsKeyDown(Keys.Escape) && !prevState.IsKeyDown(Keys.Escape))
                {
                    menu = new PauseMenu(Content, new Rectangle(graphics.PreferredBackBufferWidth, 0, 500, graphics.PreferredBackBufferHeight), world, torchPowerup);
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
                rockrects = world.rocks.rects;
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
                spriteBatch.Draw(scene, new Vector2(0, 0), Color.Black);
                spriteBatch.End();
                spriteBatch.Begin();
                spriteBatch.DrawString(font, "Torches x" + torchPowerup.getUses(), fontLocation, Color.LightGray);
                spriteBatch.End();
            
            if(menu.isActive)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(Content.Load < Texture2D >("backdrop"), new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), new Color(0.0f, 0.0f, 0.0f, 0.6f));
                menu.draw(spriteBatch, font);
                spriteBatch.Draw(cursor, mouseLoc, Color.White);
                spriteBatch.End();
            }

            else
            {
                //Draw fonts
                spriteBatch.Begin();

                float firstChange = 250;
                float secondChange = 500;
                float thirdChange = 750;

                //Draw controls font
                if (fontTime < firstChange)
                {
                    spriteBatch.DrawString(font, "Arrow keys/A and D to move", new Vector2((graphics.PreferredBackBufferWidth / 2) - 100, fontLocation.Y), Color.LightGray);
                }
                else if (fontTime > firstChange && fontTime < secondChange)
                {
                    spriteBatch.DrawString(font, "Space, Up arrow, or W to jump", new Vector2((graphics.PreferredBackBufferWidth / 2) - 100, fontLocation.Y), Color.LightGray);
                }
                else if (fontTime > secondChange && fontTime < thirdChange)
                {
                    spriteBatch.DrawString(font, "Throw torch with F", new Vector2((graphics.PreferredBackBufferWidth / 2) - 100, fontLocation.Y), Color.LightGray);
                }

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
            float sizeVal = (1.0f - (1.1f / (float)world.sizeFactor));
            if (sizeVal < .05f)
            {
                sizeVal = .05f;
            }

            // Create a Black Background
            spriteBatch.Begin();
           
            for (int i = 0; i < backgrounds.Count; i++)
            {
                spriteBatch.Draw(backgrounds[i], rects[i], Color.White);
            }

            spriteBatch.Draw(Content.Load<Texture2D>("backdrop"), new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), new Color(0.0f, 0.0f, 0.0f, 0.8f));
            for (int i = 0; i < rocks.Count; i++)
            {
                spriteBatch.Draw(rocks[i], rockrects[i], new Color(sizeVal, sizeVal, sizeVal));
            }
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);

            //Draw mask around the player
            spriteBatch.Draw(lightmask, new Rectangle(Convert.ToInt32(player.Location.X - offset + playerTexture.Width / 2), Convert.ToInt32(player.Location.Y - offset + playerTexture.Height / 2),
                Convert.ToInt32(lightmask.Width * sizeFactor), Convert.ToInt32(lightmask.Height * sizeFactor)), Color.LightGray);

            //Draw mask around torch
            for (int i = 0; i < player.NumTorches; i++)
            {
                if (player.getTorch(i).IsThrown == true)
                {
                    
                    spriteBatch.Draw(lightmask, new Rectangle(Convert.ToInt32(player.getTorch(i).Location.X - lightmask.Width / 2 - flameTexture.Width), Convert.ToInt32(player.getTorch(i).Location.Y - lightmask.Height / 2 - flameTexture.Height),
                    Convert.ToInt32(lightmask.Width * 1.25), Convert.ToInt32(lightmask.Height * 1.25)), Color.LightGray);
                }
            }
            spriteBatch.End();

           
            device.SetRenderTarget(null);
        }
    }
}
