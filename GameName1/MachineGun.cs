using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameName1
{
    class MachineGun : Weapon
    {
        /* MagicNumbers */
        int clipsize = 20;
        int millicooldown = 50;

        override public float GetCrosshairVelocity(double timeElapsed)
        {
            return 15f;
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
            spriteBatch.Draw(WeaponTexture, hudPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            base.DrawHUD(spriteBatch, gameTime);
        }
    }
}

