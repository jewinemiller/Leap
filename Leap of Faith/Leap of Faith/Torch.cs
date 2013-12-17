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
        Boolean falling = false;

        Rectangle bounds;
        Texture2D flame;
        World world;

        //Constructor
        public Torch(Player p, Texture2D f, World w)
        {
            player = p;

            position = p.Location;
            velocity = new Vector2(0, 0);
            bounds = new Rectangle((int)position.X, (int)position.Y, 20, 50);
            flame = f;

            world = w;
        }

        //Draw the torch
        public void display(SpriteBatch s)
        {
            if (isThrown)
            {
                s.Draw(flame, bounds, Color.Yellow);
            }
        }

        //Update
        public void update()
        {
            if (isThrown)// && falling)
            {
                if (burnTime > 0)
                {
                    foreach (Platform p in world.getPlatforms())
                    {
                        if (bounds.Intersects(p.Bounds))
                        {
                            if (position.Y + 50 < p.Bounds.Top + 18)
                            {
                                position.Y = p.Bounds.Top - 49;
                                velocity.Y = 0;
                                falling = false;
                            }
                        }
                    }

                    if (falling)
                    {
                        velocity.X = xSpeed;
                        velocity.Y += gravity;

                        position.X += velocity.X;
                        position.Y += velocity.Y;

                        bounds.X = (int)position.X;
                        bounds.Y = (int)position.Y;
                    }
                    burnTime--;
                }
                else
                {
                    //isThrown = false;
                    falling = false;
                }
            }

            if (!isThrown)
            {
                this.position = player.Location;
            }

            if (burnTime <= 0)
            {
                isThrown = false;
                int num = 0;
                for (int c = 0; c < player.torches.Length; c++)
                {
                    if (player.torches[c].burnTime > 0)
                    {
                        num++;
                    }
                }
                if (num == 0)
                {
                    player.burning2.Stop();
                }
            }
        }

        //Throw the torch
        public void throwTorch()
        {
            isThrown = true;
            falling = true;
            velocity.Y = -1 * ySpeed;
            burnTime = 200;
        }


        //Property for thrown
        public bool IsThrown
        {
            get { return isThrown; }
            set { isThrown = value; }
        }

        public Boolean Falling
        {
            get { return falling; }
            set { falling = value; }
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

        public void subtractX(float newX)
        {
            //position.X -= newX;
            bounds.Offset((int)-newX, 0);
        }
    }
}
