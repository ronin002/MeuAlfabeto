
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace meuAlfabeto
{
   public class GenericStaticObj
    {
        public Vector2 Position;
        public Texture2D Texture;
        public Rectangle SourceRect;
        public float Scale;

        public GenericStaticObj(Texture2D texture, Vector2 position, Rectangle sourceRect, float scale = 1.0f)
        {
            Texture = texture;
            Position = position;
            SourceRect = sourceRect;
            Scale = scale;
        }

        public GenericStaticObj(Vector2 position, Rectangle sourceRect, float scale = 1.0f)
        {
            Position = position;
            SourceRect = sourceRect;
            Scale = scale;
        }


        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, SourceRect, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
        }
    }
}