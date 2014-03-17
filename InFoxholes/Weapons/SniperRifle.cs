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

using InFoxholes;
using InFoxholes.Enemies;
using InFoxholes.Friendlies;
using InFoxholes.Looting;
using InFoxholes.Targeting;
using InFoxholes.Util;
using InFoxholes.Waves;
using InFoxholes.Weapons;
using InFoxholes.Windows;

namespace InFoxholes.Weapons
{
    public class SniperRifle : Weapon
    {

        /* MagicNumbers */
        float disabledAlpha = .5f;
        int clipsize = 4;
        int millicooldown = 1000;
        int reloadLength = 3500;

        override public float GetCrosshairVelocity(double timeElapsed)
        {
            return  (float)(2 - 2.6 * timeElapsed + 1.7 * Math.Pow(timeElapsed, 2)) / 30000;
        }
        override public void Initialize(ContentManager Content, SpriteBatch spriteBatch, Vector2 gunPoint, Vector2 position, Vector2 HUDPosition, int ammosupply)
        {
            milliCooldown = millicooldown;
            clipSize = clipsize;
            clipSupply = clipsize;
            reloadCooldown = reloadLength;
            WeaponTexture = Content.Load<Texture2D>("Graphics\\LeeEnfield");
            bullet = Content.Load<Texture2D>("Graphics\\LeeEnfieldAmmo");
            base.Initialize(Content, spriteBatch, gunPoint, position, HUDPosition, ammosupply);
        }
        override public void DrawHUD(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (isFireable(gameTime)) spriteBatch.Draw(WeaponTexture, hudPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            else spriteBatch.Draw(WeaponTexture, hudPosition, null, Color.White * disabledAlpha, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            base.DrawHUD(spriteBatch, gameTime);
        }

    }
}
