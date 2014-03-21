using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using InFoxholes.Waves;

namespace InFoxholes.Weapons
{
    public class MachineGun : Weapon
    {
        /* MagicNumbers */
        float disabledAlpha = .5f;
        int clipsize = 20;
        int millicooldown = 50;
        int reloadLength = 5500;

        override public float GetCrosshairVelocity(double timeElapsed)
        {
            return 15f;
        }
        override public void Initialize(ContentManager Content, SpriteBatch spriteBatch, int HUDPosition, int ammosupply, WaveManager manager)
        {
            milliCooldown = millicooldown;
            clipSize = clipsize;
            clipSupply = clipsize;
            reloadCooldown = reloadLength;
            WeaponTexture = Content.Load<Texture2D>("Graphics\\BAR");
            bullet = Content.Load<Texture2D>("Graphics\\BARAmmo");
            base.Initialize(Content, spriteBatch, HUDPosition, ammosupply, manager);
        }
        override public void DrawHUD(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Vector2 hudPosition = waveManager.getWave().layout.getHUDPlacement(hudSeat);
            if (reloadOver(gameTime) && clipSupply > 0) spriteBatch.Draw(WeaponTexture, hudPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            else spriteBatch.Draw(WeaponTexture, hudPosition, null, Color.White * disabledAlpha, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            base.DrawHUD(spriteBatch, gameTime);
        }
    }
}

