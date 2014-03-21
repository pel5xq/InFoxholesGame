using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using InFoxholes.Util;

namespace InFoxholes.Layouts
{
    public class Layout
    {
        public Pather pather;
        public Vector2 enemySpawnPoint;
        public Vector2 offscreenPosition;
        public AnimatedSprite layoutBackdrop;
        public Vector2 countdownPosition;
        public Vector2 weaponPosition;
        public Vector2 weaponGunpoint;
        public AnimatedSprite gameOverSprite;
        public Vector2 scavengerSpawnPosition;
        public float angleAdjust = .25f; // for life lost pair of shots
        public float distanceAdjust = -15f;

        public virtual Vector2 getScavengerTrenchPlacement(int seat)
        {
            return Vector2.Zero;
        }
        public virtual Vector2 getHUDPlacement(int seat)
        {
            return Vector2.Zero;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            layoutBackdrop.Update();
            layoutBackdrop.Draw(spriteBatch, Vector2.Zero, 1.0f);
        }

        public virtual void DrawGameOver(SpriteBatch spriteBatch)
        {
            gameOverSprite.Update();
            gameOverSprite.Draw(spriteBatch, Vector2.Zero, 1.0f);
        }

        public virtual void Initialize(ContentManager Content)
        {
            
        }

        // Next thing: Get trench detectin moved to layout/pather
        
    }
}
