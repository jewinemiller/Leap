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
    //Partial World Class. This acts as the manager for the platforms and anything else that shows up on screen
    partial class World
    {
        //The platforms that the world controls
        private List<Platform> platforms = new List<Platform>();
        //Graphics Device Manager
        private GraphicsDeviceManager graphics;

        public void checkForTorches(Player player, Powerup power)
        {
            foreach (Platform p in platforms)
            {
                p.isPlayerCollecting(power, player);
            }
        }

        //Add a platform to the list
        public void addPlatform(Rectangle bounds, Texture2D texture, Texture2D[] t, Random r)
        {
            platforms.Add(new Platform(bounds, texture, t, r));
        }

        public void checkFallingPlatforms(int dist)
        {
            for (int i = 0; i < platforms.Count(); i++)//Platform p in platforms)
            {
                if (platforms[i] is FallingPlatform)
                {
                    if ((platforms[i] as FallingPlatform).checkPlayerCollision(this.player))
                    {
                        player.currentPlatform = platforms[i];

                        if ((platforms[i] as FallingPlatform).isAboveGround())
                        {
                            (platforms[i] as FallingPlatform).dropPlatform(dist);
                            player.Location = new Vector2(player.Location.X, player.Location.Y + dist);
                        }
                        else
                        {
                            regenPlatform(platforms[i]);
                        }
                    }
                }
            }
        }

        //Move a platform by x Distance
        public void movePlatforms(int distX)
        {
            this.distTraveled += distX;
            this.score += ((float)distX / (float)sizeFactor); 
            if (distTraveled >= CHECKPOINT_DISTANCE)
            {
                sizeFactor = 3.5;
                distTraveled = 0;
            }
            //Move torches
            foreach (Torch t in player.torches)
            {
                t.subtractX(distX);
            }

            //Loop through the platforms

                for (int i = 0; i < platforms.Count(); i++)
                {
                    //Move each platform to the left by disX
                    platforms[i].Bounds = new Rectangle(platforms[i].Bounds.X - distX, platforms[i].Bounds.Y, platforms[i].Bounds.Width, platforms[i].Bounds.Height);
                    //If the platform is off the screen, generate a new one. 
                    if (platforms[i].Bounds.X + platforms[i].Bounds.Width <= -30)
                    {
                        regenPlatform(platforms[i]);
                    }

                   /* if (p is FallingPlatform)
                    {
                        if ((p as FallingPlatform).checkPlayerCollision(this.player))
                        {
                            if ((p as FallingPlatform).isAboveGround())
                            {
                                (p as FallingPlatform).dropPlatform(1);
                            }
                            else
                            {
                                regenPlatform(p);
                                break;
                            }
                        }
                    }*/
                }
               /* if (sizeFactor > 1.5)
                {
                    if (sizeFactor > 3)
                    {
                        if (sizeFactor > 6)
                        {
                            if (sizeFactor < 9)
                            {
                                sizeFactor -= .03;
                            }
                            else
                            {
                                sizeFactor -= .01;
                            }
                        }
                        else
                        {
                            sizeFactor -= .0025;
                        }
                    }
                    else
                    {
                        sizeFactor -= .0015;
                    }
                }*/        
        }

        //Function that generates a new platform
        private void regenPlatform(Platform plat)
        {
            int index = platforms.IndexOf(plat);
            int prevIndex = index - 1;
            if (prevIndex == -1)
            {
                prevIndex = platforms.Count - 1;
            }

            Platform p = platforms[index];
            //Remove the platform that is off the screen
            //platforms.Remove(p);
            //Get the platform from the end of the list
            Platform temp = platforms[prevIndex];
            //New random number generator
            Random rand = new Random();
            //This is the amount of Y distance that you will add between the platform currently being generated
            //and the one before it. 
            int yDist = rand.Next(-200, 100 + p.Bounds.Height);
            //The x Location of the new platform
            //This should generate a platform 100 - 200 pixels behind the rightmost edge of the previous platform
            int xVal = temp.Bounds.X + temp.Bounds.Width + rand.Next(200, 300) + this.distTraveled/100;
            //Where the platform will be vertically.
            int yVal = temp.Bounds.Y + yDist;

            //If the platform will be off the screen, move it to the bottom of the screen.
            if (yVal >= graphics.PreferredBackBufferHeight - temp.Bounds.Height || yVal <= 0 + player.Body.Height)
            {
                yVal = graphics.PreferredBackBufferHeight - temp.Bounds.Height - 15;
            }

            //Set the bounds of the new platform
            p.Bounds = new Rectangle(xVal, yVal, rand.Next(150, 350), 25);

            int isFalling = rand.Next(35);
            if (isFalling == 1 && p.Bounds.Y < graphics.PreferredBackBufferHeight - 150)
            {
                p = new FallingPlatform(p.Bounds, p.Texture, p.Textures, p.rand);
            }
            else
            {
                p = new Platform(p.Bounds, p.Texture, p.Textures, p.rand);
            }

            int hasTorch = rand.Next(20);
            //int hasTorch = 1;
            p.HasTorch = false;
            if (hasTorch == 1)
            {
                p.HasTorch = true;
            }
            //Add the new platform to the screen
            //platforms.Add(p);
            platforms[index] = p;
        }

        //Function that generates a new platform
        /*private void regenPlatform(Platform p)
        {
            //Remove the platform that is off the screen
            platforms.Remove(p);
            //Get the platform from the end of the list
            Platform temp = platforms[platforms.Count - 1];
            //New random number generator
            Random rand = new Random();
            //This is the amount of Y distance that you will add between the platform currently being generated
            //and the one before it. 
            int yDist = rand.Next(-200, 100 + p.Bounds.Height);
            //The x Location of the new platform
            //This should generate a platform 100 - 200 pixels behind the rightmost edge of the previous platform
            int xVal = temp.Bounds.X + temp.Bounds.Width + rand.Next(100, 200); 
            //Where the platform will be vertically.
            int yVal = temp.Bounds.Y + yDist;

            //If the platform will be off the screen, move it to the bottom of the screen.
            if (yVal >= graphics.PreferredBackBufferHeight - temp.Bounds.Height || yVal <= 0 + player.Body.Height)
            {
                yVal = graphics.PreferredBackBufferHeight - temp.Bounds.Height - 15;
            }
          
            //Set the bounds of the new platform
            p.Bounds = new Rectangle(xVal, yVal, rand.Next(150, 350), 25);

            int isFalling = rand.Next(35);
            if (isFalling == 1)
            {
                p = new FallingPlatform(p.Bounds, p.Texture, p.Textures, p.rand);
            }
            else
            {
                p = new Platform(p.Bounds, p.Texture, p.Textures, p.rand);
            }

            int hasTorch = rand.Next(20);
            //int hasTorch = 1;
            p.HasTorch = false;
            if (hasTorch == 1)
            {
                p.HasTorch = true;
            }
            //Add the new platform to the screen
            platforms.Add(p);
        }*/

        //Return the platforms
        public List<Platform> getPlatforms()
        {
            return platforms;
        }
    }

    class Platform
    {
        //Bounds and Texture of the Platform
        private Rectangle bounds;
        private Rectangle torchBounds;
        private Texture2D texture;
        private bool hasTorch = false;
        private Texture2D[] textures;
        private Texture2D[] piecesToDraw;
        public Random rand;

        //Public Properties representing the Texture and Bounds of the Platform
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public Texture2D[] Textures
        {
            get { return textures; }
            set { textures = value; }
        }

        public Rectangle Bounds
        {
            get{return bounds;}
            set { bounds = value; }
        }

        public bool HasTorch
        {
            get { return hasTorch; }
            set { hasTorch = value; }
        }

        //Constructor
        public Platform(Rectangle rect, Texture2D tex, Texture2D[] t, Random rand)
        {
            bounds = rect;
            texture = tex;

            textures = t;

            torchBounds = new Rectangle(bounds.X + (bounds.Width / 2) - 10, bounds.Y - 50, 20, 20);

            this.rand = rand;
            setTextures(rand);
        }
        //Empty Constructor.
        public Platform()
        {
            bounds = new Rectangle(0, 0, 10, 10);
            torchBounds = new Rectangle(bounds.X + (bounds.Width / 2) - 10, bounds.Y - 50, 20, 20);
        }

        //Picks random textures for however many will fit on the platform
        public void setTextures(Random rand)
        {
            int numTextures = (bounds.Width) / 15;
           // Random rand = new Random();

            piecesToDraw = new Texture2D[numTextures];

            for (int i = 0; i < numTextures; i++)
            {
                piecesToDraw[i] = textures[rand.Next(2, textures.Length - 3)];
            }
        }

        public void display(SpriteBatch s, Powerup torch, Rectangle drawBounds)
        {
            int xDist = 26;

            //Draw left cap
            Rectangle leftBound = new Rectangle(bounds.X, bounds.Y, 26, 25);
           
            if (drawBounds.Intersects(leftBound))
            {
                Rectangle drawnLeft = Rectangle.Intersect(drawBounds, leftBound);
                s.Draw(textures[0], drawnLeft,  new Rectangle(0,0,textures[0].Width, drawnLeft.Height), Color.White);
            }

            //Draw middle textures
            for (int i = 0; i < piecesToDraw.Length - 1; i++)
            {
                Rectangle temp = new Rectangle(bounds.X + xDist, bounds.Y, 15, 25);
              
                if (drawBounds.Intersects(temp))
                {
                    Rectangle drawnMid = Rectangle.Intersect(drawBounds, temp);
                    s.Draw(piecesToDraw[i], drawnMid, new Rectangle(0,0,piecesToDraw[i].Width, drawnMid.Height), Color.White);
                   
                }
                xDist += 15;
            }

            //Draw end cap
            Rectangle endRect = new Rectangle(bounds.X + xDist, bounds.Y, 26, 25);
            if (drawBounds.Intersects(endRect))
            {
                Rectangle drawnEnd = Rectangle.Intersect(drawBounds, endRect);
              // Rectangle temp = Rectangle.Intersect(drawBounds, endRect));
                s.Draw(textures[1], drawnEnd, new Rectangle(0,0,textures[1].Width, drawnEnd.Height), Color.White);
            }
            xDist -= 15;

            //s.Draw(texture, bounds, Color.Black);

            if (hasTorch == true)
            {
                torch.relocate(this);
                if (drawBounds.Intersects(torch.Bounds))
                {
                    torch.display(s);
                }
            }
        }

        public void isPlayerCollecting(Powerup powerup, Player player)
        {
            if (this.hasTorch == true)
            {
                torchBounds = new Rectangle(bounds.X + (bounds.Width / 2) - 10, bounds.Y - 50, 20, 20);
                if (player.Body.Intersects(torchBounds))
                {
                    this.hasTorch = false;
                    powerup.pickup();
                }
            }
        }
    }
}
