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
        private const int CHECKPOINT_DISTANCE = 10000; 

        public double sizeFactor = 12;
        private int distTraveled = 0;
        public Background bg;
        public World(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics; 
        }

        public void reset()
        {
            player.reset();
            sizeFactor = 12;
            distTraveled = 0;
            Texture2D tempTex = platforms[0].Texture;
            platforms.Clear();
            addPlatform(new Rectangle(100, 100, 150, 25), tempTex);
            addPlatform(new Rectangle(300, 100, 150, 25), tempTex);
            addPlatform(new Rectangle(550, 100, 150, 25), tempTex);
            addPlatform(new Rectangle(800, 100, 150, 25), tempTex);
        }
    }
}
