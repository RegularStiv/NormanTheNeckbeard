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
    class Platform : WorldTile
    {
        // Constructor

        public Platform(Texture2D texture, Rectangle position) : base(texture, position)
        {
        }

        /// <summary>
        /// Draws a sprite.
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(
            texture,                                                            // Texture
            new Vector2(position.X, position.Y),                                // Coordinates
            new Rectangle(0, 0, (texture.Width / 2), (texture.Height / 2)),     // Portion
            Color.White,                                                        // Color
            0.0f,                                                               // Rotation
            Vector2.Zero,                                                       // Origin
            new Vector2(.2f, .2f),                                              // Scale
            SpriteEffects.None,                                                 // Effects
            0                                                                   // Layer depth
            );
        }

        /// <summary>
        /// Draws a sprite based on number input.
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb, int part)
        {
            Rectangle piece = new Rectangle(0, 0, 0, 0);

            if (part <= 0)
                piece = new Rectangle(0, 0, (texture.Width / 2), (texture.Height / 2));
            else if (part == 1)
                piece = new Rectangle((texture.Width / 2), 0, (texture.Width / 2), (texture.Height / 2));
            else if (part == 2)
                piece = new Rectangle(0, (texture.Height / 2), (texture.Width / 2), (texture.Height / 2));
            else if (part == 3)
                piece = new Rectangle((texture.Width / 2), (texture.Height / 2), (texture.Width / 2), (texture.Height / 2));
            else
                for (int i = 0; i < 4; i++)
                    Draw(sb, i);

            sb.Draw(
            texture,                                                            // Texture
            new Vector2(position.X, position.Y),                                // Coordinates
            piece,                                                              // Portion
            Color.White,                                                        // Color
            0.0f,                                                               // Rotation
            Vector2.Zero,                                                       // Origin
            new Vector2(.2f, .2f),                                              // Scale
            SpriteEffects.None,                                                 // Effects
            0                                                                   // Layer depth
            );
        }

    }
}
