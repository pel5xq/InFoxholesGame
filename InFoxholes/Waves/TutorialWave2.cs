using InFoxholes.Enemies;
using InFoxholes.Friendlies;
using InFoxholes.Layouts;
using InFoxholes.Looting;
using InFoxholes.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace InFoxholes.Waves
{
    public class TutorialWave2 : Wave
    {
        override public void Initialize(ContentManager content, WaveManager manager)
        {
            infiniteAmmoModeOn = true;
            infiniteFoodModeOn = true;
            isTutorialWave = true;
            spawnTimings = new List<double>(waveSize);
            enemiesToSpawn = new List<Enemy>(waveSize);
            lootList = new List<Loot>(waveSize);

            enemiesToSpawn.Insert(0, new DeadEnemy1());
            spawnTimings.Insert(0, 0);
            lootList.Add(new SniperAmmoLoot(2, content));
            openingTextFilename = "Content//Text//TutorialWave2Open.txt";
            layout = new FoxholeLayout();
            helpTexture = content.Load<Texture2D>("Graphics\\Tutorial2Help");
            helpTextureController = content.Load<Texture2D>("Graphics\\Tutorial2HelpController");
            base.Initialize(content, manager);
        }

        override public bool isOver(ScavengerManager scavengerManager)
        {
            if (enemiesOnScreen.Count > 0)
                return enemiesOnScreen[0].isLooted && scavengerManager.getActiveScavenger().action == 0;
            return false;
        }
    }
}
