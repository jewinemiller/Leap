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
    /// <summary>
    /// The Menu parent class
    /// </summary>
    public class Menu
    {
        //The function that any MenuItem will use. 
        public delegate void actFunction();
        protected MouseState currState, prevState; 
        //The items on the Menu
        public List<MenuItem> items
        {
            get;
            set;
        }

        //The texture to draw for the menu
        public Texture2D MenuTex
        {
            get;
            set;
        }

        //The size and location of the Menu
        public Rectangle bounds
        {
            get;
            set;
        }
        //Whether or not the menu is actively displaying
        public bool isActive
        {
            get;
            set;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="c">Content Manager to load textures</param>
        /// <param name="b">Bounds of the Rectangle</param>
        public Menu(ContentManager c, Rectangle b)
        {
            //Initialization
            bounds = b;
            MenuTex = c.Load<Texture2D>("Menu");
        }

        public virtual void Update(GameTime gameTime)
        {
            currState = Mouse.GetState();
            if (currState.LeftButton == ButtonState.Pressed && prevState.LeftButton != ButtonState.Pressed)
            {
                Rectangle mouseBounds = new Rectangle(currState.X, currState.Y, 5, 5);
                foreach (MenuItem m in items)
                {
                    Rectangle temp = new Rectangle((int)m.location.X, (int)m.location.Y, m.texture.Width, m.texture.Height);
                    if (mouseBounds.Intersects(temp))
                    {
                        onClick(m);
                        break;
                    }
                }
            }
            prevState = currState;
        }

        /// <summary>
        /// Virtual definition of the onClick function.
        /// </summary>
        /// <param name="item">The MenuItem that will be clicked.</param>
        public virtual void onClick(MenuItem item)
        {

        }

        /// <summary>
        /// Draw function. Draws everything on the Menu.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="font"></param>
        public virtual void draw(SpriteBatch sb, SpriteFont font)
        {
            sb.Draw(MenuTex, bounds, Color.White);
            foreach(MenuItem i in items){
                i.draw(sb, font);
            }
        }
    }
}
