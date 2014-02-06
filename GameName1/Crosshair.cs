using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace GameName1
{
    class Crosshair
    {

        public Texture2D CrosshairTexture;
        public Vector2 Position;
        public int State; 
        //0 means offscreen, listening for aiming sequence
        //1 means currently being fired, listening for firing sequence 
        

        public int Width
        {
            get { return CrosshairTexture.Width; }
        }

        public int Height
        {
            get { return CrosshairTexture.Height; }
        }

        public void Initialize(ContentManager Content)
        {
            CrosshairTexture = Content.Load<Texture2D>("Graphics\\Crosshair");
            Position = new Vector2(-1*CrosshairTexture.Width, -1*CrosshairTexture.Height);
            State = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(CrosshairTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        public void resetPosition()
        {
            Position.X = -1 * CrosshairTexture.Width;
            Position.Y = -1 * CrosshairTexture.Height;
        }

    }
}
