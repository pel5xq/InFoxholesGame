using InFoxholes.Enemies;
using InFoxholes.Layouts;
using InFoxholes.Looting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace InFoxholes.Waves
{
    public class TutorialWave1 : Wave
    {

        /* Magic Numbers */
        int wavesize = 5;
        double baseTime = 1500;
        double interval = 8000;

        override public void Initialize(ContentManager content, WaveManager manager)
        {

            infiniteAmmoModeOn = true;
            infiniteFoodModeOn = true;
            isTutorialWave = true;
            waveSize = wavesize;
            spawnTimings = new List<double>(waveSize);
            enemiesToSpawn = new List<Enemy>(waveSize);
            lootList = new List<Loot>(waveSize);
            enemiesToSpawn.Insert(0, new Enemy1());
            spawnTimings.Insert(0, baseTime + 0 * interval);
            enemiesToSpawn.Insert(1, new Enemy1());
            spawnTimings.Insert(1, 2 * interval);
            enemiesToSpawn.Insert(2, new Enemy1());
            spawnTimings.Insert(2, 3.5 * interval);
            enemiesToSpawn.Insert(3, new Enemy1());
            spawnTimings.Insert(3, 5 * interval);
            enemiesToSpawn.Insert(waveSize - 1, new HeadShotTest());
            spawnTimings.Insert(waveSize - 1, baseTime + 6.5 * interval);
            lootList.Add(new SniperAmmoLoot(2, content));
            lootList.Add(new SniperAmmoLoot(2, content));
            //lootList.Add(new FoodLoot(1, content));
            lootList.Add(new MachineGunAmmoLoot(5, content));
            lootList.Add(new SniperAmmoLoot(2, content));
            lootList.Add(new MachineGunAmmoLoot(5, content));
            openingTextFilename = "Content//Text//TutorialWave1Open.txt";
            layout = new FoxholeLayout();
            helpTexture = content.Load<Texture2D>("Graphics\\Tutorial1Help");
            helpTextureController = content.Load<Texture2D>("Graphics\\Tutorial1HelpController");
            base.Initialize(content, manager);
        }
    }
}
