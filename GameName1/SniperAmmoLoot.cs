using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1
{
    class SniperAmmoLoot : Loot
    {
        public SniperAmmoLoot(int lootAmount) : base(lootAmount)
        {
        }
        override public void addLoot(SniperRifle sniper, MachineGun mg, Player player) {
            sniper.ammoSupply = sniper.ammoSupply + amount;
        }
    }
}
