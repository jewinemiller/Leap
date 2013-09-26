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
        float xSpeed = 4f;
        int screenWidth;
        int screenHeight;

        //Player states
        enum VerticalState
        {
            falling,
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
        public Player(Texture2D pTexture, GraphicsDeviceManager gdm, World w)
        {
            //Load the texture and set the vectors
            graphic = gdm;
            screenWidth = gdm.PreferredBackBufferWidth;
            screenHeight = gdm.PreferredBackBufferHeight;
            position = new Vector2(150, 50);
            body = new Rectangle((int)position.X, (int)position.Y, 50, 50);
            texture = pTexture;
            velocity = new Vector2(0, 0);
            world = w; 
            //Set the player state
            hState = HorizontalState.standing;
        }

        /// <summary>
        /// This method displays the player
        /// </summary>
        /// <param name="s">The spritebatch to draw with</param>
        public void display(SpriteBatch s)
        {
            //Display the player
            s.Draw(texture, position, Color.White);
        }

        /// <summary>
        /// This method updates the state of the player based on position and keys pressed.
        /// </summary>
        /// <param name="kbState">This is the current KeyBoardState</param>
        /// <param name="prevState">This is the previouse KeyBoardState</param>
        public void move(KeyboardState kbState, KeyboardState prevState)
        {
            //Print out debugging info
            Console.WriteLine("VSTATE: " + vState);
            Console.WriteLine("HSTATE: " + hState);
            //Console.WriteLine("POSITION: " + " ( " + position.X + ", " + position.Y + " ) ");
            //Console.WriteLine("VELOCITY: " + " ( " + velocity.X + ", " + velocity.Y + " ) ");

            //Set the x velocity to 0 automatically and set the horizontal state to standing by default
            velocity.X = 0;
            hState = HorizontalState.standing;

            //Check key presses to move player
            if (kbState.IsKeyDown(Keys.Left))
            {
                //Move left as long as there is room
                if (position.X > 0 + xSpeed)
                {
                    hState = HorizontalState.walkingLeft;

                    //Move the player to the left 
                    velocity.X = -1 * xSpeed;
                }
            }

            if (kbState.IsKeyDown(Keys.Right))
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
                    world.movePlatforms((int)xSpeed);
                }
            }

            //Have the player jump
            if (kbState.IsKeyDown(Keys.Up) && prevState.IsKeyUp(Keys.Up) && vState == VerticalState.none)
            {
                //Set our new player state
                vState = VerticalState.jumping;
                velocity.Y = -2 * ySpeed;
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
                }
            }

            //Check to see if our player is standing on anything
            if (position.Y >= screenHeight - 50 && vState == VerticalState.falling)
            {
                position.Y = screenHeight - 50;
                vState = VerticalState.none;
                velocity.Y = 0;
            }

            //Add our velocity to our position vector
            position.X += velocity.X;
            position.Y += velocity.Y;

            body.X = (int)position.X;
            body.Y = (int)position.Y;
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

            }
            else if(hState == HorizontalState.walkingRight)
            {

            }
        }
    }
}
