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
    enum DrawState
    {
        Standing,
        Running,
        Jumping,
        Falling,
        Attacking,
        Pain
    }
    enum Facing
    {
        Left,
        Right
    }

    abstract class GameObject
    {
        // Fields

        private Rectangle position;
        private Rectangle hitBox;
        protected Vector2 velocity;
        protected Texture2D texture;
        protected bool isAirborne;
        protected DrawState drawState;
        protected Facing direction;
        protected int hp;
        protected int runSpeed;



        // Properties

        // Position-based properties
        public Rectangle Position { get { return position; } }
        public Rectangle HitBox { get { return hitBox; } }
        public float VelocityX { get { return velocity.X; } set { velocity.X = value; } }
        public int X 
        { 
            get { return position.X; } 
            set 
            {
                hitBox.X = value + (hitBox.X - position.X);
                position.X = value; 
            } 
        }
        public int Y 
        {
            get { return position.Y; } 
            set 
            {
                hitBox.Y = value + (hitBox.Y - position.Y);
                position.Y = value; 
            } 
        }
        public int Width { get { return position.Width; } } 
        public int Height { get { return position.Height; } }

        // Other properties
        public Texture2D Texture { get { return texture; } set { texture = value; } }
        public bool IsAirborne { get { return isAirborne; } set { isAirborne = value; } }
        public DrawState DrawState { get { return drawState; } set { drawState = value; } }
        public Facing Direction { get { return direction; } }
        public int HP { get { return hp; } set { hp = value; } }



        // Constructor

        public GameObject(Rectangle position, Rectangle hitBox, Texture2D texture)
        {
            this.position = position;
            this.hitBox = hitBox;
            this.texture = texture;
        }



        // Methods

        public void Update()
        {
        }

        /// <summary>
        /// Draws a sprite, overridable.
        /// </summary>
        /// <param name="sb"></param>
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, position, Color.White);
        }

        /// <summary>
        /// Alters a GameObject's y position based on physics itself.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="vertAcceleration"></param>
        public virtual void Gravity(GameTime gameTime, int vertAcceleration)
        {
            if (IsAirborne)
            {
                // Velocity changes by its acceleration times seconds squared ( delta v = a * s^2 )
                // The next seconds in "seconds squared" is calculated at the bottom with the horizontal velocity
                velocity.Y += (float)(vertAcceleration * gameTime.ElapsedGameTime.TotalSeconds);

                // Update the y position based on velocity
                Y += (int)Math.Round(velocity.Y * gameTime.ElapsedGameTime.TotalSeconds);
            }
        }

        public virtual void TakeDamage(int dmgValue)
        {
            hp -= dmgValue;
        }
    }
}
