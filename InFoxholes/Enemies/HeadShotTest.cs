using InFoxholes.Looting;
using InFoxholes.Util;
using InFoxholes.Weapons;
using InFoxholes.Friendlies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace InFoxholes.Enemies
{
    public class HeadShotTest : RegionedEnemy
    {
        public AnimatedSprite DamagedTextureMap;

        /* Magic Numbers*/
        float E1Speed = .9f;
        float E1SpeedDamaged = .45f;
        int animationSpeed = 10;
        int numMapRows = 1;
        int numMapColumns = 4;

        /* Regioned Enemy fields */
        List<double> testHSRegions = new List<double> { 0, .2, 1 };
        List<int> testHSDamages = new List<int> { 2, 1, 1 };
        int testHSHealth = 2; 

        override public void Initialize(ContentManager content, Vector2 position, Loot theLoot)
        {
            EnemyTexture = content.Load<Texture2D>("Graphics\\Enemy1");
            EnemyDeathTexture = content.Load<Texture2D>("Graphics\\Enemy1Dead");
            FiringTexture = content.Load<Texture2D>("Graphics\\Enemy1Firing");
            EnemyTextureMap = new AnimatedSprite(content.Load<Texture2D>("Graphics\\Enemy1Map"), numMapRows, numMapColumns, animationSpeed);
            DamagedTextureMap = new AnimatedSprite(content.Load<Texture2D>("Graphics\\Enemy1DamagedMap"), numMapRows, numMapColumns, animationSpeed);
            speed = E1Speed;
            regions = testHSRegions;
            damages = testHSDamages;
            health = testHSHealth; 
            base.Initialize(content, position, theLoot);
        }

        override public void Update(GameTime gametime, ScavengerManager scavengerManager)
        {
            if (health < testHSHealth)
            {
                DamagedTextureMap.Update();
                speed = E1SpeedDamaged;
            }
            base.Update(gametime, scavengerManager);
        }

        override public void Draw(SpriteBatch spriteBatch)
        {
            if (Alive)
            {
                if (isFiring)
                {
                    spriteBatch.Draw(FiringTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }
                else if (health == testHSHealth)
                {
                    EnemyTextureMap.Draw(spriteBatch, Position, 1f);
                }
                else
                {
                    DamagedTextureMap.Draw(spriteBatch, Position, 1f);
                }
            }
            else
            {
                spriteBatch.Draw(EnemyDeathTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                if (!toShoot.Equals(Vector2.Zero))
                {
                    float distance = Vector2.Distance(toShoot, gunPoint);
                    float angle = (float)Math.Atan2(gunPoint.Y - toShoot.Y, gunPoint.X - toShoot.X);
                    spriteBatch.Draw(Weapon.pixel, toShoot, null, Color.Black, angle, Vector2.Zero, new Vector2(distance, lineThickness),
                                 SpriteEffects.None, 0);
                    spriteBatch.Draw(Weapon.pixel, Vector2.Add(toShoot, adjustShot), null, Color.Black, angle + angleAdjust,
                        Vector2.Zero, new Vector2(distance + distanceAdjust, lineThickness), SpriteEffects.None, 0);
                    toShoot = Vector2.Zero;
                }
            }
        }
    }
}
