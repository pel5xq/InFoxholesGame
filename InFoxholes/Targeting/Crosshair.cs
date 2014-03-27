using InFoxholes.Friendlies;
using InFoxholes.Waves;
using InFoxholes.Weapons;
using InFoxholes.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace InFoxholes.Targeting
{
    public class Crosshair
    {

        public Texture2D CrosshairTexture;
        public Texture2D aimHelpTexture;
        public Vector2 Position;
        public int State; 
        //0 means offscreen, listening for aiming sequence
        //1 means currently being fired, listening for firing sequence 
        Vector2 aimingVector;
        TimeSpan aimingTimestamp;
        public WaveManager waveManager;
        float aimingAngle;
        
        /* Magic Numbers */
        Vector2 aimHelpAdjust = new Vector2(0, -5);

        public int Width
        {
            get { return CrosshairTexture.Width; }
        }

        public int Height
        {
            get { return CrosshairTexture.Height; }
        }

        public void Initialize(ContentManager Content, WaveManager manager)
        {
            CrosshairTexture = Content.Load<Texture2D>("Graphics\\Crosshair");
            aimHelpTexture = Content.Load<Texture2D>("Graphics\\AimHelper");
            Position = new Vector2(-1*CrosshairTexture.Width, -1*CrosshairTexture.Height);
            State = 0;
            waveManager = manager;
            aimingAngle = -100;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(CrosshairTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            if (aimingAngle != -100) spriteBatch.Draw(aimHelpTexture, Vector2.Add(waveManager.getWave().layout.weaponGunpoint, aimHelpAdjust), null, 
                Color.White, aimingAngle, Vector2.Zero, 1f, SpriteEffects.None, 0f);
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
            if (waveManager.State != 0) 
            {
                if (State == 0) //Moving this check after this block creates cool redirect mechanic
                {
                    aimingAngle = -100;
                    float aimX = weapon.waveManager.getWave().layout.weaponGunpoint.X
                        + weapon.waveManager.getWave().layout.crosshairAdjustX;
                    float aimY = weapon.waveManager.getWave().layout.weaponGunpoint.Y
                        + weapon.waveManager.getWave().layout.crosshairAdjustY;

                    if (MainGame.currentGamepadState.IsConnected)
                    {
                        aimingVector.X = MainGame.currentGamepadState.ThumbSticks.Left.X;
                        aimingVector.Y = -1 * MainGame.currentGamepadState.ThumbSticks.Left.Y;
                        aimingVector.Normalize();

                        aimingAngle = (float)Math.Atan2(aimingVector.Y, aimingVector.X);
                        if (!waveManager.getWave().layout.checkAimingVector(aimingVector))
                        {
                            aimingAngle = -100;
                        }
                    }
                    else
                    {
                        if (aimingAngle == -100)
                        {
                            aimingVector.X = currentMouseState.X - aimX;
                            aimingVector.Y = currentMouseState.Y - aimY;
                            aimingVector.Normalize();

                            aimingAngle = (float)Math.Atan2(aimingVector.Y, aimingVector.X);
                            if (!waveManager.getWave().layout.checkAimingVector(aimingVector))
                            {
                                aimingAngle = -100;
                            }
                        }
                    }

                
                    //If not firing, listen for aiming click
                    //If clicked, place crosshair in front of gun
                    //and switch to aiming state,
                    //recording aiming vector
                    //Also confirm that vector is going in logical direction
                    if (currentMouseState.RightButton == ButtonState.Pressed
                         || (MainGame.currentGamepadState.Buttons.LeftShoulder == ButtonState.Pressed))
                     // || (MainGame.currentGamepadState.Triggers.Left >= MainGame.triggerThreshold)) //is what we want
                    {

                        if (waveManager.getWave().layout.checkAimingVector(aimingVector))
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


                    if (currentMouseState.RightButton == ButtonState.Released
                         && !(MainGame.currentGamepadState.Buttons.LeftShoulder == ButtonState.Pressed))
                        //&& !(MainGame.currentGamepadState.Triggers.Left >= MainGame.triggerThreshold)) //is what we want
                    {
                        resetPosition();
                        State = 0;
                    }
                    else
                    {
                        //Need to take firing cooldown/reload into consideration
                        if (weapon.isFireable(gameTime))
                        {
                            if (currentMouseState.LeftButton == ButtonState.Pressed
                                || MainGame.currentGamepadState.Triggers.Right >= MainGame.triggerThreshold) //implied state==1
                            {

                                //Update game world here and inform weapon to draw
                                //shot, but can't draw yet
                                //Make scavengers in trench safe, but not others
                                if (wave.isHit(Position) || (scavenger.action != 0 && scavenger.isHit(Position)))
                                {
                                    weapon.ShotPoint.X = Position.X + Width / 2;
                                    weapon.ShotPoint.Y = Position.Y + Height / 2;
                                }
                                else
                                {
                                    weapon.ShotPoint.X = weapon.waveManager.getWave().layout.weaponGunpoint.X + aimingVector.X * graphicsDevice.Viewport.Width;
                                    weapon.ShotPoint.Y = weapon.waveManager.getWave().layout.weaponGunpoint.Y + aimingVector.Y * graphicsDevice.Viewport.Width;
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
}
