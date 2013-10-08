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
        public FallingPlatform(Rectangle rect, Texture2D tex)
            : base(rect, tex)
        {

        }

        public bool checkPlayerCollision(Player player)
        {
            if (player.VState != Player.VerticalState.none)
            {
                return false; 
            }

            if (player.Location.X >= this.Bounds.X + this.Bounds.Width / 2)
            {
                return true; 
            }
            return false; 
        }

    }
}
