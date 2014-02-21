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
        static Player player;
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;
        MouseState currentMouseState;
        MouseState previousMouseState;
        Crosshair crosshair;
        Weapon weapon;
        static SniperRifle sniperRifle;
        static MachineGun machineGun;
        WaveManager waveManager;
        double lastWeaponToggle;
        Scavenger scavenger;
        double lastScavengeCall;
        int currentScavengeCommand; //0 = come back, 1 = scavenge, -1 for no change
        public static bool gameOver;
        public Texture2D gameOverTexture;
        public Vector2 playerPosition;
        public static bool isInMenu;
        Menu menu;
        public static bool isInfiniteAmmoMode;
        public static bool isInfiniteFoodMode;

        /* Magic Numbers */
        private int startingSniperAmmo = 10;
        private int startingMachinegunAmmo = 50;
        private int startingFood = 1;
        double weaponToggleCooldown = 100;
        double scavengeToggleCooldown = 350;
        private int trenchOffsetX = 272;
        private int trenchOffsetY = 120;
        private int gunOffsetX = 187;
        private int gunOffsetY = 115;
        private Vector2 firstHudPosition = new Vector2(10, 10);
        private Vector2 secondHudPosition = new Vector2(10, 50);
        private Vector2 thirdHudPosition = new Vector2(10, 100);
        private Vector2 fourthHudPosition = new Vector2(10, 150);
        private int enemySpawnXoffset = 100;
        private int enemySpawnYoffset = 200;
        Vector2 scavengerSpawn = new Vector2(140, 180);
        Vector2 scavengerIdle = new Vector2(30, 300);
        Vector2 gameOverPositionOffset = new Vector2(0, 225);
        int windowWidth = 800;
        int windowHeight = 482;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            gameOver = false;
            isInMenu = true;
            isInfiniteAmmoMode = false;
            isInfiniteFoodMode = false;
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = windowWidth;
            graphics.PreferredBackBufferHeight = windowHeight;
            graphics.ApplyChanges();
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
            waveManager = new WaveManager();
            lastWeaponToggle = 0;
            scavenger = new Scavenger();
            lastScavengeCall = 0;
            currentScavengeCommand = 0;
            gameOverTexture = Content.Load<Texture2D>("Graphics\\TrenchGameOver");
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            menu = new Menu(Content, spriteBatch);
            playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            player.Initialize(Content, playerPosition, startingFood, thirdHudPosition);
            crosshair.Initialize(Content);
            sniperRifle.Initialize(Content, spriteBatch, new Vector2(trenchOffsetX + playerPosition.X, trenchOffsetY + playerPosition.Y),
                new Vector2(gunOffsetX + playerPosition.X, gunOffsetY + playerPosition.Y), firstHudPosition, startingSniperAmmo);
            machineGun.Initialize(Content, spriteBatch, new Vector2(trenchOffsetX + playerPosition.X, trenchOffsetY + playerPosition.Y),
                new Vector2(gunOffsetX + playerPosition.X, gunOffsetY + playerPosition.Y), secondHudPosition, startingMachinegunAmmo);
            scavenger.Initialize(Content, scavengerIdle, scavengerSpawn, fourthHudPosition);
            waveManager.Initialize(Content, new Vector2(GraphicsDevice.Viewport.Width - enemySpawnXoffset, GraphicsDevice.Viewport.Height - enemySpawnYoffset));

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

            if (isInMenu)
            {
                menu.Update(gameTime);
            }
            else
            {
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
                crosshair.Update(currentMouseState, weapon, gameTime, waveManager.getWave(), scavenger, GraphicsDevice);
                waveManager.Update(gameTime, scavenger);
                scavenger.Update(scavengeCommand, gameTime, waveManager.getWave());

            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();

            if (isInMenu)
            {
                menu.Draw(spriteBatch, gameTime);
            }
            else
            {
                if (!gameOver)
                {
                    player.Draw(spriteBatch);
                    crosshair.Draw(spriteBatch);
                    weapon.Draw(spriteBatch);
                    sniperRifle.DrawHUD(spriteBatch, gameTime);
                    machineGun.DrawHUD(spriteBatch, gameTime);
                    scavenger.Draw(spriteBatch);
                    waveManager.Draw(spriteBatch);
                }
                else
                {
                    spriteBatch.Draw(gameOverTexture, Vector2.Subtract(playerPosition, gameOverPositionOffset), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public static void scavengerAddToSupply(Scavenger scavenger) {
            scavenger.addLootToSupply(sniperRifle, machineGun, player);
        }
    }
}
