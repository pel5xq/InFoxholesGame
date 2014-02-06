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
    class Wave1 : Wave
    {

        /* Magic Numbers */
        int wavesize = 10;
        double baseTime = 1000;
        double interval = 6500;

        override public void Initialize(ContentManager content, Vector2 position)
        {
            waveSize = wavesize;
            spawnTimings = new List<double>(waveSize);
            enemiesToSpawn = new List<Enemy>(waveSize);
            lootList = new List<Loot>(waveSize);
            for (int i = 0; i < waveSize; i++)
            {
                enemiesToSpawn.Insert(i, new Enemy1());
                spawnTimings.Insert(i, baseTime + i * interval);
            }
            lootList.Add(new SniperAmmoLoot(2));
            lootList.Add(new FoodLoot(1));
            lootList.Add(new MachineGunAmmoLoot(10));
            lootList.Add(new FoodLoot(1));
            lootList.Add(new MachineGunAmmoLoot(4));
            lootList.Add(new SniperAmmoLoot(4));
            lootList.Add(new MachineGunAmmoLoot(6));
            lootList.Add(new FoodLoot(1));
            lootList.Add(new SniperAmmoLoot(3));
            lootList.Add(new SniperAmmoLoot(1));
            base.Initialize(content, position);
        }
    }
}
