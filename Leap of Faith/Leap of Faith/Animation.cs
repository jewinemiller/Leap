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
    class Animation
    {
        int speed = 0;
        public List<Texture2D> animCycle;
        public int currFrame = 0;
        public Animation(int s, List<Texture2D> anim)
        {
            speed = s;
            animCycle = anim;
        }

        public Texture2D nextFrame()
        {
            if (currFrame != animCycle.Count - 1)
                currFrame++;
            else
            {
                currFrame = 0;
            }
            Console.WriteLine(currFrame);
            return animCycle[currFrame];
        }

        public Texture2D lastFrame()
        {
            if (currFrame != 0)
            {
                currFrame--;
            }
            else
            {
                currFrame = animCycle.Count - 1;
            }
            Console.WriteLine(currFrame);
            return animCycle[currFrame];
        }
    }
}
