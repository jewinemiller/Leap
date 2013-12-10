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

        public double sizeFactor = 3.5;
        private int distTraveled = 0;
        public Background bg;
        public Game1 game;
        public World(GraphicsDeviceManager graphics, Game1 g)
        {
            this.graphics = graphics;
            game = g;
        }

        public void reset(Powerup p)
        {
            player.reset(p);
            sizeFactor = 3.5;
            distTraveled = 0;
            Texture2D tempTex = platforms[0].Texture;
            Texture2D[] textures = platforms[0].Textures;
            Random rand = platforms[0].rand;
            platforms.Clear();
            addPlatform(new Rectangle(100, 100, 150, 25), tempTex, textures, rand);
            addPlatform(new Rectangle(300, 100, 150, 25), tempTex, textures, rand);
            addPlatform(new Rectangle(550, 100, 150, 25), tempTex, textures, rand);
            addPlatform(new Rectangle(800, 100, 150, 25), tempTex, textures, rand);
        }

        public void shrinkLight()
        {
            if (sizeFactor > 1.1)
                sizeFactor -= 0.002;
        }
    }
}
