using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using InFoxholes.Util;


namespace InFoxholes.Windows
{
    public class Menu
    {
        public AnimatedSprite menuTexture;
        Button startButton;
        Button skipButton;
        Button controlsButton;
        List<Button> buttonList;
        List<bool> buttonListHover;
        public static Texture2D pixel;
        bool hoverFlag = false;
        bool hoverFlagControls = false;
        bool hoverFlagSkip = false;
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
        static int startButtonRX = 125;
        static int startButtonRY = 180;

        static int skipButtonLX = 30;
        static int skipButtonLY = 195;
        static int skipButtonRX = 250;
        static int skipButtonRY = 240;

        static int controlsButtonLX = 30;
        static int controlsButtonLY = 260;
        static int controlsButtonRX = 180;
        static int controlsButtonRY = 305;

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
            skipButton = new Button(new Vector2(skipButtonLX, skipButtonLY),
                new Vector2(skipButtonRX, skipButtonRY), "", Vector2.Zero);
            buttonList = new List<Button>() { startButton, skipButton, controlsButton };
            buttonListHover = new List<bool>() { hoverFlag, hoverFlagSkip, hoverFlagControls };
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
                if (MainGame.currentGamepadState.IsConnected)
                {
                    if (MainGame.currentGamepadState.DPad.Up == ButtonState.Pressed && MainGame.previousGamepadState.DPad.Up != ButtonState.Pressed ||
                        MainGame.currentGamepadState.DPad.Left == ButtonState.Pressed && MainGame.previousGamepadState.DPad.Left != ButtonState.Pressed ||
                        (MainGame.currentGamepadState.ThumbSticks.Left.Length() > thresholdLength && !(MainGame.previousGamepadState.ThumbSticks.Left.Length() > thresholdLength)
                        && MainGame.currentGamepadState.ThumbSticks.Left.Y > 0))
                    {
                        Menu.scrollClickEffectInstance.Play();
                        if (selectedButton == 0) selectedButton = buttonList.Count - 1;
                        else selectedButton--;
                    }
                    else if (MainGame.currentGamepadState.DPad.Down == ButtonState.Pressed && MainGame.previousGamepadState.DPad.Down != ButtonState.Pressed ||
                        MainGame.currentGamepadState.DPad.Right == ButtonState.Pressed && MainGame.previousGamepadState.DPad.Right != ButtonState.Pressed ||
                        (MainGame.currentGamepadState.ThumbSticks.Left.Length() > thresholdLength && !(MainGame.previousGamepadState.ThumbSticks.Left.Length() > thresholdLength)
                        && MainGame.currentGamepadState.ThumbSticks.Left.Y < 0))
                    {
                        Menu.scrollClickEffectInstance.Play();
                        if (selectedButton == buttonList.Count - 1) selectedButton = 0;
                        else selectedButton++;
                    }

                    for (int i = 0; i < buttonList.Count; i++)
                    {
                        if (i == selectedButton) buttonListHover[i] = true;
                        else buttonListHover[i] = false;
                    }

                    if (MainGame.currentGamepadState.Buttons.A == ButtonState.Pressed && MainGame.previousGamepadState.Buttons.A != ButtonState.Pressed)
                    {
                        Menu.confirmClickEffectInstance.Play();
                        if (selectedButton == 0)
                        {
                            MainGame.isInMenu = false;
                            MediaPlayer.Stop();
                            windEffectInstance.Stop();
                            flagEffectInstance.Stop();
                        }
                        else if (selectedButton == 1)
                        {
                            MainGame.isInMenu = false;
                            MainGame.skipTutorial = true;
                            MediaPlayer.Stop();
                            windEffectInstance.Stop();
                            flagEffectInstance.Stop();
                        }
                        else if (selectedButton == 2)
                        {
                            isInControllerMenu = true;
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
                    if (skipButton.mouseIsOverButton(Mouse.GetState().X, Mouse.GetState().Y))
                    {
                        if (hoverFlagSkip == false) scrollClickEffectInstance.Play();
                        hoverFlagSkip = true;
                        if (Mouse.GetState().LeftButton == ButtonState.Pressed && MainGame.previousMouseState.LeftButton != ButtonState.Pressed)
                        {
                            confirmClickEffectInstance.Play();
                            MainGame.isInMenu = false;
                            MainGame.skipTutorial = true;
                            MediaPlayer.Stop();
                            windEffectInstance.Stop();
                            flagEffectInstance.Stop();
                        }
                    }
                    else
                    {
                        hoverFlagSkip = false;
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

                if (MainGame.currentGamepadState.IsConnected)
                {
                    for (int i = 0; i < buttonList.Count; i++)
                        buttonList[i].Draw(spriteBatch, pixel, buttonListHover[i], Color.Black);
                }
                else
                {
                    startButton.Draw(spriteBatch, pixel, hoverFlag, Color.Black);
                    controlsButton.Draw(spriteBatch, pixel, hoverFlagControls, Color.Black);
                    skipButton.Draw(spriteBatch, pixel, hoverFlagSkip, Color.Black);
                }
            }
        }
    }
}
