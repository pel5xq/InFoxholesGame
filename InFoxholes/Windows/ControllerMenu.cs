using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using InFoxholes.Util;

namespace InFoxholes.Windows
{
    public class ControllerMenu
     {
        public Texture2D menuTexture;
        public Texture2D controllerMenuTexture;
        public Texture2D pixel;
        bool hoverFlag = false;
        bool controllerFlag = false;
        public Button backButton;

        /* Magic Numbers */
        static int startButtonLX = 345;
        static int startButtonLY = 415;
        static int startButtonRX = 470;
        static int startButtonRY = 465;
                

        public ControllerMenu(ContentManager Content, SpriteBatch spriteBatch)
        {
            menuTexture = Content.Load<Texture2D>("Graphics\\Menu");
            controllerMenuTexture = Content.Load<Texture2D>("Graphics\\ControllerMenu");
            backButton = new Button(new Vector2(startButtonLX, startButtonLY), 
                new Vector2(startButtonRX, startButtonRY), "", Vector2.Zero);
            pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White });
        }
        public void Update(GameTime gameTime, GamePadState gamepadState)
        {
            if (gamepadState.IsConnected)
            {
                hoverFlag = true;
                controllerFlag = true;
                if (gamepadState.Buttons.A == ButtonState.Pressed && MainGame.previousGamepadState.Buttons.A != ButtonState.Pressed)
                {
                    Menu.confirmClickEffectInstance.Play();
                    Menu.isInControllerMenu = false;
                }
            }
            else
            {
                controllerFlag = false;
                if (backButton.mouseIsOverButton(Mouse.GetState().X, Mouse.GetState().Y))
                {
                    if (hoverFlag == false) Menu.scrollClickEffectInstance.Play();
                    hoverFlag = true;
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed && MainGame.previousMouseState.LeftButton != ButtonState.Pressed)
                    {
                        Menu.confirmClickEffectInstance.Play();
                        Menu.isInControllerMenu = false;
                    }
                }
                else
                {
                    hoverFlag = false;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (controllerFlag) spriteBatch.Draw(controllerMenuTexture, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            else spriteBatch.Draw(menuTexture, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            backButton.Draw(spriteBatch, pixel, hoverFlag, Color.Black);
        }
    }
}
