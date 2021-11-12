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
    enum PlayerState
    {
        Damageable,
        Invincible
    }

    /// <summary>
    /// The player class handles the character the user controls.
    /// The methods called contain movement, attacks, and animations.
    /// This class directly reacts to user input.
    /// </summary>
    class Player : GameObject
    {
        // Fields

        private InventoryManager inventory;
        private PlayerState pState;
        private double timePassed = 0;
        private int vertAcceleration = 1500;
        private bool hitStun;
        private Vector2 movement;
        private bool attack;
        private Rectangle attackRect;
        private double attackTimer;
        private bool hitCeiling;
        public double theTimer;



        // Drawing & Animation Fields

        private Color iColor;
        private float scale;
        private int spriteDimension;

        private double fps;
        private double spf;
        private double tickyTimer;
        private int currentFrame;



        // Property

        public bool Attack { get { return attack; } }
        public bool HitStun { get { return hitStun; } set { hitStun = value; } }
        public Vector2 Movement { get { return movement; } set { movement = value; } }
        public float MoveX { get { return movement.X; } set { movement.X = value; } }
        public float MoveY { get { return movement.Y; } set { movement.Y = value; } }
        public Rectangle AttackRect { get { return attackRect; } }
        public bool HitCeiling { get { return hitCeiling; } set { hitCeiling = value; } }



        // Constructor

        public Player(Rectangle position, Rectangle hitBox, Texture2D texture, InventoryManager inventory)
            : base(position, hitBox, texture)
        {
            this.inventory = inventory;
            this.texture = texture;
            hp = 100;
            runSpeed = 250;
            hitStun = false;
            attack = false;
            attackTimer = 0;
            iColor = Color.DarkGray;
            isAirborne = false;
            drawState = DrawState.Standing;
            direction = Facing.Right;
            spriteDimension = texture.Width / 4;
            scale = Game1.tileSize / (texture.Width / 4f);
            hitCeiling = false;


            fps = 6;
            spf = 1 / fps;
            tickyTimer = 0;
            currentFrame = 1;
        }



        // Methods

        public override void Gravity(GameTime gameTime, int vertAcceleration)
        {
            if (IsAirborne)
            {
                // Velocity changes by its acceleration times seconds squared ( delta v = a * s^2 )
                // The next seconds in "seconds squared" is calculated at the bottom with the horizontal velocity
                velocity.Y += (float)(vertAcceleration * gameTime.ElapsedGameTime.TotalSeconds);

                // Update the y position based on velocity
                if (!hitStun)
                {
                    movement.Y = (int)Math.Round(velocity.Y * gameTime.ElapsedGameTime.TotalSeconds) + 1;
                }
                else
                {
                    movement.Y = (int)Math.Round(velocity.Y * gameTime.ElapsedGameTime.TotalSeconds) + 1;
                }
            }
        }

        public override void TakeDamage(int dmgValue)
        {
            if (pState == PlayerState.Damageable)
            {
                base.TakeDamage(dmgValue);
                hitStun = true;
                isAirborne = true;
            }
        }

        public void Respawn()
        {
            HP = 100;
            hitStun = false;
            pState = PlayerState.Damageable;
            direction = Facing.Right;
        }

        /// <summary>
        /// Takes care of all the user input and how it affects player movement and actions.
        /// </summary>
        /// <param name="previousKbState"></param>
        /// <param name="kbState"></param>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime, KeyboardState kbState, KeyboardState prevKbState)
        {
            movement = Vector2.Zero;

            // Declare variable for the change in horizontal velocity per frame
            int horizVelocity;

            switch (pState)
            {
                case PlayerState.Damageable:

                    if (hitStun == true)
                    {
                        pState = PlayerState.Invincible;
                        // Always bump the player against current direction
                        if (direction == Facing.Right)
                            velocity.X = (int)(-runSpeed);
                        else
                            velocity.X = (int)(runSpeed);
                        // Start with an initial vertical velocity (px per sec)
                        velocity.Y = -600;
                        isAirborne = true;
                        break;
                    }
                    break;

                case PlayerState.Invincible:

                    if (!hitStun)
                    {
                        timePassed += gameTime.ElapsedGameTime.TotalSeconds;
                        if (timePassed >= 1)
                        {
                            pState = PlayerState.Damageable;
                            timePassed = 0;
                        }
                    }
                    else
                    {
                        if (!IsAirborne)
                        {
                            pState = PlayerState.Invincible;
                            hitStun = false;
                            timePassed = 0;
                        }
                    }
                    break;
            }

            if (!hitStun)
            {
                // Set horizontal velocity to zero, this will change if directional keys are pressed
                velocity.X = 0;

                if (isAirborne)
                {
                    // Horizontal speed in air (px per sec)
                    horizVelocity = (int)(runSpeed * .6);
                }
                else
                {
                    // Horizontal speed on ground (px per sec)
                    horizVelocity = runSpeed;

                    // Start a jump if the Space key is pressed
                    if (kbState.IsKeyDown(Keys.Space))
                    {
                        // Start with an initial vertical velocity (px per sec)
                        velocity.Y = -700;
                        isAirborne = true;
                    }
                }

                // Move player horizontally based on directional key input
                if (kbState.IsKeyDown(Keys.A))
                {
                    // Horizontal velocity has negative magnitude
                    velocity.X = -horizVelocity;
                    direction = Facing.Left;
                }
                else if (kbState.IsKeyDown(Keys.D))
                {
                    // Horizontal velocity has positive magnitude
                    velocity.X = horizVelocity;
                    direction = Facing.Right;
                }
            }


            // General logic check regardless of pState
            if (IsAirborne)
            {
                //applies gravity
                Gravity(gameTime, vertAcceleration);
                //checks the bottom of the platfprms and knocks you back from the bottom
                if (hitCeiling == true)
                {
                    velocity.Y = Math.Abs(velocity.Y) / 3;
                    hitCeiling = false;
                }
                //timer so he can actually be hitstuned
                if (hitStun == true)
                {
                    drawState = DrawState.Pain;
                    theTimer += .01;
                }
                else
                {
                    // Update the drawState
                    if (velocity.Y <= 0)
                        drawState = DrawState.Jumping;
                    else
                        drawState = DrawState.Falling;
                }
                
            }
            else
            {
                // Reset hitCeiling
                hitCeiling = false;

                // Vertical speed on the ground should be zero
                velocity.Y = 0;

                // Update the drawState
                if (velocity.X == 0)
                    drawState = DrawState.Standing;
                else
                    drawState = DrawState.Running;
            }

            // Open up the inventory
            if (kbState.IsKeyDown(Keys.F) && !prevKbState.IsKeyDown(Keys.F))
            {
                //need to have a way of finding out the name and communicating that with the inventory manager
                //inventory.UseItem();
            }

            // Add to attackTimer
            attackTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (attack == true)
            {
                if (attackTimer > .3)
                    attack = false;
                else
                {
                    drawState = DrawState.Attacking;

                    if (direction == Facing.Right)
                        attackRect = new Rectangle(X + (Game1.tileSize / 2), Y, Game1.tileSize, Game1.tileSize);
                    else
                        attackRect = new Rectangle(X - (Game1.tileSize / 2), Y, Game1.tileSize, Game1.tileSize);
                }
            }
            else
            {
                if (kbState.IsKeyDown(Keys.E) && attackTimer > .6)
                {
                    attack = true;
                    attackTimer = 0;
                    drawState = DrawState.Attacking;
                }
            }
            
            // Update the x position based on the velocity
            movement.X = (int)Math.Round(velocity.X * gameTime.ElapsedGameTime.TotalSeconds);

            

            if (drawState == DrawState.Running || drawState == DrawState.Attacking)
                UpdateAnimations(gameTime);
            else
                currentFrame = 1;

           
        }

        private void UpdateAnimations(GameTime gameTime)
        {
            // Add elapsed seconds to the timer
            tickyTimer += gameTime.ElapsedGameTime.TotalSeconds;

            // If enough seconds have gone by, move on to the next frame
            if (tickyTimer >= spf)
            {
                currentFrame++;
                tickyTimer -= spf;
            }

        }

        /// <summary>
        /// Draws the player sprite based on what the DrawState is, and what direction they're facing.
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            // Alter color based on pState
            Color color;
            if (pState == PlayerState.Damageable)
                color = Color.White;
            else
            {
                if (iColor == Color.DarkRed)
                    iColor = Color.White;
                else
                    iColor = Color.DarkRed;

                color = iColor;
            }

            // Determine whether or not the sprites should be flipped
            SpriteEffects flip;
            if (direction == Facing.Left)
                flip = SpriteEffects.FlipHorizontally;
            else
                flip = SpriteEffects.None;

            // Switch statement to determine which part of the spritesheet to draw
            switch (drawState)
            {
                case (DrawState.Standing):

                    sb.Draw(
                        texture,                       // Texture
                        new Vector2(X, Y),             // Coordinates
                        new Rectangle(                 // Portion
                            0, 
                            0, 
                            spriteDimension, 
                            spriteDimension
                            ),
                        color,                         // Color
                        0.0f,                          // Rotation
                        Vector2.Zero,                  // Origin
                        new Vector2(scale, scale),     // Scale
                        flip,                          // Effects
                        0                              // Layer depth
                        );

                    break;


                case (DrawState.Running):

                    if (currentFrame == 1)
                        sb.Draw(
                            texture,                       // Texture
                            new Vector2(X, Y),             // Coordinates
                            new Rectangle(                 // Portion
                                spriteDimension * 3,
                                0,
                                spriteDimension,
                                spriteDimension
                                ),
                            color,                         // Color
                            0.0f,                          // Rotation
                            Vector2.Zero,                  // Origin
                            new Vector2(scale, scale),     // Scale
                            flip,                          // Effects
                            0                              // Layer depth
                            );

                    else if (currentFrame == 2 || currentFrame >= 4)
                    {
                        sb.Draw(
                            texture,                       // Texture
                            new Vector2(X, Y),             // Coordinates
                            new Rectangle(                 // Portion
                                spriteDimension * 3,
                                spriteDimension,
                                spriteDimension,
                                spriteDimension
                                ),
                            color,                         // Color
                            0.0f,                          // Rotation
                            Vector2.Zero,                  // Origin
                            new Vector2(scale, scale),     // Scale
                            flip,                          // Effects
                            0                              // Layer depth
                            );

                        if (currentFrame > 4)
                            currentFrame = 1;
                    }

                    else if (currentFrame == 3)
                        sb.Draw(
                            texture,                       // Texture
                            new Vector2(X, Y),             // Coordinates
                            new Rectangle(                 // Portion
                                spriteDimension * 3,
                                spriteDimension * 2,
                                spriteDimension,
                                spriteDimension
                                ),
                            color,                         // Color
                            0.0f,                          // Rotation
                            Vector2.Zero,                  // Origin
                            new Vector2(scale, scale),     // Scale
                            flip,                          // Effects
                            0                              // Layer depth
                            );

                    break;


                case (DrawState.Jumping):

                    sb.Draw(
                        texture,                       // Texture
                        new Vector2(X, Y),             // Coordinates
                        new Rectangle(                 // Portion
                            spriteDimension, 
                            0, 
                            spriteDimension, 
                            spriteDimension
                            ),
                        color,                         // Color
                        0.0f,                          // Rotation
                        Vector2.Zero,                  // Origin
                        new Vector2(scale, scale),     // Scale
                        flip,                          // Effects
                        0                              // Layer depth
                        );

                    break;


                case (DrawState.Falling):

                    sb.Draw(
                        texture,                       // Texture
                        new Vector2(X, Y),             // Coordinates
                        new Rectangle(                 // Portion
                            spriteDimension * 2, 
                            0, 
                            spriteDimension, 
                            spriteDimension
                            ),
                        color,                         // Color
                        0.0f,                          // Rotation
                        Vector2.Zero,                  // Origin
                        new Vector2(scale, scale),     // Scale
                        flip,                          // Effects
                        0                              // Layer depth
                        );

                    break;


                case (DrawState.Attacking):

                    int xValue;
                    if (direction == Facing.Left)
                        xValue = X - (Game1.tileSize / 2);
                    else
                        xValue = X;

                    if (currentFrame == 1)
                        sb.Draw(
                            texture,                       // Texture
                            new Vector2(xValue, Y),        // Coordinates
                            new Rectangle(                 // Portion
                                0,
                                spriteDimension,
                                (int)(spriteDimension * 1.5),
                                spriteDimension
                                ),
                            color,                         // Color
                            0.0f,                          // Rotation
                            Vector2.Zero,                  // Origin
                            new Vector2(scale, scale),     // Scale
                            flip,                          // Effects
                            0                              // Layer depth
                            );

                    else if (currentFrame == 2)
                        sb.Draw(
                            texture,                       // Texture
                            new Vector2(xValue, Y),        // Coordinates
                            new Rectangle(                 // Portion
                                (int)(spriteDimension * 1.5),
                                spriteDimension,
                                (int)(spriteDimension * 1.5),
                                spriteDimension
                                ),
                            color,                         // Color
                            0.0f,                          // Rotation
                            Vector2.Zero,                  // Origin
                            new Vector2(scale, scale),     // Scale
                            flip,                          // Effects
                            0                              // Layer depth
                            );

                    else if (currentFrame >= 3)
                    {
                        sb.Draw(
                            texture,                       // Texture
                            new Vector2(xValue, Y),        // Coordinates
                            new Rectangle(                 // Portion
                                0,
                                spriteDimension * 2,
                                (int)(spriteDimension * 1.5),
                                spriteDimension
                                ),
                            color,                         // Color
                            0.0f,                          // Rotation
                            Vector2.Zero,                  // Origin
                            new Vector2(scale, scale),     // Scale
                            flip,                          // Effects
                            0                              // Layer depth
                            );

                        if (currentFrame > 3)
                            currentFrame = 1;
                    }

                    break;


                case (DrawState.Pain):

                    sb.Draw(
                        texture,                              // Texture
                        new Vector2(X, Y),                    // Coordinates
                        new Rectangle(                        // Portion
                            (int)(spriteDimension * 1.5),
                            spriteDimension * 2,  
                            spriteDimension, 
                            spriteDimension
                            ),
                        color,                                // Color
                        0.0f,                                 // Rotation
                        Vector2.Zero,                         // Origin
                        new Vector2(scale, scale),            // Scale
                        flip,                                 // Effects
                        0                                     // Layer depth
                        );

                    break;
            }
        }

    }
}
