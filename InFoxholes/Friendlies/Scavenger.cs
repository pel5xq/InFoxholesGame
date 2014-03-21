using InFoxholes.Enemies;
using InFoxholes.Looting;
using InFoxholes.Layouts;
using InFoxholes.Targeting;
using InFoxholes.Util;
using InFoxholes.Waves;
using InFoxholes.Weapons;
using InFoxholes.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace InFoxholes.Friendlies
{
    public class Scavenger : Targetable
    {

        public bool Alive;
        public Vector2 Position;
        public float speed;
        public Texture2D idleTexture;
        public Texture2D deathTexture;
        public Texture2D trenchDeathTexture;
        public Texture2D scavengingTexture;
        public AnimatedSprite activeTexture;
        public AnimatedSprite reverseTexture;
        public Texture2D sentOutHudTexture;
        public Texture2D sentBackHudTexture;
        public Texture2D notSentHudTexture;
        public int hudSeat;
        public int action; //0 means unsent, 1 means sent out, 2 means sent back, 3 means actively scavenging
        public List<Loot> scavengedLoot;
        double whenScavengeBegan;
        int actionToReturnTo;
        List<Loot> lootToLoot;
        public bool Active;
        public bool isLooted;
        public ScavengerManager scavengerManager;
        public int positionNumber;

        /* Magic Numbers*/
        float speedValue = .8f;
        int animationSpeed = 10;
        int numMapRows = 1;
        int numMapColumns = 4;
        double timeToScavenge = 3000;
        int lootBuffer = 5;

        public bool isHit(Vector2 crosshairPosition)
        {
            Vector2 truePosition = Vector2.Subtract(crosshairPosition, Position);
            if (Alive &&
                truePosition.X >= 0 &&
                truePosition.Y >= 0 &&
                truePosition.X <= Width &&
                truePosition.Y <= Height)
            {
                Alive = false;
                return true;
            }
            return false;
        }

        public int Width
        {
            get { return activeTexture.Texture.Width / activeTexture.Columns; }
        }

        public int Height
        {
            get { return activeTexture.Texture.Height / activeTexture.Rows; }
        }

        virtual public void Initialize(ContentManager content, int scavPositionNumber, int HUDPosition, bool activeNow, ScavengerManager manager)
        {
            idleTexture = content.Load<Texture2D>("Graphics\\TrooperIdle");
            deathTexture = content.Load<Texture2D>("Graphics\\TrooperDead");
            trenchDeathTexture = content.Load<Texture2D>("Graphics\\TrooperDeadInTrench");
            scavengingTexture = content.Load<Texture2D>("Graphics\\TrooperScavenging");
            activeTexture = new AnimatedSprite(content.Load<Texture2D>("Graphics\\Trooper"), numMapRows, numMapColumns, animationSpeed);
            reverseTexture = new AnimatedSprite(content.Load<Texture2D>("Graphics\\TrooperReverse"), numMapRows, numMapColumns, animationSpeed);
            sentOutHudTexture = content.Load<Texture2D>("Graphics\\SendOutIcon");
            sentBackHudTexture = content.Load<Texture2D>("Graphics\\SendBackIcon");
            notSentHudTexture = content.Load<Texture2D>("Graphics\\NotSentIcon");
            speed = speedValue;
            positionNumber = scavPositionNumber;
            scavengerManager = manager;
            Position = scavengerManager.waveManager.getWave().layout.getScavengerTrenchPlacement(positionNumber);
            hudSeat = HUDPosition;
            Alive = true;
            isLooted = false;
            action = 0;
            scavengedLoot = new List<Loot>();
            whenScavengeBegan = 0;
            Active = activeNow;
            
        }

        public void Update(int command, GameTime gameTime, Wave wave, ScavengerManager manager)
        {
            Vector2 scavengerSpawn = scavengerManager.waveManager.getWave().layout.scavengerSpawnPosition;
            Vector2 scavengerIdle = scavengerManager.waveManager.getWave().layout.getScavengerTrenchPlacement(positionNumber);
            if (Alive)
            {
                if (action == 0) //Idling
                {
                    //If command is to go, then put in scavenging spawn point
                    if (command == 1) 
                    {
                        Position = scavengerSpawn;
                        action = 1;
                    }
                }
                else if (action == 1) //Going out
                {
                    //If command is to come back, turn around
                    if (command == 0)
                    {
                        if (scavengerManager.waveManager.getWave().layout.pather.atTrenchEntrance(Position, Width, Height))
                        {
                            Position = scavengerIdle;
                            action = 0;
                        }
                        else 
                        {
                            Position = scavengerManager.waveManager.getWave().layout.pather.Move(Position, true, speed);
                            reverseTexture.Update();
                            action = 2;
                        }
                    }
                    else //else scavenge
                    {
                        List<Scavenger> lootableScavengers = manager.getScavengableScavengers();
                        //If no lootable things, just move forward
                        if (wave.enemiesOnScreen.Count + lootableScavengers.Count == 0)
                        {
                            Position = scavengerManager.waveManager.getWave().layout.pather.Move(Position, false, speed);
                            activeTexture.Update();
                        }
                        else
                        {
                            //Look for nearest unlooted body
                            Enemy closestEnemy = null;
                            float closestDistance = float.MaxValue;
                            for (int i = 0; i < wave.enemiesOnScreen.Count; i++)
                            {
                                if (!wave.enemiesOnScreen[i].Alive && !wave.enemiesOnScreen[i].isLooted)
                                {
                                    float enemyDistance = Vector2.Distance(Position, wave.enemiesOnScreen[i].Position);
                                    if (enemyDistance < closestDistance)
                                    {
                                        closestEnemy = wave.enemiesOnScreen[i];
                                        closestDistance = enemyDistance;
                                    }
                                }
                            }
                            Scavenger closestScavenger = null;
                            float closestScavengerDistance = float.MaxValue;
                            for (int i = 0; i < lootableScavengers.Count; i++)
                            {
                                float scavengerDistance = Vector2.Distance(Position, lootableScavengers[i].Position);
                                if (scavengerDistance < closestScavengerDistance)
                                {
                                    closestScavenger = lootableScavengers[i];
                                    closestScavengerDistance = scavengerDistance;
                                }
                            }
                            float myR = Width + Position.X;
                            float myL = Position.X;
                            float enemyR = 0;
                            float enemyL = 0;
                            bool isScavenger = false;

                            if ((closestEnemy == null && closestScavenger != null)
                                || (closestEnemy != null && closestScavenger != null && closestScavengerDistance <= closestDistance))
                            {
                                enemyR = closestScavenger.Width + closestScavenger.Position.X;
                                enemyL = closestScavenger.Position.X;
                                isScavenger = true;
                            }
                            else if ((closestEnemy != null && closestScavenger == null)
                                || (closestEnemy != null && closestScavenger != null && closestScavengerDistance > closestDistance))
                            {
                                enemyR = closestEnemy.Width + closestEnemy.Position.X;
                                enemyL = closestEnemy.Position.X;
                                isScavenger = false;
                            }

                            if (enemyR != 0 && enemyL != 0)
                            {
                                //If it is close enough to scavenge, do so
                                if ((myR >= enemyL && myR <= enemyR) || (myL >= enemyL && myL <= enemyR))
                                {
                                    if (isScavenger)
                                    {
                                        closestScavenger.isLooted = true;
                                        lootToLoot = new List<Loot>();
                                        lootToLoot.AddRange(closestScavenger.scavengedLoot);
                                    }
                                    else
                                    {
                                        closestEnemy.isLooted = true;
                                        lootToLoot = new List<Loot>();
                                        lootToLoot.Add(closestEnemy.loot);
                                    }
                                    action = 3;
                                    whenScavengeBegan = gameTime.TotalGameTime.TotalMilliseconds;
                                    actionToReturnTo = 1;
                                }
                                else //else move towards it
                                {
                                    if (enemyL > myL) //Move right
                                    {
                                        Position = scavengerManager.waveManager.getWave().layout.pather.Move(Position, false, speed);
                                        activeTexture.Update();
                                    }
                                    else //Move left 
                                    {
                                        Position = scavengerManager.waveManager.getWave().layout.pather.Move(Position, true, speed);
                                        reverseTexture.Update();
                                    }
                                }
                            }
                            else //Shouldn't happen
                            {
                                Position = scavengerManager.waveManager.getWave().layout.pather.Move(Position, false, speed);
                                activeTexture.Update();
                            }
                        }
                    }
                }
                else if (action == 2) //Coming back
                {
                    if (command == 1)
                    {
                        Position = scavengerManager.waveManager.getWave().layout.pather.Move(Position, false, speed);
                        activeTexture.Update();
                        action = 1;
                    }
                    else
                    {
                        if (scavengerManager.waveManager.getWave().layout.pather.atTrenchEntrance(Position, Width, Height))
                        {
                            Position = scavengerIdle;
                            action = 0;
                            MainGame.scavengerAddToSupply(this);
                        }
                        else
                        {
                            Position = scavengerManager.waveManager.getWave().layout.pather.Move(Position, true, speed);
                            reverseTexture.Update();
                        }
                    }
                }
                else //Scavenging from body
                {
                    if (whenScavengeBegan != 0 && gameTime.TotalGameTime.TotalMilliseconds - whenScavengeBegan > timeToScavenge)
                    {
                        action = actionToReturnTo;
                        scavengedLoot.AddRange(lootToLoot);
                        lootToLoot.Clear();
                    }
                    else
                    {
                        if (command == 0) actionToReturnTo = 2;
                        else if (command == 1) actionToReturnTo = 1;
                    }
                }
            }
        }

        public void addLootToSupply(SniperRifle sr, MachineGun mg, Player player)
        {
            for (int i = 0; i < scavengedLoot.Count; i++)
            {
                scavengedLoot[i].addLoot(sr, mg, player);
            }
            scavengedLoot.Clear();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Alive)
            {
                if (action == 0) spriteBatch.Draw(idleTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                else if (action == 1) activeTexture.Draw(spriteBatch, Position, 1f);
                else if (action == 2) reverseTexture.Draw(spriteBatch, Position, 1f);
                else
                {
                    spriteBatch.Draw(scavengingTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    lootToLoot[0].Draw(spriteBatch, new Vector2(Position.X, Position.Y - lootToLoot[0].texture.Height - lootBuffer));
                }
            }
            else
            {
                if (action == 0) {
                    spriteBatch.Draw(trenchDeathTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }
                else {
                    spriteBatch.Draw(deathTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }
            } 

            if (Active)
            {
                Texture2D textureToDraw = notSentHudTexture;
                if (action == 1) textureToDraw = sentOutHudTexture;
                else if (action == 2) textureToDraw = sentBackHudTexture;
                else if (action == 3)
                {
                    if (actionToReturnTo == 1) textureToDraw = sentOutHudTexture;
                    else if (actionToReturnTo == 2) textureToDraw = sentBackHudTexture;
                }
                spriteBatch.Draw(textureToDraw, scavengerManager.waveManager.getWave().layout.getHUDPlacement(hudSeat), 
                    null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }

        public void returnToTrench()
        {
            Position = scavengerManager.waveManager.getWave().layout.getScavengerTrenchPlacement(positionNumber);
            action = 0; 
            MainGame.scavengerAddToSupply(this);
        }
    }
}
