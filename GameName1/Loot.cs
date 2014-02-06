using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1
{
    class Loot
    {
        public int amount;
        public Loot(int lootAmount)
        {
            amount = lootAmount;
        }
        virtual public void addLoot(SniperRifle sniper, MachineGun mg, Player player) { }

        //Add draw method for scavenger feedback?
    }
}
