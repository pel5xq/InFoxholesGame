using InFoxholes.Enemies;
using InFoxholes.Layouts;
using InFoxholes.Looting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace InFoxholes.Waves
{
    public class Wave1 : Wave
    {

        /* Magic Numbers */
        int wavesize = 3;
        double baseTime = 1500;
        double interval = 8000;

        override public void Initialize(ContentManager content, WaveManager manager)
        {
            infiniteAmmoModeOn = true;
            infiniteFoodModeOn = true;
            waveSize = wavesize;
            spawnTimings = new List<double>(waveSize);
            enemiesToSpawn = new List<Enemy>(waveSize);
            lootList = new List<Loot>(waveSize);
            for (int i = 0; i < waveSize-1; i++)
            {
                enemiesToSpawn.Insert(i, new Enemy1());
                spawnTimings.Insert(i, baseTime + i * interval);
            }
            enemiesToSpawn.Insert(waveSize-1, new HeadShotTest());
            spawnTimings.Insert(waveSize - 1, baseTime + (waveSize - 1) * interval);
            lootList.Add(new SniperAmmoLoot(2, content));
            lootList.Add(new FoodLoot(1, content));
            lootList.Add(new MachineGunAmmoLoot(10, content));
            openingTextFilename = "Content//Text//Wave1Open.txt";
            layout = new FoxholeLayout();
            base.Initialize(content, manager);
        }
    }
}
