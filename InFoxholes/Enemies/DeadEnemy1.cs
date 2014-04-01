using InFoxholes.Looting;
using InFoxholes.Util;
using InFoxholes.Waves;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace InFoxholes.Enemies
{
    class DeadEnemy1 : Enemy1
    {
        override public void Initialize(ContentManager content, Vector2 position, Loot theLoot, Wave theWave)
        {
            base.Initialize(content, position, theLoot, theWave);
            Alive = false;
            Position = new Vector2(position.X/2, position.Y);
        }
    }
}
