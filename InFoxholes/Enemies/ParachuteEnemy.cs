﻿using InFoxholes.Looting;
using InFoxholes.Util;
using InFoxholes.Waves;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace InFoxholes.Enemies
{
    public class ParachuteEnemy : Enemy1
    {
        /* Magic Numbers*/
        int counter = 0;
        bool inSky = true;
        int windSpeed = 0;
        int lastWindSpeed = 0;
        Texture2D FlyingTexture;
        Random rnd = new Random();
        int parachuteTopToBottom = 35;
        int parachuteBottomToHelmet = 70;
        bool isHitInParachute;
       


        override public void Initialize(ContentManager content, Vector2 position, Loot theLoot, Wave theWave)
        {

           FlyingTexture = content.Load<Texture2D>("Graphics\\ParachuteEnemy");
            /*
            EnemyDeathTexture = content.Load<Texture2D>("Graphics\\Enemy1Dead");
            FiringTexture = content.Load<Texture2D>("Graphics\\Enemy1Firing");
             
            EnemyTextureMap = new AnimatedSprite(content.Load<Texture2D>("Graphics\\Enemy1Map"), numMapRows, numMapColumns, animationSpeed);
            speed = E1Speed;
            base.Initialize(content, position, theLoot, theWave);
             * */

            base.Initialize(content, position, theLoot, theWave);

            if (inSky)
            {
                if (position.X < 0)
                {
                    speed *= 2;
                    Position = new Vector2(100, -75);
                }
                else
                {
                    speed *= 2;
                    Position = new Vector2(650, -75);
                }
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
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

              if (inSky)
              {
                  if (Alive)
                  {
                      if (isHitInParachute)
                      {
                          base.Draw(spriteBatch);
                      }
                      else
                      {
                          spriteBatch.Draw(FlyingTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
                      }
                  }
                  else
                  {
                      base.Draw(spriteBatch);
                  }

              }
              else
              {
                  base.Draw(spriteBatch);
              }

        }


        public override bool isHit(Vector2 crosshairPosition)
        {


            Vector2 truePosition = Vector2.Subtract(crosshairPosition, Position);

            if (inSky)
            {

                if (Alive &&
                    truePosition.X >= 0 &&
                    truePosition.Y >= 0 &&
                    truePosition.X <= Width &&
                    truePosition.Y <= parachuteTopToBottom)
                {

                    isHitInParachute = true;
                    Position.Y += 70;
                    return true;
                }

                else if (Alive &&
                   truePosition.X >= 0 &&
                   truePosition.Y > parachuteBottomToHelmet &&
                   truePosition.X <= Width &&
                   truePosition.Y <= Height)
                {
                    
                    Alive = false;
                    return true;
                }

                return false;
            }
            else
            {
                return base.isHit(crosshairPosition);
            }
        }

        public override void Update(GameTime gametime, Friendlies.ScavengerManager scavengerManager)
        {
            counter++;
          
         // creates a number between 1 and 12

            if (inSky)
            {
                if (isHitInParachute || !Alive)
                {
                    Position.Y += speed * 3;

                }
                bool hitground = false;
                if (isHitInParachute || !Alive)
                {
                    if (wave.layout.isOnGround(Position, Width, Height))
                    {
                        base.Update(gametime, scavengerManager);
                        inSky = false;
                        hitground = true;
                    }
                }
                else {
                    if (wave.layout.isOnGround(new Vector2(Position.X, Position.Y + 70), Width, Height))
                    {
                        if (Alive){
                            Position.Y += 70;
                        }
                        base.Update(gametime, scavengerManager);
                        inSky = false;
                        hitground = true;
                    }
                }
                if (hitground || Alive)
                {
                    if (counter % 30 == 0)
                    {
                        windSpeed = rnd.Next(-1, 1);
                        if (windSpeed == 0)
                            windSpeed = 1;

                        lastWindSpeed = windSpeed;
                    }
                    if (Alive)
                        Position.X += lastWindSpeed;

                        Position.Y += speed;
                }
            }
            else
            {
                base.Update(gametime, scavengerManager);
            }


        }
    }
}
