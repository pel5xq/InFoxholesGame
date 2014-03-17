using InFoxholes.Friendlies;
using InFoxholes.Weapons;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace InFoxholes.Looting
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
