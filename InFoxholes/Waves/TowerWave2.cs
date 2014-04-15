﻿using InFoxholes.Enemies;
using InFoxholes.Layouts;
using InFoxholes.Looting;
using InFoxholes.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace InFoxholes.Waves
{
    public class TowerWave2 : Wave
    {

        /* Magic Numbers */
        int wavesize = 12;
        double baseTime = 1000;
        double interval = 6500;

        override public void Initialize(ContentManager content, WaveManager manager)
        {
            infiniteAmmoModeOn = false;
            infiniteFoodModeOn = true;
            isTutorialWave = false;
            waveSize = wavesize;
            spawnTimings = new List<double>(waveSize);
            enemiesToSpawn = new List<Enemy>(waveSize);
            lootList = new List<Loot>(waveSize);
            enemiesToSpawn.Insert(0, new HeadShotTest());
            spawnTimings.Insert(0, baseTime + 0 * interval);
            enemiesToSpawn.Insert(1, new DogEnemy());
            spawnTimings.Insert(1, baseTime + 1 * interval);
            enemiesToSpawn.Insert(2, new ParachuteEnemy());
            spawnTimings.Insert(2, baseTime + 2 * interval);
            enemiesToSpawn.Insert(3, new DogEnemy());
            spawnTimings.Insert(3, baseTime + 2 * interval);
            enemiesToSpawn.Insert(4, new DogEnemy());
            spawnTimings.Insert(4, baseTime + 3.5 * interval);
            enemiesToSpawn.Insert(5, new Enemy1());
            spawnTimings.Insert(5, baseTime + 4.5 * interval);
            enemiesToSpawn.Insert(6, new HumanShieldEnemy());
            spawnTimings.Insert(6, baseTime + 6.5 * interval);
            enemiesToSpawn.Insert(7, new ParachuteEnemy());
            spawnTimings.Insert(7, baseTime + 7 * interval);
            enemiesToSpawn.Insert(8, new HeadShotTest());
            spawnTimings.Insert(8, baseTime + 9 * interval);
            enemiesToSpawn.Insert(9, new HumanShieldEnemy());
            spawnTimings.Insert(9, baseTime + 9.5 * interval);
            //enemiesToSpawn.Insert(10, new HeadShotTest());
            enemiesToSpawn.Insert(10, new ParachuteEnemy());
            spawnTimings.Insert(10, baseTime + 10.5 * interval);
            enemiesToSpawn.Insert(11, new HeadShotTest());
            spawnTimings.Insert(11, baseTime + 11.5 * interval);
            lootList.Add(new SniperAmmoLoot(2, content));
            lootList.Add(new SniperAmmoLoot(2, content));
            lootList.Add(new MachineGunAmmoLoot(10, content));
            lootList.Add(new SniperAmmoLoot(2, content));
            lootList.Add(new MachineGunAmmoLoot(4, content));
            lootList.Add(new SniperAmmoLoot(4, content));
            lootList.Add(new MachineGunAmmoLoot(6, content));
            lootList.Add(new SniperAmmoLoot(2, content));
            lootList.Add(new SniperAmmoLoot(3, content));
            lootList.Add(new SniperAmmoLoot(2, content));
            lootList.Add(new MachineGunAmmoLoot(6, content));
            lootList.Add(new SniperAmmoLoot(2, content));
            openingTextFilename = "Content//Text//TowerWave2.txt";
            layout = new TowerLayout();
            base.Initialize(content, manager);
        }

    }
}