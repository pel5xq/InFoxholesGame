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
using System.IO;

namespace GameName1
{
    public class Wave
    {
        public List<Enemy> enemiesToSpawn;
        public List<double> spawnTimings;
        public List<Enemy> enemiesOnScreen;
        public List<Loot> lootList;
        public int waveSize;
        Vector2 spawnPoint;
        ContentManager contentManager;
        double waveStartTime;
        public bool infiniteAmmoModeOn;
        public bool infiniteFoodModeOn;
        public String openingText;
        public String openingTextFilename;

        virtual public void Initialize(ContentManager content, Vector2 position)
        {
            enemiesOnScreen = new List<Enemy>(waveSize);
            spawnPoint = position;
            contentManager = content;
            waveStartTime = 0;
            openingText = getTextFromFile(openingTextFilename);
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
                enemiesToSpawn[0].Initialize(contentManager, spawnPoint, lootList[0]);
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
            for (int i = 0; i < enemiesOnScreen.Count; i++)
            {
                enemiesOnScreen[i].Draw(spriteBatch);
            }
        }

        virtual public void applyModes()
        {
            Game1.isInfiniteAmmoMode = infiniteAmmoModeOn;
            Game1.isInfiniteFoodMode = infiniteFoodModeOn;
        }

        public bool isOver()
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
