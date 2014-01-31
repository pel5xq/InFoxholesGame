using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameName1
{
    /**
     * Adapted from R.B.Whitaker's Monogame Tutorial 
     * http://rbwhitaker.wikidot.com/monogame-texture-atlases-2 
     */
    public class AnimatedSprite
    {
        public Texture2D Texture { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        private int currentFrame;
        private int totalFrames;
        private int animationSpeed;
        private int animationCounter;

        public AnimatedSprite(Texture2D texture, int rows, int columns, int animationspeed)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = Rows * Columns;
            animationSpeed = animationspeed;
            animationCounter = 0;
        }

        public void Update()
        {
            animationCounter++;
            if (animationCounter > animationSpeed)
            {
                animationCounter = 0;
                currentFrame++;
                if (currentFrame == totalFrames)
                    currentFrame = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location, float alpha)
        {
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White * alpha);
        }
    }
}
