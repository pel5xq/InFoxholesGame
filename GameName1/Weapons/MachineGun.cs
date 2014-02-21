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
        override public void Initialize(ContentManager Content, SpriteBatch spriteBatch, Vector2 gunPoint, Vector2 position, Vector2 HUDPosition, int ammosupply)
        {
            milliCooldown = millicooldown;
            clipSize = clipsize;
            clipSupply = clipsize;
            reloadCooldown = reloadLength;
            WeaponTexture = Content.Load<Texture2D>("Graphics\\BAR");
            bullet = Content.Load<Texture2D>("Graphics\\BARAmmo");
            base.Initialize(Content, spriteBatch, gunPoint, position, HUDPosition, ammosupply);
        }
        override public void DrawHUD(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (reloadOver(gameTime) && clipSupply > 0) spriteBatch.Draw(WeaponTexture, hudPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            else spriteBatch.Draw(WeaponTexture, hudPosition, null, Color.White * disabledAlpha, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            base.DrawHUD(spriteBatch, gameTime);
        }
    }
}

