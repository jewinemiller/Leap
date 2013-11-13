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
            height = 50;
            bounds = new Rectangle((int)pos.X, (int)pos.Y, (int)width, (int)height);
            numUses = uses;
        }

        public Rectangle Bounds
        {
            get { return bounds; }
            set { bounds = value; }
        }

        //Use the powerup
        public void use()
        {
            if (numUses > 0)
            {
                numUses--;
            }
        }

        public float getUses()
        {
            return numUses;
        }

        public void setUses(float u)
        {
            numUses = u;
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
        public void relocate(Platform p)
        {
            position.X = p.Bounds.X;
            position.Y = p.Bounds.Y - 50;

            bounds = new Rectangle((int)position.X + p.Bounds.Width / 2 - 10, (int)position.Y, (int)width, (int)height);
            collected = false;
        }

        //Property for collected
        public bool Collected
        {
            get { return collected; }
            set { collected = value; }
        }
    }
}
