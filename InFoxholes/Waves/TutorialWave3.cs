using InFoxholes.Enemies;
using InFoxholes.Layouts;
using InFoxholes.Looting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace InFoxholes.Waves
{
    public class TutorialWave3 : Wave
    {

        /* Magic Numbers */
        int wavesize = 9;
        double baseTime = 1500;
        double interval = 7000;

        override public void Initialize(ContentManager content, WaveManager manager)
        {
            infiniteAmmoModeOn = false;
            infiniteFoodModeOn = true;
            isTutorialWave = true;
            waveSize = wavesize;
            spawnTimings = new List<double>(waveSize);
            enemiesToSpawn = new List<Enemy>(waveSize);
            lootList = new List<Loot>(waveSize);
           // for (int i = 0; i < waveSize - 1; i++)
           // {
           
            enemiesToSpawn.Insert(0, new Enemy1());
            spawnTimings.Insert(0, baseTime + 0 * interval);
            enemiesToSpawn.Insert(1, new Enemy1());
            spawnTimings.Insert(1, baseTime + 1 * interval);
            enemiesToSpawn.Insert(2, new HeadShotTest());
            spawnTimings.Insert(2, baseTime + 2 * interval);
            enemiesToSpawn.Insert(3, new Enemy1());
            spawnTimings.Insert(3, baseTime + 2 * interval);
            enemiesToSpawn.Insert(4, new HeadShotTest());
            spawnTimings.Insert(4, baseTime + 3 * interval);
            enemiesToSpawn.Insert(5, new Enemy1());
            spawnTimings.Insert(5, baseTime + 4 * interval);
            enemiesToSpawn.Insert(6, new Enemy1());
            spawnTimings.Insert(6, baseTime + 5 * interval);
           // }
            enemiesToSpawn.Insert(7, new HeadShotTest());
            spawnTimings.Insert(7, baseTime + 6 * interval);
            lootList.Add(new SniperAmmoLoot(2, content));
            lootList.Add(new SniperAmmoLoot(2, content));
            lootList.Add(new MachineGunAmmoLoot(10, content));
            lootList.Add(new SniperAmmoLoot(2, content));
            lootList.Add(new MachineGunAmmoLoot(10, content));
            lootList.Add(new SniperAmmoLoot(2, content));
            lootList.Add(new SniperAmmoLoot(2, content));
            lootList.Add(new MachineGunAmmoLoot(10, content));
            openingTextFilename = "Content//Text//TutorialWave3Open.txt";
            layout = new FoxholeLayout();
            helpTexture = content.Load<Texture2D>("Graphics\\Tutorial3Help");
            helpTextureController = content.Load<Texture2D>("Graphics\\Tutorial3HelpController");
            base.Initialize(content, manager);
        }
    }
}
