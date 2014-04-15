using InFoxholes.Looting;
using InFoxholes.Util;
using InFoxholes.Friendlies;
using InFoxholes.Waves;
using InFoxholes.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic; 

namespace InFoxholes.Enemies
{
    public class DogEnemy : RegionedEnemy
    {
        public static SoundEffectInstance dogBiteSound;

        /* Magic Numbers*/
        float E1Speed = .8f;
        int animationSpeed = 8;
        int numMapRows = 3;
        int numMapColumns = 3;
        float firinganimationRate = 150;
        int heightOffset = 30;
        Vector2 deathOffset = new Vector2(20,10);

        /* Regioned Enemy fields */
        List<double> enemyRegions = new List<double> { 0, .2, 1 };
        List<int> enemyDamages = new List<int> { 1, 1, 1 };
        List<SoundEffectInstance> enemySounds = new List<SoundEffectInstance> { 
            WaveManager.enemyShotSound, WaveManager.enemyShotSound, WaveManager.enemyShotSound };
        int enemyHealth = 1;

        override public void Initialize(ContentManager content, Vector2 position, Loot theLoot, Wave theWave)
        {
            dogBiteSound = (content.Load<SoundEffect>("Music\\DogBite.wav")).CreateInstance();
            EnemyDeathTexture = content.Load<Texture2D>("Graphics\\DogDead");
            FiringTexture = content.Load<Texture2D>("Graphics\\DogBite");
            EnemyTextureMap = new AnimatedSprite(content.Load<Texture2D>("Graphics\\DogMap"), numMapRows, numMapColumns, animationSpeed);
            speed = E1Speed;
            regions = enemyRegions;
            damages = enemyDamages;
            sounds = enemySounds;
            health = enemyHealth;
            base.Initialize(content, position, theLoot, theWave);
            firingAnimationRate = firinganimationRate;
            isLooted = true; //Dogs can't be looted
            Position.Y += heightOffset;
        }

        override public void Draw(SpriteBatch spriteBatch)
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
                    toShoot = Vector2.Zero;
                }
            }
        }

        override public void Update(GameTime gametime, ScavengerManager scavengerManager)
        {

            if (Alive && scavengerManager.getActiveScavenger().Alive)
            {
                if (wave.layout.pather.intersectsWith(Position, Width, Height, scavengerManager.getActiveScavenger().Position,
                    scavengerManager.getActiveScavenger().Width, scavengerManager.getActiveScavenger().Height))
                {
                    isFiring = true;
                    dogBiteSound.Play();
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
                        dogBiteSound.Play();
                        scavengerManager.getActiveScavenger().Alive = false;
                        beginFiringTime = gametime.TotalGameTime.TotalMilliseconds;
                        Alive = false;
                        Position = Vector2.Add(scavengerManager.getActiveScavenger().Position, deathOffset);
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
    }
}