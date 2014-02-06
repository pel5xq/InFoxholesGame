using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1
{
     class FoodLoot : Loot
        {
            public FoodLoot(int lootAmount)
                : base(lootAmount)
            {
            }
            override public void addLoot(SniperRifle sniper, MachineGun mg, Player player)
            {
                player.foodSupply = player.foodSupply + amount;
            }
        }
}
