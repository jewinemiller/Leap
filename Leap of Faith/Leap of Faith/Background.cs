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
    public class Background
    {
        int counter = 0;
        public Rectangle[] rects;
        public Background(Rectangle[] rect)
        {
            rects = rect;
        }
        public Rectangle[] scroll(int speed)
        {
       
                if (counter != rects.Length - 1)
                {
                    if ((rects[counter].X + rects[counter].Width) > 0)
                    {
                        rects[counter].X -= speed;
                        rects[counter + 1].X -= speed;
                    }
                    else
                    {
                        rects[counter].X = rects[counter].Width;
                        counter++;
                    }

                }
                else if (counter == rects.Length - 1)
                {
                    if ((rects[counter].X + rects[counter].Width) >= 0)
                    {
                        rects[counter].X -= speed;
                        rects[0].X -= speed;
                    }
                    else
                    {
                        rects[counter].X = rects[counter].Width;
                        counter = 0;
                    }
                }
                Console.Out.WriteLine("drawing rect " + counter);
           
          /*  for (int i = 0; i < rects.Length; i++)
            {
                if ((i == counter && i != rects.Length-1) && i != rects.Length-1)
                {
                    if ((rects[i].X + rects[i].Width) > 0)
                    {
                        rects[i].X -= speed;
                        rects[i + 1].X -= speed;
                    }
                    else
                    {
                        counter++;
                    }

                }
                else if (i == rects.Length - 1)
                {
                    if ((rects[i].X + rects[i].Width) >= 0)
                    {
                        rects[i].X -= speed;
                        rects[0].X -= speed;
                    }
                    else
                    {
                        counter = 0;
                    }
                }
                Console.Out.WriteLine("drawing rect " + counter);
            }*/
            return rects;
        }
    }
}
