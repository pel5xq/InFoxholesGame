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
    public class MachineGunAmmoLoot : Loot
    {
        public MachineGunAmmoLoot(int lootAmount, ContentManager Content)
            : base(lootAmount, Content)
        {
            texture = Content.Load<Texture2D>("Graphics\\BARAmmo");
        }
        override public void addLoot(SniperRifle sniper, MachineGun mg, Player player)
        {
            mg.ammoSupply = mg.ammoSupply + amount;
        }
    }
}
