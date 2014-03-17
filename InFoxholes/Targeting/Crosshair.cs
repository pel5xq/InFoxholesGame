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

using InFoxholes;
using InFoxholes.Enemies;
using InFoxholes.Friendlies;
using InFoxholes.Looting;
using InFoxholes.Targeting;
using InFoxholes.Util;
using InFoxholes.Waves;
using InFoxholes.Weapons;
using InFoxholes.Windows;

namespace InFoxholes.Targeting
{
    public class Crosshair
    {

        public Texture2D CrosshairTexture;
        public Vector2 Position;
        public int State; 
        //0 means offscreen, listening for aiming sequence
        //1 means currently being fired, listening for firing sequence 
        Vector2 aimingVector;
        TimeSpan aimingTimestamp;
        

        public int Width
        {
            get { return CrosshairTexture.Width; }
        }

        public int Height
        {
            get { return CrosshairTexture.Height; }
        }

        public void Initialize(ContentManager Content)
        {
            CrosshairTexture = Content.Load<Texture2D>("Graphics\\Crosshair");
            Position = new Vector2(-1*CrosshairTexture.Width, -1*CrosshairTexture.Height);
            State = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(CrosshairTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        public void resetPosition()
        {
            Position.X = -1 * CrosshairTexture.Width;
            Position.Y = -1 * CrosshairTexture.Height;
        }

        public void interruptAiming()
        {
            resetPosition();
            State = 0;
        }

        public void Update(MouseState currentMouseState, Weapon weapon, GameTime gameTime, Wave wave, Scavenger scavenger,
            GraphicsDevice graphicsDevice)
        {

            if (State == 0)
            {
                //If not firing, listen for aiming click
                //If clicked, place crosshair in front of gun
                //and switch to aiming state,
                //recording aiming vector
                //Also confirm that vector is going in logical direction
                if (currentMouseState.RightButton == ButtonState.Pressed)
                {
                    float aimX = weapon.GunPoint.X;
                    float aimY = weapon.GunPoint.Y - Height / 2;
                    aimingVector.X = currentMouseState.X - aimX;
                    aimingVector.Y = currentMouseState.Y - aimY;
                    aimingVector.Normalize();
                    if (aimingVector.X > 0)
                    {
                        aimingTimestamp = gameTime.TotalGameTime;
                        Position.X = aimX;
                        Position.Y = aimY;
                        State = 1;
                    }
                }

            }
            else
            {
                //If aiming, listen for firing click
                //If the aiming button is released, place it back offscreen
                //switch to not firing state
                //Otherwise, move crosshair along aiming vector
                //and if there is a firing clip, determine hit
                //and ammo changes or reload need


                if (currentMouseState.RightButton == ButtonState.Released)
                {
                    resetPosition();
                    State = 0;
                }
                else
                {
                    //Need to take firing cooldown/reload into consideration
                    if (weapon.isFireable(gameTime))
                    {
                        if (currentMouseState.LeftButton == ButtonState.Pressed) //implied state==1
                        {
                            //Update game world here and inform weapon to draw
                            //shot, but can't draw yet

                            if (wave.isHit(Position) || scavenger.isHit(Position))
                            {
                                weapon.ShotPoint.X = Position.X + Width / 2;
                                weapon.ShotPoint.Y = Position.Y + Height / 2;
                            }
                            else
                            {
                                weapon.ShotPoint.X = weapon.GunPoint.X + aimingVector.X * graphicsDevice.Viewport.Width;
                                weapon.ShotPoint.Y = weapon.GunPoint.Y + aimingVector.Y * graphicsDevice.Viewport.Width;
                            }
                            weapon.startShotCooldown(gameTime);
                        }
                    }
                    float velocity = weapon.GetCrosshairVelocity((gameTime.TotalGameTime.Subtract(aimingTimestamp)).TotalMilliseconds);
                    Position.X = aimingVector.X * velocity + Position.X;
                    Position.Y = aimingVector.Y * velocity + Position.Y;
                }

            }

        }

    }
}
