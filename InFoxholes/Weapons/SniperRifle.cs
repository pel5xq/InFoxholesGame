using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using InFoxholes.Waves;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;


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
            return  (float)(2 - 2.6 * timeElapsed + 1.7 * Math.Pow(timeElapsed, 2)) / 60000;
            //Classic: (float)(2 - 2.6 * timeElapsed + 1.7 * Math.Pow(timeElapsed, 2)) / 30000;
        }
        override public void Initialize(ContentManager Content, SpriteBatch spriteBatch, int HUDPosition, int ammosupply, WaveManager manager)
        {
            milliCooldown = millicooldown;
            clipSize = clipsize;
            clipSupply = clipsize;
            reloadCooldown = reloadLength;
            WeaponTexture = Content.Load<Texture2D>("Graphics\\LeeEnfield");
            bullet = Content.Load<Texture2D>("Graphics\\LeeEnfieldAmmo");
            shotSound = Content.Load<SoundEffect>("Music\\rifle.wav");
            reloadSound = Content.Load<SoundEffect>("Music\\Sniper_Fire_Reload.wav");
            base.Initialize(Content, spriteBatch, HUDPosition, ammosupply, manager);
        }
        override public void DrawHUD(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Vector2 hudPosition = waveManager.getWave().layout.getHUDPlacement(hudSeat);
            if (isFireable(gameTime)) spriteBatch.Draw(WeaponTexture, hudPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            else spriteBatch.Draw(WeaponTexture, hudPosition, null, Color.White * disabledAlpha, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            base.DrawHUD(spriteBatch, gameTime);
        }

        public override void playShot(GameTime currentTime)
        {
            if (clipSupply == 0 && ammoSupply == 0) emptyClipSound.Play();
            else if (!reloadOver(currentTime)) emptyClipSound.Play();
            else
            {
                if (isFireable(currentTime)) shotSound.Play();
            }
        }

    }
}
