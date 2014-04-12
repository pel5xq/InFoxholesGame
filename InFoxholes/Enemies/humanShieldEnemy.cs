using InFoxholes.Looting;
using InFoxholes.Util;
using InFoxholes.Waves;
using InFoxholes.Friendlies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic; 

namespace InFoxholes.Enemies
{
    public class HumanShieldEnemy : RegionedEnemy
    {
        public Texture2D shieldFiringTexture;
        public AnimatedSprite shieldTextureMap;
        public bool isHoldingShield;

        /* Magic Numbers*/
        float E1Speed = .4f;
        int animationSpeed = 10;
        int numMapRows = 1;
        int numMapColumns = 4;

        /* Regioned Enemy fields */
        List<double> enemyRegions = new List<double> { 0, .2, 1 };
        List<int> enemyDamages = new List<int> { 1, 0, 0 };
        List<SoundEffectInstance> enemySounds = new List<SoundEffectInstance> { 
            WaveManager.headshotSound, WaveManager.enemyShotSound, WaveManager.enemyShotSound };
        int enemyHealth = 1;
        float headCutoff = .2f;

        override public void Initialize(ContentManager content, Vector2 position, Loot theLoot, Wave theWave)
        {
            EnemyDeathTexture = content.Load<Texture2D>("Graphics\\EnemyShieldDead");
            FiringTexture = content.Load<Texture2D>("Graphics\\EnemyPreShieldFiring");
            EnemyTextureMap = new AnimatedSprite(content.Load<Texture2D>("Graphics\\EnemyPreShieldMap"), numMapRows, numMapColumns, animationSpeed);
            shieldFiringTexture = content.Load<Texture2D>("Graphics\\EnemyShieldFiring");
            shieldTextureMap = new AnimatedSprite(content.Load<Texture2D>("Graphics\\EnemyShieldMap"), numMapRows, numMapColumns, animationSpeed);
            speed = E1Speed;
            regions = enemyRegions;
            damages = enemyDamages;
            sounds = enemySounds;
            health = enemyHealth;
            isHoldingShield = false;
            base.Initialize(content, position, theLoot, theWave);   
        }

        override public bool isHit(Vector2 crosshairPosition)
        {
            if (isHoldingShield) return base.isHit(crosshairPosition);
            else
            {
                Vector2 truePosition = Vector2.Subtract(crosshairPosition, Position);
                if (Alive &&
                    truePosition.X >= 0 &&
                    truePosition.Y >= 0 &&
                    truePosition.X <= Width &&
                    truePosition.Y <= Height)
                {
                    if (truePosition.Y / Height > headCutoff) WaveManager.enemyShotSound.Play();
                    else WaveManager.headshotSound.Play();
                    Alive = false;
                    return true;
                }
                return false;
            }
        }

        public override void Update(GameTime gametime, ScavengerManager scavengerManager)
        {
            if (Alive && !isHoldingShield)
            {
                for (int i = 0; i < wave.enemiesOnScreen.Count; i++)
                {
                    if (wave.enemiesOnScreen[i] != this)
                    {
                        if (!wave.enemiesOnScreen[i].Alive && intersectsLow(wave.enemiesOnScreen[i]))
                        {
                            isHoldingShield = true;
                            wave.enemiesOnScreen[i].Position = wave.layout.offscreenPosition;
                            break;
                        }
                    }
                }
            }
            if (isHoldingShield) shieldTextureMap.Update();
            base.Update(gametime, scavengerManager);
        }

        private bool intersectsLow(Enemy enemy)
        {
            Vector2 botLeft = new Vector2(Position.X, Position.Y + Height);
            Vector2 botRight = new Vector2(Position.X + Width, Position.Y + Height);
            if ( (botLeft.X >= enemy.Position.X && botLeft.X <= enemy.Position.X + enemy.Width &&
                botLeft.Y >= enemy.Position.Y && botLeft.Y <= enemy.Position.Y + enemy.Height) ||
                (botRight.X >= enemy.Position.X && botRight.X <= enemy.Position.X + enemy.Width &&
                botRight.Y >= enemy.Position.Y && botRight.Y <= enemy.Position.Y + enemy.Height))
            {
                return true;
            }
            return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isHoldingShield && Alive)
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
                    spriteBatch.Draw(shieldFiringTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
                }
                else
                {
                    shieldTextureMap.Draw(spriteBatch, Position, 1f, effect);
                }
            }
            else
            {
                base.Draw(spriteBatch);
            }
        }
    }
}
