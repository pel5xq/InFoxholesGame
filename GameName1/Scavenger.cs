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
    class Scavenger : Targetable
    {

        public bool Alive;
        public Vector2 Position;
        public float speed;
        public Texture2D idleTexture;
        public Texture2D deathTexture;
        public Texture2D scavengingTexture;
        public AnimatedSprite activeTexture;
        public AnimatedSprite reverseTexture;
        int action; //0 means unsent, 1 means sent out, 2 means sent back, 3 means actively scavenging
        double whenScavengeBegan;
        int actionToReturnTo;

        /* Magic Numbers*/
        float speedValue = .8f;
        int animationSpeed = 10;
        int numMapRows = 1;
        int numMapColumns = 4;
        Vector2 scavengerSpawn = new Vector2(140, 180);
        Vector2 scavengerIdle = new Vector2(30, 300);
        int ladderDetectX = 122;
        double timeToScavenge = 3000;

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

        virtual public void Initialize(ContentManager content, Vector2 position)
        {
            idleTexture = content.Load<Texture2D>("Graphics\\TrooperIdle");
            deathTexture = content.Load<Texture2D>("Graphics\\TrooperDead");
            scavengingTexture = content.Load<Texture2D>("Graphics\\TrooperScavenging");
            activeTexture = new AnimatedSprite(content.Load<Texture2D>("Graphics\\Trooper"), numMapRows, numMapColumns, animationSpeed);
            reverseTexture = new AnimatedSprite(content.Load<Texture2D>("Graphics\\TrooperReverse"), numMapRows, numMapColumns, animationSpeed);
            speed = speedValue;
            Position = position;
            Alive = true;
            action = 0;
        }

        public void Update(int command, GameTime gameTime)
        {
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
                        if (Position.X <= ladderDetectX)
                        {
                            Position = scavengerIdle;
                            action = 0;
                        }
                        else 
                        {
                            Position = Pather.Move(Position, true, speed);
                            reverseTexture.Update();
                            action = 2;
                        }
                    }
                    else //else TODO look for nearest body to scavenge / check if can start to scavenge
                    {
                        Position = Pather.Move(Position, false, speed);
                        activeTexture.Update();
                    }
                }
                else if (action == 2) //Coming back
                {
                    if (command == 1)
                    {
                        Position = Pather.Move(Position, false, speed);
                        activeTexture.Update();
                        action = 1;
                    }
                    else
                    {
                        if (Position.X <= ladderDetectX)
                        {
                            Position = scavengerIdle;
                            action = 0;
                        }
                        else
                        {
                            Position = Pather.Move(Position, true, speed);
                            reverseTexture.Update();
                        }
                    }
                }
                else //Scavenging from body
                {
                    if (gameTime.TotalGameTime.TotalMilliseconds - whenScavengeBegan > timeToScavenge)
                    {
                        action = actionToReturnTo;
                    }
                    else
                    {
                        if (command != -1) actionToReturnTo = command;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Alive) 
            { 
                if (action == 0) spriteBatch.Draw(idleTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                else if (action == 1) activeTexture.Draw(spriteBatch, Position, 1f);
                else if (action == 2) reverseTexture.Draw(spriteBatch, Position, 1f);
                else spriteBatch.Draw(scavengingTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
            else spriteBatch.Draw(deathTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
