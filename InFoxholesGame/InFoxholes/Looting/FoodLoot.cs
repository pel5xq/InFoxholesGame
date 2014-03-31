using InFoxholes.Friendlies;
using InFoxholes.Weapons;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace InFoxholes.Looting
{
     public class FoodLoot : Loot
        {
         public FoodLoot(int lootAmount, ContentManager Content)
             : base(lootAmount, Content)
            {
                texture = Content.Load<Texture2D>("Graphics\\Food");
            }
            override public void addLoot(SniperRifle sniper, MachineGun mg, Player player)
            {
                player.foodSupply = player.foodSupply + amount;
            }
        }
}
