#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace GameName1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;
        MouseState currentMouseState;
        MouseState previousMouseState;
        Crosshair crosshair;
        Vector2 aimingVector;
        TimeSpan aimingTimestamp;
        Weapon weapon;
        SniperRifle sniperRifle;
        MachineGun machineGun;
        Wave wave;
        double lastWeaponToggle;
        Scavenger scavenger;
        double lastScavengeCall;
        int currentScavengeCommand; //0 = come back, 1 = scavenge

        /* Magic Numbers */
        private int startingSniperAmmo = 10;
        private int startingMachinegunAmmo = 50;
        double weaponToggleCooldown = 100;
        double scavengeToggleCooldown = 350;
        private int trenchOffsetX = 272;
        private int trenchOffsetY = 120;
        private int gunOffsetX = 187;
        private int gunOffsetY = 115;
        private Vector2 firstHudPosition = new Vector2(10, 10);
        private Vector2 secondHudPosition = new Vector2(10, 50);
        private int enemySpawnXoffset = 100;
        private int enemySpawnYoffset = 200;
        private int scavengerSpawnXoffset = 140;
        private int scavengerSpawnYoffset = 180;
        private int scavengerIdleXoffset = 30;
        private int scavengerIdleYoffset = 300;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            player = new Player();
            crosshair = new Crosshair();
            sniperRifle = new SniperRifle();
            machineGun = new MachineGun();
            weapon = sniperRifle;
            sniperRifle.isSelected = true;
            machineGun.isSelected = false;
            wave = new Wave1();
            lastWeaponToggle = 0;
            scavenger = new Scavenger();
            lastScavengeCall = 0;
            currentScavengeCommand = 0;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            player.Initialize(Content.Load<Texture2D>("Graphics\\Trench"), playerPosition);
            crosshair.Initialize(Content.Load<Texture2D>("Graphics\\Crosshair"));
            sniperRifle.Initialize(spriteBatch, new Vector2(trenchOffsetX + playerPosition.X, trenchOffsetY + playerPosition.Y), Content.Load<Texture2D>("Graphics\\rifleBurst"),
                Content.Load<Texture2D>("Graphics\\LeeEnfield"), new Vector2(gunOffsetX + playerPosition.X, gunOffsetY + playerPosition.Y),
                firstHudPosition, startingSniperAmmo, Content.Load<Texture2D>("Graphics\\LeeEnfieldAmmo"));
            machineGun.Initialize(spriteBatch, new Vector2(trenchOffsetX + playerPosition.X, trenchOffsetY + playerPosition.Y), Content.Load<Texture2D>("Graphics\\rifleBurst"),
                Content.Load<Texture2D>("Graphics\\BAR"), new Vector2(gunOffsetX + playerPosition.X, gunOffsetY + playerPosition.Y),
                secondHudPosition, startingMachinegunAmmo, Content.Load<Texture2D>("Graphics\\BARAmmo"));
            scavenger.Initialize(Content, new Vector2(scavengerIdleXoffset, scavengerIdleYoffset));
            wave.Initialize(Content, new Vector2(GraphicsDevice.Viewport.Width - enemySpawnXoffset, GraphicsDevice.Viewport.Height - enemySpawnYoffset));

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                weapon.reload(gameTime);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Q) && gameTime.TotalGameTime.TotalMilliseconds - lastWeaponToggle > weaponToggleCooldown)
            {
                if (sniperRifle.isSelected)
                {
                    sniperRifle.isSelected = false;
                    machineGun.isSelected = true;
                    weapon = machineGun;
                }
                else
                {
                    sniperRifle.isSelected = true;
                    machineGun.isSelected = false;
                    weapon = sniperRifle;
                }
                lastWeaponToggle = gameTime.TotalGameTime.TotalMilliseconds;
            }
            int scavengeCommand = -1;
            if (Keyboard.GetState().IsKeyDown(Keys.W) && gameTime.TotalGameTime.TotalMilliseconds - lastScavengeCall > scavengeToggleCooldown)
            {
                if (currentScavengeCommand == 0)
                {
                    currentScavengeCommand = 1;
                    scavengeCommand = 1;
                }
                else
                {
                    currentScavengeCommand = 0;
                    scavengeCommand = 0;   
                }
                lastScavengeCall = gameTime.TotalGameTime.TotalMilliseconds;
            }
            previousKeyboardState = currentKeyboardState;
            previousMouseState = currentMouseState;
            currentKeyboardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();
            UpdateCrosshair(gameTime);
            scavenger.Update(scavengeCommand, gameTime);
            wave.Update(gameTime, scavenger);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            player.Draw(spriteBatch);
            crosshair.Draw(spriteBatch);
            weapon.Draw(spriteBatch);
            sniperRifle.DrawHUD(spriteBatch, gameTime);
            machineGun.DrawHUD(spriteBatch, gameTime);
            scavenger.Draw(spriteBatch);
            wave.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void UpdateCrosshair(GameTime gameTime)
        {
            if (crosshair.State == 0)
            { 
                //If not firing, listen for aiming click
                //If clicked, place crosshair in front of gun
                //and switch to aiming state,
                //recording aiming vector
                //Also confirm that vector is going in logical direction
                if (currentMouseState.RightButton == ButtonState.Pressed)
                {
                    float aimX = weapon.GunPoint.X;
                    float aimY = weapon.GunPoint.Y - crosshair.Height / 2;
                    aimingVector.X = currentMouseState.X - aimX;
                    aimingVector.Y = currentMouseState.Y - aimY;
                    aimingVector.Normalize();
                    if (aimingVector.X > 0) {
                        aimingTimestamp = gameTime.TotalGameTime;
                        crosshair.Position.X = aimX;
                        crosshair.Position.Y = aimY;
                        crosshair.State = 1;
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
                    crosshair.resetPosition();
                    crosshair.State = 0;
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

                            if (wave.isHit(crosshair.Position) || scavenger.isHit(crosshair.Position))
                            {
                                weapon.ShotPoint.X = crosshair.Position.X + crosshair.Width / 2;
                                weapon.ShotPoint.Y = crosshair.Position.Y + crosshair.Height / 2;
                            }
                            else
                            {
                                weapon.ShotPoint.X = weapon.GunPoint.X + aimingVector.X * GraphicsDevice.Viewport.Width;
                                weapon.ShotPoint.Y = weapon.GunPoint.Y + aimingVector.Y * GraphicsDevice.Viewport.Width;
                            }
                            weapon.startShotCooldown(gameTime);
                        }
                    }
                    float velocity = weapon.GetCrosshairVelocity((gameTime.TotalGameTime.Subtract(aimingTimestamp)).TotalMilliseconds);
                    crosshair.Position.X = aimingVector.X * velocity + crosshair.Position.X;
                    crosshair.Position.Y = aimingVector.Y * velocity + crosshair.Position.Y;
                }

            }

        }
    }
}
