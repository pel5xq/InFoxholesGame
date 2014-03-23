#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#endregion

using InFoxholes.Friendlies;
using InFoxholes.Targeting;
using InFoxholes.Waves;
using InFoxholes.Weapons;

namespace InFoxholes.Windows
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        static Player player;
        public static KeyboardState currentKeyboardState;
        public static KeyboardState previousKeyboardState;
        public static MouseState currentMouseState;
        public static MouseState previousMouseState;
        public static GamePadState currentGamepadState;
        public static GamePadState previousGamepadState;
        Crosshair crosshair;
        Weapon weapon;
        static SniperRifle sniperRifle;
        static MachineGun machineGun;
        WaveManager waveManager;
        ScavengerManager scavengerManager;
        static int currentScavengeCommand; //0 = come back, 1 = scavenge, -1 for no change
        public static bool gameOver;
        public static bool isInMenu;
        Menu menu;
        public static bool isInfiniteAmmoMode;
        public static bool isInfiniteFoodMode;
        public static SpriteFont font;

        //How to Write to Console: System.Diagnostics.Debug.WriteLine();

        /* Magic Numbers */
        private static int startingSniperAmmo = 10;
        private static int startingMachinegunAmmo = 50;
        private static int startingFood = 1;
        int windowWidth = 800;
        int windowHeight = 482;
        int numStartingLives = 4;
        public static float triggerThreshold = .8f;

        public MainGame()
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
            font = Content.Load<SpriteFont>("Fonts\\Font");
            player = new Player();
            crosshair = new Crosshair();
            sniperRifle = new SniperRifle();
            machineGun = new MachineGun();
            weapon = sniperRifle;
            sniperRifle.isSelected = true;
            machineGun.isSelected = false;
            waveManager = new WaveManager();
            scavengerManager = new ScavengerManager();
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
            menu = new Menu(Content, spriteBatch);
            waveManager.Initialize(Content);
            crosshair.Initialize(Content, waveManager);
            sniperRifle.Initialize(Content, spriteBatch, 0, startingSniperAmmo, waveManager);
            machineGun.Initialize(Content, spriteBatch, 1, startingMachinegunAmmo, waveManager);
            scavengerManager.Initialize(Content, 3, numStartingLives, waveManager);
            player.Initialize(Content, startingFood, 2, waveManager);

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
            previousKeyboardState = currentKeyboardState;
            previousMouseState = currentMouseState;
            currentKeyboardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();
            previousGamepadState = currentGamepadState;
            currentGamepadState = GamePad.GetState(PlayerIndex.One);

            if (currentGamepadState.Buttons.Back == ButtonState.Pressed || currentKeyboardState.IsKeyDown(Keys.Escape))
                Exit();

            if (isInMenu)
            {
                menu.Update(gameTime, currentGamepadState);
            }
            else
            {
                if (waveManager.State != 0 && (Keyboard.GetState().IsKeyDown(Keys.R) || currentGamepadState.Buttons.X == ButtonState.Pressed))
                {
                    weapon.reload(gameTime);
                }
                if (waveManager.State != 0 && (Keyboard.GetState().IsKeyDown(Keys.Q) && !previousKeyboardState.IsKeyDown(Keys.Q)
                    || (currentGamepadState.Buttons.Y == ButtonState.Pressed && !(previousGamepadState.Buttons.Y == ButtonState.Pressed))))
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
                    crosshair.interruptAiming();
                }
                int scavengeCommand = -1;
                if (waveManager.State != 0 && (Keyboard.GetState().IsKeyDown(Keys.W) && !previousKeyboardState.IsKeyDown(Keys.W)
                    || (currentGamepadState.Buttons.A == ButtonState.Pressed && !(previousGamepadState.Buttons.A == ButtonState.Pressed))))
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
                }
                crosshair.Update(currentMouseState, weapon, gameTime, waveManager.getWave(), scavengerManager.getActiveScavenger(), GraphicsDevice);
                waveManager.Update(gameTime, scavengerManager);
                scavengerManager.Update(scavengeCommand, gameTime, waveManager.getWave());

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
                    scavengerManager.Draw(spriteBatch);
                    waveManager.Draw(spriteBatch);
                }
                else
                {
                    waveManager.getWave().layout.DrawGameOver(spriteBatch);
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public static void scavengerAddToSupply(Scavenger scavenger) {
            scavenger.addLootToSupply(sniperRifle, machineGun, player);
        }

        public static void initializeAmmo()
        {
            sniperRifle.ammoSupply = startingSniperAmmo;
            sniperRifle.clipSupply = sniperRifle.clipSize;
            machineGun.ammoSupply = startingMachinegunAmmo;
            machineGun.clipSupply = machineGun.clipSize;
            player.foodSupply = startingFood;
        }

        public static void resetScavengeCommand()
        {
            currentScavengeCommand = 0;
        }
    }
}
