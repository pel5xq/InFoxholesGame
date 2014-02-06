using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1
{
    class MachineGunAmmoLoot : Loot
    {
        public MachineGunAmmoLoot(int lootAmount)
            : base(lootAmount)
        {
        }
        override public void addLoot(SniperRifle sniper, MachineGun mg, Player player)
        {
            mg.ammoSupply = mg.ammoSupply + amount;
        }
    }
}
