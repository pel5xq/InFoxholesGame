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

namespace GameName1
{
    class WaveManager
    {
        public List<Wave> waves;
        public int currentWave;
        public int State; 
        // 0 = Wave Introduction, 1 = Wave Day
        // 2 = End of Day Countdown, 3 = Wave Conclusion

        /* Magic Numbers */
        //double gracePeriodLength = 15000;
        int numberOfImplementedWaves = 2;

        public void Initialize(ContentManager content, Vector2 position)
        {
            waves = new List<Wave>();
            currentWave = 0;
            State = 1;
            waves.Add(new Wave1());
            waves.Add(new Wave2());
            for (int i = 0; i < waves.Count; i++)
            {
                waves[i].Initialize(content, position);
            }
            getWave().applyModes();
        }

        public bool isHit(Vector2 crosshairPosition)
        {
            return getWave().isHit(crosshairPosition);
        }

        public void Update(GameTime gametime, Scavenger scavenger)
        {
            getWave().Update(gametime, scavenger);
            if (getWave().isOver())
            {
                nextWave();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            getWave().Draw(spriteBatch);
        }

        public Wave getWave()
        {
            return waves[currentWave];
        }

        public void nextWave()
        {
            if (currentWave + 1 < numberOfImplementedWaves)
            {
                currentWave++;
                getWave().applyModes();
            }
        }

    }
}
