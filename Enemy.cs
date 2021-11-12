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
    class Enemy : GameObject
    {
        // Fields

        private Player norman;
        public bool gotHit;



        // Constructor

        public Enemy(Rectangle position, Rectangle hitBox, Texture2D texture, Player norman)
            : base(position, hitBox, texture)
        {
            hp = 40;
            this.norman = norman;
            runSpeed = 5;
            gotHit = false;
        }



        // Methods

        public void MoveLeft(int speed)
        {
            X -= speed;
        }

        public void MoveRight(int speed)
        {
            X += speed;
        }

        public void Update(GameTime gameTime)
        {
            if (IsAirborne)
                //enemies apply gravity
                Gravity(gameTime, 1000);
            else
                // Vertical speed on the ground should be zero
                velocity.Y = 0;
            
            UpdateAI(norman);

            if (!norman.Attack)
                gotHit = false;

            // Update the enemy's x position based on the velocity
            X += (int)Math.Round(velocity.X * gameTime.ElapsedGameTime.TotalSeconds);
        }

        private void UpdateAI(GameObject entity)
        {
            // Get the distance between enemy and entity
            int distance = (X + (Width / 2)) - (entity.X + (entity.Width / 2));

            // Check if entity is within vertical range
            if (Math.Abs(Y - entity.Y) <= 200)
            {
                // Check if entity is within horizontal range
                if (Math.Abs(distance) <= 300)
                {
                    if (distance >= 0)
                    {
                        velocity.X -= runSpeed;
                        direction = Facing.Left;
                    }
                    else
                    {
                        velocity.X += runSpeed;
                        direction = Facing.Right;
                    }
                }
                else
                {
                    // Make sure the enemy slows down when out of range
                    // But not if it's airborne
                    if (!IsAirborne && velocity.X != 0)
                    {
                        if (Math.Abs(velocity.X) < runSpeed)
                            velocity.X = 0;
                        else
                        {
                            int direction = (int)(velocity.X / Math.Abs(velocity.X));
                            velocity.X -= direction * runSpeed;
                        }
                    }
                }
            }
            else
            {
                // Make sure the enemy slows down when out of range
                // But not if it's airborne
                if (!IsAirborne && velocity.X != 0)
                {
                    if (Math.Abs(velocity.X) < runSpeed)
                        velocity.X = 0;
                    else
                    {
                        int direction = (int)(velocity.X / Math.Abs(velocity.X));
                        velocity.X -= direction * runSpeed;
                    }
                }
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            // Determine whether or not the sprites should be flipped
            SpriteEffects flip;
            if (direction == Facing.Left)
                flip = SpriteEffects.FlipHorizontally;
            else
                flip = SpriteEffects.None;

            sb.Draw(
            texture,                                                 // Texture
            new Vector2(X, Y),                                       // Coordinates
            new Rectangle(0, 0, texture.Width, texture.Height),      // Portion
            Color.White,                                             // Color
            0.0f,                                                    // Rotation
            Vector2.Zero,                                            // Origin
            new Vector2(.2f, .2f),                                   // Scale
            flip,                                                    // Effects
            0                                                        // Layer depth
            );
        }
    }
}
