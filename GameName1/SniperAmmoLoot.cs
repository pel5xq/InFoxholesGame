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
    public class SniperAmmoLoot : Loot
    {
        public SniperAmmoLoot(int lootAmount, ContentManager Content) : base(lootAmount, Content)
        {
            texture = Content.Load<Texture2D>("Graphics\\LeeEnfieldAmmo");
        }
        override public void addLoot(SniperRifle sniper, MachineGun mg, Player player) {
            sniper.ammoSupply = sniper.ammoSupply + amount;
        }
    }
}
