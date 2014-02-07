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
