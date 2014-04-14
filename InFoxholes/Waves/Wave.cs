using InFoxholes.Enemies;
using InFoxholes.Friendlies;
using InFoxholes.Layouts;
using InFoxholes.Looting;
using InFoxholes.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace InFoxholes.Waves
{
    public class Wave
    {
        public List<Enemy> enemiesToSpawn;
        public List<double> spawnTimings;
        public List<Enemy> enemiesOnScreen;
        public List<Loot> lootList;
        public int waveSize;
        ContentManager contentManager;
        double waveStartTime;
        public bool infiniteAmmoModeOn;
        public bool infiniteFoodModeOn;
        public String openingText;
        public String openingTextFilename;
        public WaveManager waveManager;
        public Layout layout;
        public bool isTutorialWave;
        public Texture2D helpTexture;
        public Texture2D helpTextureController;


        virtual public void Initialize(ContentManager content, WaveManager manager)
        {
            enemiesOnScreen = new List<Enemy>(waveSize);
            contentManager = content;
            waveStartTime = 0;
            openingText = getTextFromFile(openingTextFilename);
            waveManager = manager;
            layout.Initialize(content);
        }

        public bool isHit(Vector2 crosshairPosition)
        {
            bool retval = false;
            for (int i = 0; i < enemiesOnScreen.Count; i++)
            {
                if (enemiesOnScreen[i].isHit(crosshairPosition)) retval = true;
            }
            return retval;
        }

        public void Update(GameTime gametime, ScavengerManager scavengerManager)
        {
            if (waveStartTime == 0) waveStartTime = gametime.TotalGameTime.TotalMilliseconds;
            else if ((spawnTimings.Count > 0) && spawnTimings[0] <= gametime.TotalGameTime.TotalMilliseconds - waveStartTime)
            {
                enemiesToSpawn[0].Initialize(contentManager, layout.enemySpawnPoint(), lootList[0], this);
                enemiesOnScreen.Add(enemiesToSpawn[0]);
                enemiesToSpawn.RemoveAt(0);
                spawnTimings.RemoveAt(0);
                lootList.RemoveAt(0);
            }
            for (int i = 0; i < enemiesOnScreen.Count; i++)
            {
                enemiesOnScreen[i].Update(gametime, scavengerManager);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            layout.Draw(spriteBatch);
            for (int i = 0; i < enemiesOnScreen.Count; i++)
            {
                enemiesOnScreen[i].Draw(spriteBatch);
            }
        }

        virtual public void applyModes()
        {
            MainGame.isInfiniteAmmoMode = infiniteAmmoModeOn;
            MainGame.isInfiniteFoodMode = infiniteFoodModeOn;
        }

        public virtual bool isOver(ScavengerManager scavengerManager)
        {
            if (enemiesToSpawn.Count == 0)
            {
                for (int i = 0; i < enemiesOnScreen.Count; i++)
                {
                    if (enemiesOnScreen[i].Alive) return false;
                }
                return true;
            }
            return false;
        }

        public String getTextFromFile(String filename)
        {
            String result = "";
            try
            {
                using (StreamReader sr = new StreamReader(filename))
                {
                    String line = sr.ReadToEnd();
                    result += line;
                }
            }
            catch (Exception e)
            {
                result = "Error" + e;
            }
            return result;
        }
    }
}
