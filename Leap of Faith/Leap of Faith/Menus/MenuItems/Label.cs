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
    /// Label Class
    /// Displays a string on Menus.
    /// That is all.
    /// Seriously.
    /// That is all.
    /// </summary>
    class Label : MenuItem
    {
        public string Text
        {
            get;
            set;
        }

        public Label(Vector2 loc, Texture2D texture, string text)
            : base(loc, texture)
        {
            Text = text;
        }

        public override void draw(SpriteBatch sb, SpriteFont drawFont)
        {
            sb.DrawString(drawFont, Text, location, Color.White);
        }
    }
}
