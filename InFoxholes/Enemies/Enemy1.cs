using InFoxholes.Looting;
using InFoxholes.Util;
using InFoxholes.Waves;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic; 

namespace InFoxholes.Enemies
{
    public class Enemy1 : RegionedEnemy
    {
        /* Magic Numbers*/
        float E1Speed = .4f;
        int animationSpeed = 10;
        int numMapRows = 1;
        int numMapColumns = 4;

        /* Regioned Enemy fields */
        List<double> enemyRegions = new List<double> { 0, .2, 1 };
        List<int> enemyDamages = new List<int> { 1, 1, 1 };
        List<SoundEffectInstance> enemySounds = new List<SoundEffectInstance> { 
            WaveManager.headshotSound, WaveManager.enemyShotSound, WaveManager.enemyShotSound };
        int enemyHealth = 1;

        override public void Initialize(ContentManager content, Vector2 position, Loot theLoot, Wave theWave)
        {
            EnemyDeathTexture = content.Load<Texture2D>("Graphics\\Enemy1Dead");
            FiringTexture = content.Load<Texture2D>("Graphics\\Enemy1Firing");
            EnemyTextureMap = new AnimatedSprite(content.Load<Texture2D>("Graphics\\Enemy1Map"), numMapRows, numMapColumns, animationSpeed);
            speed = E1Speed;
            regions = enemyRegions;
            damages = enemyDamages;
            sounds = enemySounds;
            health = enemyHealth;
            base.Initialize(content, position, theLoot, theWave);   
        }
    }
}
