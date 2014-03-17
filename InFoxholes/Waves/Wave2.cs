using InFoxholes.Enemies;
using InFoxholes.Looting;
using InFoxholes.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace InFoxholes.Waves
{
    public class Wave2 : Wave
    {

        /* Magic Numbers */
        int wavesize = 10;
        double baseTime = 1000;
        double interval = 6500;

        override public void Initialize(ContentManager content, Vector2 position)
        {
            infiniteAmmoModeOn = false;
            infiniteFoodModeOn = false;
            waveSize = wavesize;
            spawnTimings = new List<double>(waveSize);
            enemiesToSpawn = new List<Enemy>(waveSize);
            lootList = new List<Loot>(waveSize);
            for (int i = 0; i < waveSize; i++)
            {
                enemiesToSpawn.Insert(i, new Enemy1());
                spawnTimings.Insert(i, baseTime + i * interval);
            }
            lootList.Add(new SniperAmmoLoot(2, content));
            lootList.Add(new FoodLoot(1, content));
            lootList.Add(new MachineGunAmmoLoot(10, content));
            lootList.Add(new FoodLoot(1, content));
            lootList.Add(new MachineGunAmmoLoot(4, content));
            lootList.Add(new SniperAmmoLoot(4, content));
            lootList.Add(new MachineGunAmmoLoot(6, content));
            lootList.Add(new FoodLoot(1, content));
            lootList.Add(new SniperAmmoLoot(3, content));
            lootList.Add(new SniperAmmoLoot(1, content));
            openingTextFilename = "Content//Text//Wave2Open.txt";
            base.Initialize(content, position);
        }

        override public void applyModes()
        {
            MainGame.initializeAmmo();
            base.applyModes();
        }
    }
}
