using InFoxholes.Friendlies;
using InFoxholes.Weapons;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace InFoxholes.Looting
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
