using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Leap_of_Faith
{
    partial class World
    {
        private Player player;
        public Player Player
        {
            get { return player; }
            set { player = value; }
        }
    }
    class Player
    {
        //Variables
        Texture2D texture;
        Rectangle body;
        Vector2 position;
        Vector2 velocity;
        GraphicsDeviceManager graphic;
        private World world; 
        float gravity = 0.25f;
        float ySpeed = 5f;
        float xSpeed = 6f;
        int screenWidth;
        int screenHeight;

        //Torches stuff
        Texture2D flameTexture;
        public Torch[] torches;
        int currTorch = 0;
        SoundEffect burning;
        public SoundEffectInstance burning2;

        // Platform player is currently on.
        public Platform currentPlatform = null;

        //Player states
        public enum VerticalState
        {
            falling,
            fallingPlatform,
            jumping,
            none
        }

        enum HorizontalState
        {
            walkingLeft,
            walkingRight,
            standing
        }
        VerticalState vState;
        HorizontalState hState;

        /// <summary>
        /// The parameterized constructor for the player class
        /// </summary>
        /// <param name="pTexture">The texture that the player will use</param>
        /// <param name="g">The GraphicsDeviceManager that will be used</param>
        public Player(Texture2D pTexture, GraphicsDeviceManager gdm, World w, Texture2D fTexture)
        {
            //Load the texture and set the vectors
            graphic = gdm;
            screenWidth = gdm.PreferredBackBufferWidth;
            screenHeight = gdm.PreferredBackBufferHeight;
            position = new Vector2(150, 50);
            body = new Rectangle((int)position.X, (int)position.Y, 50, 50);
            //TODO: add content manager
            
            burning = w.game.Content.Load<SoundEffect>("Audio/WAVs/fire");
            burning2 = burning.CreateInstance();

            texture = pTexture;
            flameTexture = fTexture;
            velocity = new Vector2(0, 0);
            world = w;
            world.Player = this; 
            //Set the player state
            hState = HorizontalState.standing;

            //Make an array of three torches
            torches = new Torch[999];
            for (int i = 0; i < torches.Length; i++)
            {
                torches[i] = new Torch(this, fTexture, w);
            }
        }

        /// <summary>
        /// This method displays the player
        /// </summary>
        /// <param name="s">The spritebatch to draw with</param>
        public void display(SpriteBatch s)
        {
            //Display the player
            s.Draw(texture, position, Color.White);

            //Draw torches, if any
            for (int i = 0; i < torches.Length; i++)
            {
                torches[i].display(s);
            }
        }

        /// <summary>
        /// This method updates the state of the player based on position and keys pressed.
        /// </summary>
        /// <param name="kbState">This is the current KeyBoardState</param>
        /// <param name="prevState">This is the previouse KeyBoardState</param>
        public void move(KeyboardState kbState, KeyboardState prevState, Powerup tPower)
        {
            //Print out debugging info
            //Console.WriteLine("VSTATE: " + vState);
            //Console.WriteLine("HSTATE: " + hState);
            //Console.WriteLine("POSITION: " + " ( " + position.X + ", " + position.Y + " ) ");
            //Console.WriteLine("VELOCITY: " + " ( " + velocity.X + ", " + velocity.Y + " ) ");

            //Set the x velocity to 0 automatically and set the horizontal state to standing by default
            velocity.X = 0;
            hState = HorizontalState.standing;

            //Check key presses to move player
            if (kbState.IsKeyDown(Keys.Left) && kbState.IsKeyUp(Keys.Right))
            {
                //Move left as long as there is room
                if (position.X > 0 + xSpeed)
                {
                    hState = HorizontalState.walkingLeft;

                    //Move the player to the left 
                    velocity.X = -1 * xSpeed;
                }
            }

            if (kbState.IsKeyDown(Keys.Right) && kbState.IsKeyUp(Keys.Left))
            {
                //Move right as long as there is room
                if (position.X < ((float)screenWidth - xSpeed)/2 - texture.Width / 2)
                {
                    hState = HorizontalState.walkingRight;

                    //Move the player to the right
                    velocity.X = xSpeed;
                }
                else
                {
                    world.bg.scroll((int)xSpeed);
                    world.movePlatforms((int)xSpeed);
                    for (int i = 0; i < torches.Length; i++)
                    {
                        if (torches[i].Falling == false)
                        {
                            torches[i].Location = new Vector2(torches[i].Location.X - xSpeed, torches[i].Location.Y);
                        }
                    }

                    hState = HorizontalState.walkingRight;
                }
            }

            // Jumping/Falling
            if (kbState.IsKeyDown(Keys.Up) || kbState.IsKeyDown(Keys.Space)) // If the player is currently pressing up.
            {
                if (vState == VerticalState.none || vState == VerticalState.fallingPlatform) // If the player's veritcal state is none (not falling or jumping) or the player is on a Falling Platform.
                {
                    currentPlatform = null;
                    vState = VerticalState.jumping;
                    velocity.Y = -2 * ySpeed;
                }
            }
            else if (vState == VerticalState.jumping) // If the player is currently not pressing up and is jumping.
            {
                // Stop the jump.
                vState = VerticalState.falling;
                velocity.Y = -0.5f * velocity.Y;
            }

            //Check for collision detection
            if(vState == VerticalState.falling)
            {
                foreach (Platform p in world.getPlatforms())
                {
                    if (body.Intersects(p.Bounds))
                    {
                        // && (position.X > p.Bounds.Left - 50 *.75) && (position.X < p.Bounds.Right - 50 * .75)
                        if (position.Y + 50 < p.Bounds.Top + 18)
                        {
                            position.Y = p.Bounds.Top - 49;
                            vState = VerticalState.none;
                            velocity.Y = 0;

                            currentPlatform = p;
                        }
                    }
                }
            }
            if (vState == VerticalState.none)
            {
                bool isColliding = false;
                foreach (Platform p in world.getPlatforms())
                {
                    if (body.Intersects(p.Bounds))
                    {
                        isColliding = true;
                    }
                }

                if (!isColliding)
                {
                    vState = VerticalState.falling;
                    currentPlatform = null;
                }
            }
            if (vState == VerticalState.fallingPlatform) // Check to see if the player is still on the platform horizontally.
            {
                if (position.X + 50 < currentPlatform.Bounds.Left || position.X > currentPlatform.Bounds.Right)
                {
                    vState = VerticalState.falling;
                    currentPlatform = null;
                }
            }

            if (position.Y >= screenHeight - 50) // If the player is at the bottom of the screen.
            {
                if (vState == VerticalState.falling || vState == VerticalState.fallingPlatform) // If the player's vertical state is falling or fallingPlatform.
                {
                    world.reset(tPower);
                }
            }

            //Add our velocity to our position vector
            position.X += velocity.X;
            position.Y += velocity.Y;

            body.X = (int)position.X;
            body.Y = (int)position.Y;

            //Check to see if player is throwing torches
            throwTorches(kbState, prevState, tPower);

            //Update all torches, if any
            for (int i = 0; i < torches.Length; i++)
            {
                torches[i].update();
            }
        } 
  
        /// <summary>
        /// This method tells the player what to do based on what state he is in
        /// </summary>
        public void checkState()
        {
            if (vState == VerticalState.falling)
            {
                //Subtract gravity
                velocity.Y += gravity;
            }
            else if (vState == VerticalState.jumping)
            {
                //Subtract gravity
                velocity.Y += gravity;

                //Check to see if the player needs to start falling
                if (velocity.Y >= 0)
                {
                    vState = VerticalState.falling;
                }
            }
            else if (hState == HorizontalState.standing)
            {
                //If the horizontal state is standing, then the player is neither jumping or falling
                vState = VerticalState.none;
            }
            else if (hState == HorizontalState.walkingLeft)
            {
                vState = VerticalState.none;
            }
            else if(hState == HorizontalState.walkingRight)
            {
                vState = VerticalState.none;
            }
        }

        public Vector2 Location
        {
            get { return this.position; }
            set { this.position = value; }
        }

        public VerticalState VState
        {
            get { return this.vState; }
            set { vState = value; }
        }

        public int NumTorches
        {
            get { return torches.Length; }
        }

        public Torch getTorch(int index)
        {
            if (index < torches.Length)
                return torches[index];
            else
                return null;
        }

        public float XSpeed
        {
            get { return xSpeed; }
            set { xSpeed = value; }
        }

        public Rectangle Body
        {
            get { return body; }
            set { body = value; }
        }

        //Throw torches
        public void throwTorches(KeyboardState currState, KeyboardState prevState, Powerup powerup)
        {
            if (powerup.getUses() > 0)
            {
                if (currState.IsKeyDown(Keys.F) && prevState.IsKeyUp(Keys.F) && currTorch < torches.Length)
                {
                    powerup.use();
                    torches[currTorch].throwTorch();
                    currTorch++;
                    burning2.Play();
                    
                    // here
                }
            }
        }

        public void reset(Powerup tp)
        {
            position = new Vector2(150, 50);
            body = new Rectangle((int)position.X, (int)position.Y, 50, 50);
            velocity = new Vector2(0, 0);
            hState = HorizontalState.standing;

            //Make an array of three torches
            torches = new Torch[999];
            for (int i = 0; i < torches.Length; i++)
            {
                torches[i] = new Torch(this, flameTexture, world);
            }
            vState = VerticalState.none;
            tp.setUses(3);
        }
      
    }
}
