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
        Vector2 topleft;
        Vector2 botright;
        Vector2 topleftControls;
        Vector2 botrightControls;
        public Texture2D pixel;
        bool hoverFlag = false;
        bool hoverFlagControls = false;
        public static bool isInControllerMenu;
        public ControllerMenu controllerMenu;
        int selectedButton;
        public Song menuBgm;
        public SoundEffect windEffect;
        public SoundEffect flagEffect;
        public SoundEffectInstance windEffectInstance;
        public SoundEffectInstance flagEffectInstance;

        /* Magic Numbers */
        static int startButtonLX = 30;
        static int startButtonLY = 135;
        static int startButtonRX = 120;
        static int startButtonRY = 175;
        static int controlsButtonLX = 30;
        static int controlsButtonLY = 205;
        static int controlsButtonRX = 180;
        static int controlsButtonRY = 240;
        int startButtonWidth = startButtonRX - startButtonLX;
        int startButtonHeight = startButtonRY - startButtonLY;
        int controlsButtonWidth = controlsButtonRX - controlsButtonLX;
        int controlsButtonHeight = controlsButtonRY - controlsButtonLY;
        private float lineThickness = 4f;
        private float halfPi = (float)(Math.PI / 2);
        int animationSpeed = 30;
        float thresholdLength = .8f;
        float flagVolume = .7f;
        float windVolume = .7f;
                

        public Menu(ContentManager Content, SpriteBatch spriteBatch)
        {
            menuTexture = new AnimatedSprite(Content.Load<Texture2D>("Graphics\\MainMenu"), 3, 2, animationSpeed);
            topleft = new Vector2(startButtonLX, startButtonLY);
            botright = new Vector2(startButtonRX, startButtonRY);
            topleftControls = new Vector2(controlsButtonLX, controlsButtonLY);
            botrightControls = new Vector2(controlsButtonRX, controlsButtonRY);
            pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White });
            isInControllerMenu = false;
            controllerMenu = new ControllerMenu(Content, spriteBatch);
            selectedButton = 0;
            menuBgm = Content.Load<Song>("Music\\Cylinder_Eight.wav");
            windEffect = Content.Load<SoundEffect>("Music\\Wind.wav");
            flagEffect = Content.Load<SoundEffect>("Music\\Flag.wav");
            windEffectInstance = windEffect.CreateInstance();
            flagEffectInstance = flagEffect.CreateInstance();
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(menuBgm);
            windEffectInstance.Volume = windVolume;
            flagEffectInstance.Volume = flagVolume;
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
                            selectedButton = 1;
                        }
                    }
                    else if (selectedButton == 1)
                    {
                        hoverFlag = false;
                        hoverFlagControls = true;
                        if (gamepadState.Buttons.A == ButtonState.Pressed && MainGame.previousGamepadState.Buttons.A != ButtonState.Pressed)
                        {
                            isInControllerMenu = true;
                        }
                        if (gamepadState.DPad.Down == ButtonState.Pressed && MainGame.previousGamepadState.DPad.Down != ButtonState.Pressed ||
                            gamepadState.DPad.Up == ButtonState.Pressed && MainGame.previousGamepadState.DPad.Up != ButtonState.Pressed ||
                            gamepadState.DPad.Left == ButtonState.Pressed && MainGame.previousGamepadState.DPad.Left != ButtonState.Pressed ||
                            gamepadState.DPad.Right == ButtonState.Pressed && MainGame.previousGamepadState.DPad.Right != ButtonState.Pressed ||
                            gamepadState.ThumbSticks.Left.Length() > thresholdLength && !(MainGame.previousGamepadState.ThumbSticks.Left.Length() > thresholdLength))
                        {
                            selectedButton = 0;
                        }
                    }
                }
                else
                {
                    if (overStartButton(Mouse.GetState().X, Mouse.GetState().Y))
                    {
                        hoverFlag = true;
                        if (Mouse.GetState().LeftButton == ButtonState.Pressed && MainGame.previousMouseState.LeftButton != ButtonState.Pressed)
                        {
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
                    if (overControlsButton(Mouse.GetState().X, Mouse.GetState().Y))
                    {
                        hoverFlagControls = true;
                        if (Mouse.GetState().LeftButton == ButtonState.Pressed && MainGame.previousMouseState.LeftButton != ButtonState.Pressed)
                        {
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
                if (hoverFlag)
                {
                    spriteBatch.Draw(pixel, topleft, null, Color.Black, 0, Vector2.Zero, new Vector2(startButtonWidth, lineThickness),
                        SpriteEffects.None, 0);
                    spriteBatch.Draw(pixel, topleft, null, Color.Black, halfPi, Vector2.Zero, new Vector2(startButtonHeight, lineThickness),
                        SpriteEffects.None, 0);
                    spriteBatch.Draw(pixel, botright, null, Color.Black, 2 * halfPi, Vector2.Zero, new Vector2(startButtonWidth, lineThickness),
                        SpriteEffects.None, 0);
                    spriteBatch.Draw(pixel, botright, null, Color.Black, -1 * halfPi, Vector2.Zero, new Vector2(startButtonHeight, lineThickness),
                        SpriteEffects.None, 0);
                }
                if (hoverFlagControls)
                {
                    spriteBatch.Draw(pixel, topleftControls, null, Color.Black, 0, Vector2.Zero, new Vector2(controlsButtonWidth, lineThickness),
                        SpriteEffects.None, 0);
                    spriteBatch.Draw(pixel, topleftControls, null, Color.Black, halfPi, Vector2.Zero, new Vector2(controlsButtonHeight, lineThickness),
                        SpriteEffects.None, 0);
                    spriteBatch.Draw(pixel, botrightControls, null, Color.Black, 2 * halfPi, Vector2.Zero, new Vector2(controlsButtonWidth, lineThickness),
                        SpriteEffects.None, 0);
                    spriteBatch.Draw(pixel, botrightControls, null, Color.Black, -1 * halfPi, Vector2.Zero, new Vector2(controlsButtonHeight, lineThickness),
                        SpriteEffects.None, 0);
                }
            }
        }
        private bool overStartButton(int mouseX, int mouseY)
        {
            if (mouseX <= startButtonRX && mouseX >= startButtonLX
                && mouseY <= startButtonRY && mouseY >= startButtonLY)
            {
                return true;
            }
            return false;
        }
        private bool overControlsButton(int mouseX, int mouseY)
        {
            if (mouseX <= controlsButtonRX && mouseX >= controlsButtonLX
                && mouseY <= controlsButtonRY && mouseY >= controlsButtonLY)
            {
                return true;
            }
            return false;
        }
    }
}
