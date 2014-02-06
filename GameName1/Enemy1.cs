using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace GameName1
{
    class Enemy1 : Enemy
    {
        /* Magic Numbers*/
        float E1Speed = .4f;
        int animationSpeed = 10;
        int numMapRows = 1;
        int numMapColumns = 4;

        override public void Initialize(ContentManager content, Vector2 position)
        {
            EnemyTexture = content.Load<Texture2D>("Graphics\\Enemy1");
            EnemyDeathTexture = content.Load<Texture2D>("Graphics\\Enemy1Dead");
            FiringTexture = content.Load<Texture2D>("Graphics\\Enemy1Firing");
            EnemyTextureMap = new AnimatedSprite(content.Load<Texture2D>("Graphics\\Enemy1Map"), numMapRows, numMapColumns, animationSpeed);
            speed = E1Speed;
            base.Initialize(content, position);   
        }
    }
}
