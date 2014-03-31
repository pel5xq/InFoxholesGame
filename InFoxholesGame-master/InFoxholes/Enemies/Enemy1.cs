using InFoxholes.Looting;
using InFoxholes.Util;
using InFoxholes.Waves;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace InFoxholes.Enemies
{
    public class Enemy1 : Enemy
    {
        /* Magic Numbers*/
        float E1Speed = .4f;
        int animationSpeed = 10;
        int numMapRows = 1;
        int numMapColumns = 4;

        override public void Initialize(ContentManager content, Vector2 position, Loot theLoot, Wave theWave)
        {
            EnemyDeathTexture = content.Load<Texture2D>("Graphics\\Enemy1Dead");
            FiringTexture = content.Load<Texture2D>("Graphics\\Enemy1Firing");
            EnemyTextureMap = new AnimatedSprite(content.Load<Texture2D>("Graphics\\Enemy1Map"), numMapRows, numMapColumns, animationSpeed);
            speed = E1Speed;
            base.Initialize(content, position, theLoot, theWave);   
        }
    }
}
