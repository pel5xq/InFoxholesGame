using InFoxholes.Enemies;
using InFoxholes.Layouts;
using InFoxholes.Looting;
using InFoxholes.Friendlies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace InFoxholes.Waves
{
    public class LadderWave1 : Wave
    {

        /* Magic Numbers */
        int wavesize = 1;
        double baseTime = 1500;
        double interval = 8000;

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
            lootList.Add(new SniperAmmoLoot(2, content));
            openingTextFilename = "Content//Text//LadderWave1.txt";
            layout = new LadderLayout();
            base.Initialize(content, manager);
        }
    }
}
