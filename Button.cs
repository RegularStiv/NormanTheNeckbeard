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
    class Button
    {
        // Fields:
        private Texture2D texture;
        private Rectangle hitBox;
        private float scale;
        private Vector2 position;

        // Properties:
        public Rectangle HitBox
        {
            get
            {
                return hitBox;
            }
        }

        // Constructor:
        public Button(Texture2D texture, Rectangle hitBox, float scale)
        {
            this.texture = texture;
            this.hitBox = hitBox;
            this.scale = scale;
            position = new Vector2(this.hitBox.X, this.hitBox.Y);
        }

        // Methods:
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(
                           texture,                                                                             // Texture
                           position,                                                                            // Coordinates
                           new Rectangle(0,0, texture.Width, texture.Height),                                   // Portion
                           Color.White,                                                                         // Color
                           0.0f,                                                                                // Rotation
                           Vector2.Zero,                                                                        // Origin
                           new Vector2(scale, scale),                                                           // Scale
                           SpriteEffects.None,                                                                  // Effects
                           0                                                                                    // Layer depth
                           );
        }
    }
}
