using InFoxholes.Friendlies;
using InFoxholes.Layouts;
using InFoxholes.Looting;
using InFoxholes.Targeting;
using InFoxholes.Util;
using InFoxholes.Waves;
using InFoxholes.Weapons;
using InFoxholes.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace InFoxholes.Enemies
{
    public class Enemy : Targetable
    {
        public Texture2D EnemyTexture;
        public AnimatedSprite EnemyTextureMap;
        public Texture2D EnemyDeathTexture;
        public Texture2D FiringTexture;
        public double firingAnimationRate;
        public double beginFiringTime;
        public Vector2 Position;
        public float speed;
        public bool Alive;
        public bool isFiring;
        public Loot loot;
        public bool isLooted;
        public Vector2 toShoot;
        public Wave wave;

        /* Magic Number */
        public float firinganimationrate = 300;
        public Vector2 adjustShot = new Vector2(25, 7);
        public float lineThickness = 2f;
        Vector2 gunpointOffset = new Vector2(5, 30);

        public int Width
        {
            get { return EnemyTexture.Width; }
        }

        public int Height
        {
            get { return EnemyTexture.Height; }
        }

        virtual public void Initialize(ContentManager content, Vector2 position, Loot theLoot, Wave theWave)
        {
            Position = position;
            Alive = true;
            isFiring = false;
            firingAnimationRate = firinganimationrate;
            beginFiringTime = 0;
            loot = theLoot;
            isLooted = false;
            toShoot = Vector2.Zero;
            wave = theWave;
        }

        virtual public bool isHit(Vector2 crosshairPosition)
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

        virtual public void Update(GameTime gametime, ScavengerManager scavengerManager)
        {

            if (Alive && scavengerManager.getActiveScavenger().Alive)
            {
                float scavR = scavengerManager.getActiveScavenger().Width 
                    + scavengerManager.getActiveScavenger().Position.X;
                float scavL = scavengerManager.getActiveScavenger().Position.X;
                float myR = Width + Position.X;
                float myL = Position.X;
                if ((scavR >= myL && scavR <= myR) || (scavL >= myL && scavL <= myR))
                {
                    isFiring = true;
                    scavengerManager.getActiveScavenger().Alive = false;
                    beginFiringTime = gametime.TotalGameTime.TotalMilliseconds;
                }
            }
            if (isFiring)
            {
                if (gametime.TotalGameTime.TotalMilliseconds - beginFiringTime > firingAnimationRate) isFiring = false;
            }

            if (Alive && !isFiring)
            {
                //Check if at trench
                if (wave.layout.pather.atTrenchEntrance(Position, Width, Height))
                {
                    //If the scavenger is there, kill both
                    if (scavengerManager.getActiveScavenger().Alive && scavengerManager.getActiveScavenger().action == 0)
                    {
                        scavengerManager.getActiveScavenger().Alive = false;
                        beginFiringTime = gametime.TotalGameTime.TotalMilliseconds;
                        Alive = false;
                        toShoot = new Vector2(scavengerManager.getActiveScavenger().Position.X +
                            scavengerManager.getActiveScavenger().Width / 2, scavengerManager.getActiveScavenger().Position.Y
                            + scavengerManager.getActiveScavenger().Height / 4);
                    }
                    else //Otherwise, game over
                    {
                        MainGame.gameOver = true;
                    }
                }
                else
                {
                    Position = wave.layout.pather.Move(Position, true, speed);
                    EnemyTextureMap.Update();
                }
            }
        }

        virtual public void Draw(SpriteBatch spriteBatch)
        {
            if (Alive)
            {
                SpriteEffects effect;
                if (wave.layout.pather.isForward(Position, true, speed))
                {
                    effect = SpriteEffects.FlipHorizontally;
                }
                else
                {
                    effect = SpriteEffects.None;
                }
                if (isFiring)
                {
                    spriteBatch.Draw(FiringTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
                }
                else
                {
                    EnemyTextureMap.Draw(spriteBatch, Position, 1f, effect);
                }
            }
            else
            {
                spriteBatch.Draw(EnemyDeathTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                if (!toShoot.Equals(Vector2.Zero))
                {
                    Vector2 gunPoint = Vector2.Add(Position, gunpointOffset);
                    float distance = Vector2.Distance(toShoot, gunPoint);
                    float angle = (float)Math.Atan2(gunPoint.Y - toShoot.Y, gunPoint.X - toShoot.X);
                    spriteBatch.Draw(Weapon.pixel, toShoot, null, Color.Black, angle, Vector2.Zero, new Vector2(distance, lineThickness),
                                 SpriteEffects.None, 0);
                    spriteBatch.Draw(Weapon.pixel, Vector2.Add(toShoot, adjustShot), null, Color.Black, angle + wave.layout.angleAdjust, 
                        Vector2.Zero, new Vector2(distance + wave.layout.distanceAdjust, lineThickness), SpriteEffects.None, 0);
                    toShoot = Vector2.Zero;
                }
            }
        }
    }
}
