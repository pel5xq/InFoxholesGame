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
        float speed = (float).4;
        int animationSpeed = 10;
        int numMapRows = 1;
        int numMapColumns = 4;

        override public void Initialize(ContentManager content, Vector2 position)
        {
            EnemyTexture = content.Load<Texture2D>("Graphics\\Enemy1");
            EnemyDeathTexture = content.Load<Texture2D>("Graphics\\Enemy1Dead");
            EnemyTextureMap = new AnimatedSprite(content.Load<Texture2D>("Graphics\\Enemy1Map"), numMapRows, numMapColumns, animationSpeed);
            Movement = new Vector2(-1 * speed, 0f);
            base.Initialize(content, position);   
        }
    }
}
