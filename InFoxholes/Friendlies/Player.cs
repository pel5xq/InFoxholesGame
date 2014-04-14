using InFoxholes.Windows;
using InFoxholes.Waves;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace InFoxholes.Friendlies
{
    public class Player
    {
        public Texture2D foodTexture;
        public int foodSupply;
        public WaveManager waveManager;
        public int hudSeat;

        /* Magic Numbers */
        int foodBuffer = 5;

        public void Initialize(ContentManager Content, int startingFood, int HUDPosition, WaveManager manager)
        {
            foodSupply = startingFood;
            hudSeat = HUDPosition;
            foodTexture = Content.Load<Texture2D>("Graphics\\Food");
            waveManager = manager;
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!MainGame.isInfiniteFoodMode)
            {
                spriteBatch.Draw(foodTexture, waveManager.getWave().layout.getHUDPlacement(hudSeat), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(MainGame.font, "x " + foodSupply, Vector2.Add(waveManager.getWave().layout.getHUDPlacement(hudSeat), 
                    new Vector2(foodBuffer + foodTexture.Width, 0)), Color.Black);
            }
        }

    }
}
