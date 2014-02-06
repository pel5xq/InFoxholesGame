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
        public Texture2D EnemyTexture;
        public AnimatedSprite EnemyTextureMap;
        public Texture2D EnemyDeathTexture;
        public Texture2D FiringTexture;
        double firingAnimationRate;
        double beginFiringTime;
        public Vector2 Position;
        public float speed;
        public bool Alive;
        public bool isFiring;
        public Loot loot;
        public bool isLooted;

        /* Magic Number */
        float firinganimationrate = 300;

        public int Width
        {
            get { return EnemyTexture.Width; }
        }

        public int Height
        {
            get { return EnemyTexture.Height; }
        }

        virtual public void Initialize(ContentManager content, Vector2 position, Loot theLoot)
        {
            Position = position;
            Alive = true;
            isFiring = false;
            firingAnimationRate = firinganimationrate;
            beginFiringTime = 0;
            loot = theLoot;
            isLooted = false;
        }

        public bool isHit(Vector2 crosshairPosition)
        {
            Vector2 truePosition = Vector2.Subtract(crosshairPosition, Position);
            if (Alive &&
                truePosition.X >= 0 &&
                truePosition.Y >= 0 &&
                truePosition.X <= EnemyTexture.Width &&
                truePosition.Y <= EnemyTexture.Height)
            {
                Alive = false;
                return true;
            }
            return false;
        }

        public void Update(GameTime gametime, Scavenger scavenger)
        {

            if (Alive && scavenger.Alive) {
                float scavR = scavenger.Width + scavenger.Position.X;
                float scavL = scavenger.Position.X;
                float myR = Width + Position.X;
                float myL = Position.X;
                if ((scavR >= myL && scavR <= myR) || (scavL >= myL && scavL <= myR))
                {
                    isFiring = true;
                    scavenger.Alive = false;
                    beginFiringTime = gametime.TotalGameTime.TotalMilliseconds;
                }
            }
            else if (isFiring)
            {
                if (gametime.TotalGameTime.TotalMilliseconds - beginFiringTime > firingAnimationRate) isFiring = false;
            }
            //Check if at trench, otherwise move forward
            if (Alive && !isFiring)
            {
                Position = Pather.Move(Position, true, speed);
                EnemyTextureMap.Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Alive)
            {
                if (isFiring) spriteBatch.Draw(FiringTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                else EnemyTextureMap.Draw(spriteBatch, Position, 1f);
            }
            else spriteBatch.Draw(EnemyDeathTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
