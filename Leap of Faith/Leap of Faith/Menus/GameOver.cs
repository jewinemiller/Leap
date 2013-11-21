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
using System.Xml.Serialization;
using System.IO;
namespace Leap_of_Faith
{
    /// <summary>
    /// Main Menu - Displays at the start of the game
    /// </summary>
    class GameOver : Menu
    {
        //Buttons to start, quit, and show stats
        private Button restart, quit;
        //ContentManager. Used to load textures
        private ContentManager content;
        World world;
        Powerup pUp;
        SoundEffect buttonClick;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="c">Content Manager</param>
        /// <param name="b">Bounding Rectangle - Displays the menu of set size</param>
        public GameOver(ContentManager c, Rectangle b, World w, Powerup power)
            : base(c, b)
        {
            //Initialize Everything
            items = new List<MenuItem>();
            world = w;
            pUp = power;
            restart = new Button(new Vector2(300.0f, 200.0f), c.Load<Texture2D>("restart"));
            quit = new Button(new Vector2(300.0f, 300.0f), c.Load<Texture2D>("quit"));
            buttonClick = c.Load<SoundEffect>("Audio/WAVs/Buttons/button2");

            //Add everything to the array of items.
            isActive = true;
            items.Add(restart);
            items.Add(quit);

            //Finish some initialization
            content = c;
        }

        /// <summary>
        /// onClick Function
        /// Handles the clicking of any of the MenuItems displayed on the screen
        /// </summary>
        /// <param name="item"></param>
        public override void onClick(MenuItem item)
        {
            //If the button is play, start the game
            if (item.Equals(restart))
            {
                (restart as Button).act(restartGame);
                buttonClick.Play();
            }
            //Otherwise, if the button is quit, exit the game
            else if (item.Equals(quit))
            {
                (quit as Button).act(exit);
                buttonClick.Play();
            }

        }

        /// <summary>
        /// Start the Game (simply set a boolean to false)
        /// </summary>
        private void restartGame()
        {
            world.reset(pUp);
            this.isActive = false;
        }

        /// <summary>
        /// Exit the program
        /// </summary>
        private void exit()
        {
            Environment.Exit(0);
        }


    }
}