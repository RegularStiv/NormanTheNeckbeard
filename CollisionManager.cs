using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace NormanTheNeckbeard
{
    class CollisionManager
    {
        // Fields

        private Player norman;
        private WorldManager worldManager;
        private int platformCollisions;



        // Properties





        // Constructor

        public CollisionManager(Player norman, WorldManager worldManager)
        {
            this.norman = norman;
            this.worldManager = worldManager;
        }



        // Methods
       
        public void Update()
        {
            Collides(norman);
            foreach (Enemy enemy in worldManager.Enemies)
                Collides(enemy);
        }

        public void Collides(GameObject entity)
        {
            // Set platformCollisions to be zero
            platformCollisions = 0;

            foreach (Platform platform in worldManager.PlatformList)
            {
                // This is the potentially overlapping platform
                Rectangle checktangle = platform.Position;

                // If the platform overlaps with the gameObject
                if (entity.HitBox.Intersects(checktangle))
                {
                    // Add to platformCollisions count
                    platformCollisions++;

                    // Constructing the rectangle that represents the overlapping area between norman and the platform
                    Rectangle overlap = new Rectangle();

                    // calculates the vertical components of the overlapping rectangle
                    // dont know why but the vibing stopped but it works.
                    if (entity.HitBox.Y < checktangle.Y)
                    {
                        overlap.Y = checktangle.Y;
                        overlap.Height = entity.HitBox.Y + entity.HitBox.Height - checktangle.Y;
                    }
                    else
                    {
                        overlap.Y = entity.HitBox.Y;
                        overlap.Height = checktangle.Y + checktangle.Height - entity.HitBox.Y;
                    }

                    // calculates the horizontal components of the overlapping rectangle
                    if (entity.HitBox.X < checktangle.X)
                    {
                        overlap.X = checktangle.X;
                        overlap.Width = entity.HitBox.X + entity.HitBox.Width - checktangle.X;
                    }
                    else
                    {
                        overlap.X = entity.HitBox.X;
                        overlap.Width = checktangle.X + checktangle.Width - entity.HitBox.X;
                    }


                    // now we adjust norman's position based on the rectangle
                    // check if norman's closer to the sides than the top or bottom
                    if (overlap.Width < overlap.Height)
                    {
                        // if he's closer to the left
                        if (overlap.X <= checktangle.X)
                        {
                            if (entity == norman)
                            {
                                norman.MoveX -= overlap.Width - 1;
                            }
                            else
                            {
                                entity.X -= overlap.Width - 1;
                                entity.VelocityX = -(entity.VelocityX / 3);
                            }
                        }
                        // otherwise, he's closer to the right
                        else
                        {
                            if (entity == norman)
                                norman.MoveX += overlap.Width;
                            else
                            {
                                entity.X += overlap.Width;
                                entity.VelocityX = -entity.VelocityX / 3;
                            }
                        }
                    }
                    // otherwise, he's closer to the top or bottom
                    else
                    {
                        // if he's closer to the top
                        if (overlap.Y == checktangle.Y)
                        {
                            if (entity == norman)
                            {
                                norman.MoveY -= (overlap.Height - 2);
                            }
                            else
                                entity.Y -= overlap.Height - 1;

                            // also this means he's on the ground so:
                            entity.IsAirborne = false;
                        }
                        // otherwise, he's closer to the bottom
                        else
                        {
                            if (entity == norman)
                            {
                                norman.MoveY += overlap.Height;
                                norman.HitCeiling = true;
                            }
                            else
                                entity.Y += overlap.Height;
                        }
                    }
                }
            }

            
            //if it is norman it checks everytile so if he is in contact with the ground then he doesnt get knocked up twice
            if (entity == norman)
            {
                Rectangle hitBoxCheck = new Rectangle(norman.HitBox.X + 8, norman.HitBox.Y + norman.HitBox.Height + 1, norman.HitBox.Width - 16, 1);
                foreach (Platform platform in worldManager.PlatformList)
                {
                    //checks to see if he isn't being knocked up by enemies and determiens if he is in contact with the ground
                    if (platform.Position.Intersects(hitBoxCheck) && (norman.theTimer >= .1 || norman.DrawState != DrawState.Pain))
                    {
                        norman.theTimer = 0;
                        entity.IsAirborne = false;
                        return;
                    }
                }
                entity.IsAirborne = true;
            }
            else if (platformCollisions == 0)
            {
                entity.IsAirborne = true;
            }
        }

    }
}
