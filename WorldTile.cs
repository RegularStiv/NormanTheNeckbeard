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
    class WorldTile
    {
        // Fields

        protected Texture2D texture;
        protected Rectangle position;
        protected Vector2 vectPosition;



        // Properties

        public Texture2D Texture { get { return texture; } }
        public Rectangle Position { get { return position; } }

        public float X 
        { 
            get { return vectPosition.X; } 
            set 
            { 
                vectPosition.X = value;
                position.X = (int)vectPosition.X;
            } 
        }
        public float Y 
        { 
            get { return vectPosition.Y; } 
            set 
            { 
                vectPosition.Y = value;
                position.Y = (int)vectPosition.Y;
            } 
        }


        // Constructor

        public WorldTile(Texture2D texture, Rectangle position)
        {
            this.texture = texture;
            this.position = position;
            vectPosition = Vector2.Zero;
            vectPosition.X = position.X;
            vectPosition.Y = position.Y;
        }



        // Method

        /// <summary>
        /// Draws a sprite.
        /// </summary>
        /// <param name="sb"></param>
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(
            texture,                                                 // Texture
            new Vector2(X, Y),                                       // Coordinates
            new Rectangle(0, 0, texture.Width, texture.Height),      // Portion
            Color.White,                                             // Color
            0.0f,                                                    // Rotation
            Vector2.Zero,                                            // Origin
            new Vector2(.2f, .2f),                                   // Scale
            SpriteEffects.None,                                      // Effects
            0                                                        // Layer depth
            );
        }

    }
}
