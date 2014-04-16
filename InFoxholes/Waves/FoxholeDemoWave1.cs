using InFoxholes.Enemies;
using InFoxholes.Layouts;
using InFoxholes.Looting;
using InFoxholes.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace InFoxholes.Waves
{
    public class FoxholeDemoWave1 : Wave
    {

        /* Magic Numbers */
        int wavesize = 8;
        double baseTime = 1500;
        double interval = 8000;

        /*  */

        override public void Initialize(ContentManager content, WaveManager manager)
        {
            infiniteAmmoModeOn = false;
            infiniteFoodModeOn = true;
            isTutorialWave = false;
            waveSize = wavesize;
            spawnTimings = new List<double>(waveSize);
            enemiesToSpawn = new List<Enemy>(waveSize);
            lootList = new List<Loot>(waveSize);
            enemiesToSpawn.Insert(0, new Enemy1());
            spawnTimings.Insert(0, baseTime + 0 * interval);
            enemiesToSpawn.Insert(1, new Enemy1());
            spawnTimings.Insert(1, baseTime + 1.5 * interval);
            //enemiesToSpawn.Insert(2, new HeadShotTest());
            enemiesToSpawn.Insert(2, new DogEnemy());
            spawnTimings.Insert(2, baseTime + 3 * interval);
            enemiesToSpawn.Insert(3, new Enemy1());
            spawnTimings.Insert(3, baseTime + 4 * interval);
            enemiesToSpawn.Insert(4, new HeadShotTest());
            spawnTimings.Insert(4, baseTime + 5.5 * interval);
            enemiesToSpawn.Insert(5, new Enemy1());
            spawnTimings.Insert(5, baseTime + 6.5 * interval);
            enemiesToSpawn.Insert(6, new Enemy1());
            spawnTimings.Insert(6, baseTime + 7.5 * interval);
            enemiesToSpawn.Insert(7, new DogEnemy());
            spawnTimings.Insert(7, baseTime + 8.5 * interval);
            lootList.Add(new SniperAmmoLoot(2, content));
            lootList.Add(new SniperAmmoLoot(2, content));
            lootList.Add(new MachineGunAmmoLoot(10, content));
            lootList.Add(new SniperAmmoLoot(2, content));
            lootList.Add(new MachineGunAmmoLoot(4, content));
            lootList.Add(new SniperAmmoLoot(4, content));
            lootList.Add(new MachineGunAmmoLoot(6, content));
            lootList.Add(new SniperAmmoLoot(0, content));

            openingTextFilename = "Content//Text//FoxholeDemoWave1.txt";
            layout = new FoxholeLayout();
            base.Initialize(content, manager);
        }
        override public void applyModes()
        {
            MainGame.initializeAmmo();
            base.applyModes();
        }
    }
}
