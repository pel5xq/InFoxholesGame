using InFoxholes.Waves;
using InFoxholes.Weapons;
using InFoxholes.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace InFoxholes.Friendlies
{
    public class ScavengerManager
    {
        List<Scavenger> scavengers;
        List<Vector2> positions;
        int activeScavenger;

        /* Magic Numbers */
        Vector2 firstIdlePosition = new Vector2(30, 300);
        Vector2 secondIdlePosition = new Vector2(30, 350);
        Vector2 thirdIdlePosition = new Vector2(0, 320);
        Vector2 fourthIdlePosition = new Vector2(70, 350);
        //Trench must be bigger for more scavengers
        Vector2 offscreenPosition = new Vector2(-150, -150);

        public Scavenger getActiveScavenger()
        {
            return scavengers[activeScavenger];
        }
        public bool isHit(Vector2 crosshairPosition)
        {
            return getActiveScavenger().isHit(crosshairPosition);
        }
        virtual public void Initialize(ContentManager content, Vector2 spawnPosition, Vector2 HUDPosition, int numLives)
        {
            positions = new List<Vector2>();
            positions.Add(firstIdlePosition);
            positions.Add(secondIdlePosition);
            positions.Add(thirdIdlePosition);
            positions.Add(fourthIdlePosition);

            scavengers = new List<Scavenger>(numLives);
            scavengers.Insert(0, new Scavenger());
            scavengers[0].Initialize(content, positions[0], spawnPosition, HUDPosition, true);
            activeScavenger = 0;
            for (int i = 1; i < numLives; i++)
            {
                scavengers.Insert(i, new Scavenger());
                scavengers[i].Initialize(content, positions[i], spawnPosition, HUDPosition, false);
            }
        }
        public void Update(int command, GameTime gameTime, Wave wave)
        {
            if (!scavengers[activeScavenger].Alive && activeScavenger < scavengers.Count - 1)
            {
                scavengers[activeScavenger].Active = false;
                activeScavenger++;
                scavengers[activeScavenger].Active = true;
                //Don't use last scavenger's commands at death
                command = -1;
                MainGame.resetScavengeCommand();
            }
            for (int i = 0; i < scavengers.Count; i++)
            {
                if (i == activeScavenger)
                {
                    scavengers[i].Update(command, gameTime, wave, this);
                }
                else
                {
                    scavengers[i].Update(-1, gameTime, wave, this);
                }
            }
        }
        public void addLootToSupply(SniperRifle sr, MachineGun mg, Player player)
        {
            getActiveScavenger().addLootToSupply(sr, mg, player);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < scavengers.Count; i++)
            {
                scavengers[i].Draw(spriteBatch);
            }
        }
        public void returnToTrench()
        {
            getActiveScavenger().returnToTrench();
        }
        public void cleanUpBodies()
        {
            for (int i = 0; i < scavengers.Count; i++)
            {
                if (!scavengers[i].Alive)
                {
                    scavengers[i].Position = offscreenPosition;
                }
            }
        }
        public List<Scavenger> getScavengableScavengers()
        {
            List<Scavenger> lootableScavs = new List<Scavenger>();
            for (int i = 0; i < scavengers.Count; i++)
            {
                if (!scavengers[i].Alive && scavengers[i].scavengedLoot.Count > 0 && !scavengers[i].isLooted) // && scavengers[i].action != 0 ?
                {
                    lootableScavs.Add(scavengers[i]);
                }
            }
            return lootableScavs;
        }
    }
}
