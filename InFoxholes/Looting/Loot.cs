using InFoxholes.Friendlies;
using InFoxholes.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace InFoxholes.Looting
{
    public class Loot
    {
        public int amount;
        public Texture2D texture;

        /* Magic Numbers */
        int drawBuffer = 2;
        float drawAlpha = .6f;

        public Loot(int lootAmount, ContentManager content)
        {
            amount = lootAmount;
        }
        virtual public void addLoot(SniperRifle sniper, MachineGun mg, Player player) { }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            for (int i = 0; i < amount; i++)
            {
                spriteBatch.Draw(texture, new Vector2(position.X + i*(texture.Width + drawBuffer), position.Y), 
                    null, Color.White * drawAlpha, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }
    }
}
