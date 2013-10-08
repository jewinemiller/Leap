using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Leap_of_Faith
{
    class Torch
    {
        //Variables
        Vector2 position;
        Vector2 velocity;
        float gravity = 0.2f;
        Boolean isThrown = false;
        Player player;
        float xSpeed = 5f;
        float ySpeed = 5f;
        float burnTime;

        Rectangle bounds;
        Texture2D flame;

        //Constructor
        public Torch(Player p, Texture2D f)
        {
            player = p;

            position = p.Location;
            velocity = new Vector2(0, 0);
            bounds = new Rectangle((int)position.X, (int)position.Y, 20, 20);
            flame = f;
        }

        //Draw the torch
        public void display(SpriteBatch s)
        {
            if (isThrown)
            {
                s.Draw(flame, bounds, Color.White);
            }
        }

        //Update
        public void update()
        {
            if (isThrown)
            {
                if (burnTime > 0)
                {
                    velocity.X = xSpeed;
                    velocity.Y += gravity;

                    position.X += velocity.X;
                    position.Y += velocity.Y;

                    bounds.X = (int)position.X;
                    bounds.Y = (int)position.Y;

                    burnTime--;
                }
                else
                {
                    isThrown = false;
                }
            }
            else
            {
                position = player.Location;
            }
        }

        //Throw the torch
        public void throwTorch()
        {
            isThrown = true;
            velocity.Y = -1 * ySpeed;
            burnTime = 500;
        }


        //Property for thrown
        public bool IsThrown
        {
            get { return isThrown; }
            set { isThrown = value; }
        }

        public Vector2 Location
        {
            get { return position; }
            set { position = value; }
        }

        public float BurnTime
        {
            get { return burnTime; }
            set { burnTime = value; }
        }
    }
}
