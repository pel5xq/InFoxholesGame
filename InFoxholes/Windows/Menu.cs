using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using InFoxholes.Util;


namespace InFoxholes.Windows
{
    public class Menu
    {
        public AnimatedSprite menuTexture;
        Button startButton;
        Button controlsButton;
        public static Texture2D pixel;
        bool hoverFlag = false;
        bool hoverFlagControls = false;
        public static bool isInControllerMenu;
        public ControllerMenu controllerMenu;
        int selectedButton;
        public Song menuBgm;
        public static SoundEffectInstance windEffectInstance;
        public static SoundEffectInstance flagEffectInstance;
        public static SoundEffectInstance scrollClickEffectInstance;
        public static SoundEffectInstance confirmClickEffectInstance;

        /* Magic Numbers */
        static int startButtonLX = 30;
        static int startButtonLY = 135;
        static int startButtonRX = 120;
        static int startButtonRY = 175;
        static int controlsButtonLX = 30;
        static int controlsButtonLY = 205;
        static int controlsButtonRX = 180;
        static int controlsButtonRY = 240;

        int animationSpeed = 30;
        float thresholdLength = .8f;
        float flagVolume = .7f;
        float windVolume = .7f;
        float clickVolume = 1.0f;
                

        public Menu(ContentManager Content, SpriteBatch spriteBatch)
        {
            menuTexture = new AnimatedSprite(Content.Load<Texture2D>("Graphics\\MainMenu"), 3, 2, animationSpeed);
            startButton = new Button(new Vector2(startButtonLX, startButtonLY), 
                new Vector2(startButtonRX, startButtonRY), "", Vector2.Zero);
            controlsButton = new Button(new Vector2(controlsButtonLX, controlsButtonLY),
                new Vector2(controlsButtonRX, controlsButtonRY), "", Vector2.Zero);
            pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White });
            isInControllerMenu = false;
            controllerMenu = new ControllerMenu(Content, spriteBatch);
            selectedButton = 0;
            menuBgm = Content.Load<Song>("Music\\Cylinder_Eight.wav");
            windEffectInstance = (Content.Load<SoundEffect>("Music\\Wind.wav")).CreateInstance();
            flagEffectInstance = (Content.Load<SoundEffect>("Music\\Flag.wav")).CreateInstance();
            scrollClickEffectInstance = (Content.Load<SoundEffect>("Music\\Click.wav")).CreateInstance();
            confirmClickEffectInstance = (Content.Load<SoundEffect>("Music\\Click2.wav")).CreateInstance();
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(menuBgm);
            windEffectInstance.Volume = windVolume;
            flagEffectInstance.Volume = flagVolume;
            scrollClickEffectInstance.Volume = clickVolume;
            confirmClickEffectInstance.Volume = clickVolume;
            windEffectInstance.Play();
            flagEffectInstance.Play();
        }
        public void Update(GameTime gameTime, GamePadState gamepadState)
        {
            if (isInControllerMenu)
            {
                controllerMenu.Update(gameTime, gamepadState);
            }
            else
            {
                if (gamepadState.IsConnected)
                {
                    if (selectedButton == 0)
                    {
                        hoverFlag = true;
                        hoverFlagControls = false;
                        if (gamepadState.Buttons.A == ButtonState.Pressed && MainGame.previousGamepadState.Buttons.A != ButtonState.Pressed)
                        {
                            confirmClickEffectInstance.Play();
                            MainGame.isInMenu = false;
                            MediaPlayer.Stop();
                            windEffectInstance.Stop();
                            flagEffectInstance.Stop();
                        }
                        if (gamepadState.DPad.Down == ButtonState.Pressed  && MainGame.previousGamepadState.DPad.Down != ButtonState.Pressed ||
                            gamepadState.DPad.Up == ButtonState.Pressed && MainGame.previousGamepadState.DPad.Up != ButtonState.Pressed ||
                            gamepadState.DPad.Left == ButtonState.Pressed && MainGame.previousGamepadState.DPad.Left != ButtonState.Pressed ||
                            gamepadState.DPad.Right == ButtonState.Pressed && MainGame.previousGamepadState.DPad.Right != ButtonState.Pressed ||
                            gamepadState.ThumbSticks.Left.Length() > thresholdLength && !(MainGame.previousGamepadState.ThumbSticks.Left.Length() > thresholdLength))
                        {
                            scrollClickEffectInstance.Play();
                            selectedButton = 1;
                        }
                    }
                    else if (selectedButton == 1)
                    {
                        hoverFlag = false;
                        hoverFlagControls = true;
                        if (gamepadState.Buttons.A == ButtonState.Pressed && MainGame.previousGamepadState.Buttons.A != ButtonState.Pressed)
                        {
                            confirmClickEffectInstance.Play();
                            isInControllerMenu = true;
                        }
                        if (gamepadState.DPad.Down == ButtonState.Pressed && MainGame.previousGamepadState.DPad.Down != ButtonState.Pressed ||
                            gamepadState.DPad.Up == ButtonState.Pressed && MainGame.previousGamepadState.DPad.Up != ButtonState.Pressed ||
                            gamepadState.DPad.Left == ButtonState.Pressed && MainGame.previousGamepadState.DPad.Left != ButtonState.Pressed ||
                            gamepadState.DPad.Right == ButtonState.Pressed && MainGame.previousGamepadState.DPad.Right != ButtonState.Pressed ||
                            gamepadState.ThumbSticks.Left.Length() > thresholdLength && !(MainGame.previousGamepadState.ThumbSticks.Left.Length() > thresholdLength))
                        {
                            scrollClickEffectInstance.Play();
                            selectedButton = 0;
                        }
                    }
                }
                else
                {
                    if (startButton.mouseIsOverButton(Mouse.GetState().X, Mouse.GetState().Y))
                    {
                        if (hoverFlag == false) scrollClickEffectInstance.Play();
                        hoverFlag = true;
                        if (Mouse.GetState().LeftButton == ButtonState.Pressed && MainGame.previousMouseState.LeftButton != ButtonState.Pressed)
                        {
                            confirmClickEffectInstance.Play();
                            MainGame.isInMenu = false;
                            MediaPlayer.Stop();
                            windEffectInstance.Stop();
                            flagEffectInstance.Stop();
                        }
                    }
                    else
                    {
                        hoverFlag = false;
                    }
                    if (controlsButton.mouseIsOverButton(Mouse.GetState().X, Mouse.GetState().Y))
                    {
                        if (hoverFlagControls == false) scrollClickEffectInstance.Play();
                        hoverFlagControls = true;
                        if (Mouse.GetState().LeftButton == ButtonState.Pressed && MainGame.previousMouseState.LeftButton != ButtonState.Pressed)
                        {
                            confirmClickEffectInstance.Play();
                            isInControllerMenu = true;
                        }
                    }
                    else
                    {
                        hoverFlagControls = false;
                    }
                }
                
            }
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (isInControllerMenu)
            {
                controllerMenu.Draw(spriteBatch, gameTime);
            }
            else
            {
                menuTexture.Update();
                menuTexture.Draw(spriteBatch, Vector2.Zero,  1f, SpriteEffects.None);
                startButton.Draw(spriteBatch, pixel, hoverFlag, Color.Black);
                controlsButton.Draw(spriteBatch, pixel, hoverFlagControls, Color.Black);
            }
        }
    }
}
