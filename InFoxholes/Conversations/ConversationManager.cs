using InFoxholes.Friendlies;
using InFoxholes.Layouts;
using InFoxholes.Windows;
using InFoxholes.Waves;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace InFoxholes.Conversations
{
    public class ConversationManager
    {

        int State;
        WaveManager manager;
        List<String> menNames;
        

        /* Magic Numbers */
        List<String> sampleNames = new List<String>() { "Andy", "Paul", "Evan", "Bill" };

        public void Initialize(ContentManager content, WaveManager waveManager)
        {
            manager = waveManager;
            menNames = sampleNames;
            State = 0;
        }

        public void Update(GameTime gametime, ScavengerManager scavengerManager)
        {
            if (State == 0)
            {

            }
            //...
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(manager.blankScreen, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            if (State == 0)
            {

            }
            //...
        }
        
    }
}
