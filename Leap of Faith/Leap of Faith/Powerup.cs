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

namespace WindowsGame2
{
    class Powerup
    {
        //Variables
        Texture2D powerupTexture;
        Vector2 position;
        Rectangle bounds;
        float width;
        float height;
        float numUses;
        bool collected;

        //Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="texture">Texture used to draw with</param>
        /// <param name="pos">Position of the powerup</param>
        /// <param name="uses">Number of uses</param>
        public Powerup(Texture2D texture, Vector2 pos, float uses)
        {
            powerupTexture = texture;
            position = pos;
            width = 20;
            height = 20;
            bounds = new Rectangle((int)pos.X, (int)pos.Y, (int)width, (int)height);
            numUses = uses;
        }

        //Use the powerup
        public void use(Func<int> method)
        {
            if (numUses > 0)
            {
                numUses--;

                //WHATEVER EFFECT THE POWERUP HAS WILL BE USED HERE
                method();
            }
        }

        //Pick up another one
        public void pickup()
        {
            numUses++;
            collected = true;
        }

        //Display the powerup
        public void display(SpriteBatch s)
        {
            if (!collected)
                s.Draw(powerupTexture, bounds, Color.White);
        }

        //Move the powerup icon when the player picks it up
        public void relocate(int x, int y)
        {
            position.X += x;
            position.Y += y;
            bounds.Offset(x, y);
            collected = false;
        }
    }
}
