using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InFoxholes.Looting;
using InFoxholes.Util;
using InFoxholes.Waves;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;


namespace InFoxholes.Enemies
{
    class TankEnemy : Enemy1
    {

        int heightOffset = -130;
        Texture2D TankTexture;
        float tankSpeed = .3f;
        int tankHealth = 3;
        float firinganimationRate = 150;

        public override void Initialize(Microsoft.Xna.Framework.Content.ContentManager content, Microsoft.Xna.Framework.Vector2 position, Looting.Loot theLoot, Waves.Wave theWave)
        {

            TankTexture = content.Load<Texture2D>("Graphics\\TankEnemy");

            base.Initialize(content, position, theLoot, theWave);

 
            speed = tankSpeed;
          //  regions = enemyRegions;
           // damages = enemyDamages;
            //sounds = enemySounds;
            health = tankHealth;
            base.Initialize(content, position, theLoot, theWave);
            firingAnimationRate = firinganimationRate;
            isLooted = true; //Dogs can't be looted
            Position.Y += heightOffset;
         
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

            spriteBatch.Draw(TankTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);



        }
    }
}
