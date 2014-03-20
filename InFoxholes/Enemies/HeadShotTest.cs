using System.Collections.Generic; 
using InFoxholes.Looting;
using InFoxholes.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace InFoxholes.Enemies
{
    public class HeadShotTest : RegionedEnemy
    {
        /* Magic Numbers*/
        float E1Speed = .9f;
        int animationSpeed = 10;
        int numMapRows = 1;
        int numMapColumns = 4;

        /* Regioned Enemy fields */
        List<double> testHSRegions = new List<double> { .5, 1 };
        List<int> testHSDamages = new List<int> { 2, 1 };
        int testHSHealth = 2; 

        override public void Initialize(ContentManager content, Vector2 position, Loot theLoot)
        {
            EnemyTexture = content.Load<Texture2D>("Graphics\\Enemy1");
            EnemyDeathTexture = content.Load<Texture2D>("Graphics\\Enemy1Dead");
            FiringTexture = content.Load<Texture2D>("Graphics\\Enemy1Firing");
            EnemyTextureMap = new AnimatedSprite(content.Load<Texture2D>("Graphics\\Enemy1Map"), numMapRows, numMapColumns, animationSpeed);
            speed = E1Speed;
            regions = testHSRegions;
            damages = testHSDamages;
            health = testHSHealth; 
            base.Initialize(content, position, theLoot);
        }
    }
}
