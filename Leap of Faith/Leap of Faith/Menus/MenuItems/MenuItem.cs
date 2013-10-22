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
    //Abstract Definition of the MenuItem Class
    public abstract class MenuItem
    {
        //Where the item will go
       public Vector2 location
        {
            get;
            set;
        }

        //How the item will display
       public Texture2D texture
        {
            get;
            set;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="tex"></param>
        public MenuItem(Vector2 loc, Texture2D tex)
        {
            location = loc;
            texture = tex;
        }

       
        /// <summary>
        /// Draw the MenuItem. 
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="font"></param>
        public virtual void draw(SpriteBatch sb, SpriteFont font)
        {
            sb.Draw(texture, location, Color.White);
        }
    }
}
