using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameName1
{
    class SniperRifle : Weapon
    {

        /* MagicNumbers */
        float disabledAlpha = .5f;
        int clipsize = 4;
        int millicooldown = 1000;

        override public float GetCrosshairVelocity(double timeElapsed)
        {
            return  (float)(2 - 2.6 * timeElapsed + 1.7 * Math.Pow(timeElapsed, 2)) / 30000;
        }
        override public void Initialize(SpriteBatch spriteBatch, Vector2 gunPoint, Texture2D rifleBurst, 
            Texture2D texture, Vector2 position, Vector2 HUDPosition, int ammosupply)
        {
            milliCooldown = millicooldown;
            clipSize = clipsize;
            base.Initialize(spriteBatch, gunPoint, rifleBurst, texture, position, HUDPosition, ammosupply);
        }
        override public void DrawHUD(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (cooldownOver(gameTime)) spriteBatch.Draw(WeaponTexture, hudPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            else spriteBatch.Draw(WeaponTexture, hudPosition, null, Color.White * disabledAlpha, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            base.DrawHUD(spriteBatch, gameTime);
        }

    }
}
