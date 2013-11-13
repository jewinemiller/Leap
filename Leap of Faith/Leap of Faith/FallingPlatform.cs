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
    class FallingPlatform:Platform
    {
        public FallingPlatform(Rectangle rect, Texture2D tex, Texture2D[] t, Random r)
            : base(rect, tex, t, r)
        {

        }

        public bool checkPlayerCollision(Player player)
        {
            if (player.VState != Player.VerticalState.none)
            {
                return false; 
            }

            if (player.Body.Intersects(Bounds))
            {
                // && (position.X > p.Bounds.Left - 50 *.75) && (position.X < p.Bounds.Right - 50 * .75)
                if (player.Location.Y + 50 < Bounds.Top + 18)
                {
                    return true;
                }
            }

            /*
            if (player.Location.X >= this.Bounds.X + this.Bounds.Width / 2)
            {
                return true; 
            }*/
            return false; 
        }

        /// <summary>
        /// Drops the platform by a given distance.
        /// </summary>
        /// <param name="distance">The distance for the platform to drop by.</param>
        public void dropPlatform(int distance)
        {
            Bounds = new Rectangle(Bounds.X, Bounds.Y + distance, Bounds.Width, Bounds.Height);
        }

        public bool isAboveGround()
        {
            if (Bounds.Y + Bounds.Height >= 540)
            {
                return false;
            }

            return true;
        }
    }
}
