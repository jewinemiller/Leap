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
    public class PauseMenu : Menu
    {
       //Buttons to start, quit, and show stats
        private Button resume, options, quit;
        //ContentManager. Used to load textures
        private ContentManager content;

        private SoundEffect buttonClick;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="c">Content Manager</param>
        /// <param name="p">The Player</param>
        /// <param name="b">Bounding Rectangle - Displays the menu of set size</param>
        /// <param name="w">The World that the Game takes place in</param>
        public PauseMenu(ContentManager c, Rectangle b):base(c, b)
        {
            //Initialize Everything
            items = new List<MenuItem>();
          
            resume = new Button(new Vector2(300.0f, 100.0f), c.Load<Texture2D>("start"));
            options = new Button(new Vector2(300.0f, 200.0f), c.Load<Texture2D>("options"));
            quit = new Button(new Vector2(300.0f, 300.0f), c.Load<Texture2D>("quit"));
            buttonClick = c.Load<SoundEffect>("Audio/WAVs/Buttons/button2");

            //Add everything to the array of items.
            isActive = true;
            items.Add(resume);
            items.Add(options);
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
            if(item.Equals(resume)){
                (resume as Button).act(startGame);
                buttonClick.Play();
            }
                //Otherwise, if the button is quit, exit the game
            else if(item.Equals(quit)){
                (quit as Button).act(exit);
                buttonClick.Play();
            }
           
        }

        /// <summary>
        /// Start the Game (simply set a boolean to false)
        /// </summary>
        private void startGame()
        {
            isActive = false;
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
