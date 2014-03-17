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

using InFoxholes;
using InFoxholes.Enemies;
using InFoxholes.Friendlies;
using InFoxholes.Looting;
using InFoxholes.Targeting;
using InFoxholes.Util;
using InFoxholes.Waves;
using InFoxholes.Weapons;
using InFoxholes.Windows;

namespace InFoxholes.Friendlies
{
    public class Player
    {

        public Texture2D PlayerTexture;
        public Vector2 Position;
        public Vector2 hudPosition;
        public static Texture2D foodTexture;
        public bool Active;
        public int foodSupply;

        /* Magic Numbers */
        int foodBuffer = 5;

        public int Width
        {
            get { return PlayerTexture.Width; }
        }

        public int Height
        {
            get { return PlayerTexture.Height; }
        }

        public void Initialize(ContentManager Content, Vector2 position, int startingFood, Vector2 HUDPosition)
        {
            PlayerTexture = Content.Load<Texture2D>("Graphics\\Trench");
            Position = position;
            Active = true;
            foodSupply = startingFood;
            hudPosition = HUDPosition;
            foodTexture = Content.Load<Texture2D>("Graphics\\Food");
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(PlayerTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            if (!MainGame.isInfiniteFoodMode)
            {
                spriteBatch.Draw(foodTexture, hudPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(MainGame.font, "x "+foodSupply, Vector2.Add(hudPosition, 
                    new Vector2(foodBuffer + foodTexture.Width, 0)), Color.Black);
            }
        }

    }
}
