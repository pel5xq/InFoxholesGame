using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using InFoxholes.Windows;

namespace InFoxholes.Util
{
    public class Button
    {
        public Vector2 topLeft;
        public Vector2 botRight;
        public String text;
        public int Width;
        public int Height;
        public Vector2 textOffset;

        /* Magic Numbers */
        private float lineThickness = 4f;
        private float halfPi = (float)(Math.PI / 2);

        public Button(Vector2 topleft, Vector2 botright, String innerText,Vector2 textoffset)
        {
            topLeft = topleft;
            botRight = botright;
            text = innerText;
            Width = (int) (botRight.X - topLeft.X);
            Height = (int)(botRight.Y - topLeft.Y);
            textOffset = textoffset;
        }

        public void Draw(SpriteBatch spriteBatch,Texture2D pixel, bool hoverFlag, Color color)
        {
            if (text != "") spriteBatch.DrawString(MainGame.font, text, Vector2.Add(topLeft, textOffset), color);
            if (hoverFlag)
            {
                spriteBatch.Draw(pixel, topLeft, null, color, 0, Vector2.Zero, new Vector2(Width, lineThickness),
                    SpriteEffects.None, 0);
                spriteBatch.Draw(pixel, topLeft, null, color, halfPi, Vector2.Zero, new Vector2(Height, lineThickness),
                    SpriteEffects.None, 0);
                spriteBatch.Draw(pixel, botRight, null, color, 2 * halfPi, Vector2.Zero, new Vector2(Width, lineThickness),
                    SpriteEffects.None, 0);
                spriteBatch.Draw(pixel, botRight, null, color, -1 * halfPi, Vector2.Zero, new Vector2(Height, lineThickness),
                    SpriteEffects.None, 0);
            }
        }

        public bool mouseIsOverButton(int mouseX, int mouseY)
        {
            if (mouseX <= botRight.X && mouseX >= topLeft.X
                && mouseY <= botRight.Y && mouseY >= topLeft.Y)
            {
                return true;
            }
            return false;
        }

    }
}
