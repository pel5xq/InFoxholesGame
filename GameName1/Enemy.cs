using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace GameName1
{
    class Enemy : Targetable
    {
        public bool isHit(Vector2 crosshairPosition)
        {
            Vector2 truePosition = Vector2.Subtract(crosshairPosition, Position);
            if (Alive && 
                truePosition.X >=0 &&
                truePosition.Y >=0 &&
                truePosition.X <= EnemyTexture.Width &&
                truePosition.Y <= EnemyTexture.Height) {
                    Alive = false;
                    return true;
            }
            return false;
        }
        public Texture2D EnemyTexture;
        public AnimatedSprite EnemyTextureMap;
        public Texture2D EnemyDeathTexture;
        public Vector2 Position;
        public Vector2 Movement;
        public bool Alive;

        public int Width
        {
            get { return EnemyTexture.Width; }
        }

        public int Height
        {
            get { return EnemyTexture.Height; }
        }

        virtual public void Initialize(ContentManager content, Vector2 position)
        {
            Position = position;
            Alive = true;
        }

        public void Update()
        {
            //Check if at trench, otherwise move forward
            if (Alive)
            {
                Position = Vector2.Add(Position, Movement);
                EnemyTextureMap.Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //if (Alive) spriteBatch.Draw(EnemyTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            if (Alive) EnemyTextureMap.Draw(spriteBatch, Position, 1f);
            else spriteBatch.Draw(EnemyDeathTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
