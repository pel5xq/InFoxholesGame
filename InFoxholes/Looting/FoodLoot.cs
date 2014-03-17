using InFoxholes.Friendlies;
using InFoxholes.Weapons;
using Microsoft.Xna.Framework.Content;

namespace InFoxholes.Looting
{
     public class FoodLoot : Loot
        {
         public FoodLoot(int lootAmount, ContentManager Content)
             : base(lootAmount, Content)
            {
                texture = Player.foodTexture;
            }
            override public void addLoot(SniperRifle sniper, MachineGun mg, Player player)
            {
                player.foodSupply = player.foodSupply + amount;
            }
        }
}
