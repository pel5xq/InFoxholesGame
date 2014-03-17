using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace GameName1
{
    class Menu
    {
        public Texture2D menuTexture;
        Vector2 topleft;
        Vector2 botright;
        public Texture2D pixel;
        bool hoverFlag = false;

        /* Magic Numbers */
        static int startButtonLX = 345;
        static int startButtonLY = 415;
        static int startButtonRX = 470;
        static int startButtonRY = 465;
        int startButtonWidth = startButtonRX - startButtonLX;
        int startButtonHeight = startButtonRY - startButtonLY;
        private float lineThickness = 4f;
        private float halfPi = (float)(Math.PI / 2);
                

        public Menu(ContentManager Content, SpriteBatch spriteBatch)
        {
            menuTexture = Content.Load<Texture2D>("Graphics\\Menu");
            topleft = new Vector2(startButtonLX, startButtonLY);
            botright = new Vector2(startButtonRX, startButtonRY);
            pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White });
        }
        public void Update(GameTime gameTime)
        {
            if (overStartButton(Mouse.GetState().X, Mouse.GetState().Y)) {
                hoverFlag = true;
                 if(Mouse.GetState().LeftButton == ButtonState.Pressed)
                 {
                     MainGame.isInMenu = false;
                 }
            }
            else {
                hoverFlag = false;
            }
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(menuTexture, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            if (hoverFlag)
            {
                spriteBatch.Draw(pixel, topleft, null, Color.Black, 0, Vector2.Zero, new Vector2(125, lineThickness),
                    SpriteEffects.None, 0);
                spriteBatch.Draw(pixel, topleft, null, Color.Black, halfPi, Vector2.Zero, new Vector2(50, lineThickness),
                    SpriteEffects.None, 0);
                spriteBatch.Draw(pixel, botright, null, Color.Black, 2 * halfPi, Vector2.Zero, new Vector2(125, lineThickness),
                    SpriteEffects.None, 0);
                spriteBatch.Draw(pixel, botright, null, Color.Black, -1 * halfPi, Vector2.Zero, new Vector2(50, lineThickness),
                    SpriteEffects.None, 0);
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
    }
}
