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
    class Collectables : GameObject
    {
        private Rectangle position;
        private Rectangle hitBox;
        private Texture2D texture;
        public Collectables(Rectangle position, Rectangle hitBox, Texture2D texture)
            : base(position, hitBox, texture)
        {
            this.position = position;
            this.hitBox = hitBox;
            this.texture = texture;
        }
        public override void Draw(SpriteBatch sb)
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
