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
   public class Button : MenuItem
    {
        delegate void actFunction();

        public Button(Vector2 loc, Texture2D tex)
            : base(loc, tex)
        {

        }
     
        public void act(Menu.actFunction func)
        {
            func();
        }
    }
}
