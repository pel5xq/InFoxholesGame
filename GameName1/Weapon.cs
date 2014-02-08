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
    public abstract class Weapon
    {
        public Texture2D WeaponTexture;
        public Vector2 Position;
        public Vector2 ShotPoint;
        public static Texture2D pixel;
        public Texture2D burst;
        public Texture2D bullet;
        public Vector2 burstPoint;
        public Vector2 GunPoint;
        public Vector2 hudPosition;
        public double lastShotMilli;
        public double milliCooldown;
        public double reloadMilli;
        public double reloadCooldown;
        public bool isSelected;
        public int clipSize;
        public int clipSupply;
        public int ammoSupply;

        /* Magic Numbers */
        private int hudPadding = 5;
        private float lineThickness = 2f;
        private float halfPi = (float)(Math.PI / 2);

        virtual public void Initialize(ContentManager Content, SpriteBatch spriteBatch, Vector2 gunPoint, Vector2 position, Vector2 HUDPosition, int ammosupply)
        {
            pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            burst = Content.Load<Texture2D>("Graphics\\rifleBurst");
            pixel.SetData(new[] { Color.White });
            GunPoint = gunPoint;
            burstPoint = new Vector2(GunPoint.X - burst.Width / 2, GunPoint.Y - burst.Height / 2);
            ShotPoint = Vector2.Zero;
            lastShotMilli = 0;
            reloadMilli = 0;
            
            Position = position;
            hudPosition = HUDPosition;
            ammoSupply = ammosupply - clipSize;
        }

        public int Width
        {
            get { return WeaponTexture.Width; }
        }

        public int Height
        {
            get { return WeaponTexture.Height; }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(WeaponTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            if (!ShotPoint.Equals(Vector2.Zero))
            {
                spriteBatch.Draw(burst, burstPoint, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                float distance = Vector2.Distance(GunPoint, ShotPoint);
                float angle = (float)Math.Atan2(ShotPoint.Y - GunPoint.Y, ShotPoint.X - GunPoint.X);
                spriteBatch.Draw(pixel, GunPoint, null, Color.Black, angle, Vector2.Zero, new Vector2(distance, lineThickness),
                             SpriteEffects.None, 0);
                ShotPoint = Vector2.Zero;
            }
        }

        virtual public void DrawHUD(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //Draw rectangle around if selected
            if (isSelected)
            {
                Vector2 topleft = new Vector2(hudPosition.X - hudPadding, hudPosition.Y - hudPadding);
                Vector2 botright = new Vector2(hudPosition.X + WeaponTexture.Width + hudPadding, hudPosition.Y + WeaponTexture.Height + hudPadding);
                spriteBatch.Draw(pixel, topleft, null, Color.Black, 0, Vector2.Zero, new Vector2(WeaponTexture.Width+ 2 * hudPadding, lineThickness), 
                    SpriteEffects.None, 0);
                spriteBatch.Draw(pixel, topleft, null, Color.Black, halfPi, Vector2.Zero, new Vector2(WeaponTexture.Height + 2 * hudPadding, lineThickness), 
                    SpriteEffects.None, 0);
                spriteBatch.Draw(pixel, botright, null, Color.Black, 2*halfPi, Vector2.Zero, new Vector2(WeaponTexture.Width + 2 * hudPadding, lineThickness), 
                    SpriteEffects.None, 0);
                spriteBatch.Draw(pixel, botright, null, Color.Black, -1*halfPi, Vector2.Zero, new Vector2(WeaponTexture.Height + 2 * hudPadding, lineThickness), 
                    SpriteEffects.None, 0);
            }
            //Leave spaces for all shot bullets in clip, then bullets in clip, then a space and then bullets in supply
            for (int i = clipSize - clipSupply; i < clipSize; i++)
            {
                spriteBatch.Draw(bullet, new Vector2(hudPosition.X + hudPadding * 2 + WeaponTexture.Width + i * bullet.Width, hudPosition.Y), 
                    null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
            for (int i = clipSize; i < ammoSupply + clipSize; i++)
            {
                spriteBatch.Draw(bullet, new Vector2(hudPosition.X + hudPadding * 2 + WeaponTexture.Width + (i + 2) * bullet.Width, hudPosition.Y),
                    null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }

        public void startShotCooldown(GameTime shotTime)
        {
            removeAmmo(shotTime);
            lastShotMilli = shotTime.TotalGameTime.TotalMilliseconds;
        }

        public bool cooldownOver(GameTime currentTime)
        {
            if (0 == lastShotMilli || currentTime.TotalGameTime.TotalMilliseconds - lastShotMilli > milliCooldown)
            {
                return true;
            }
            return false;
        }

        public bool reloadOver(GameTime currentTime)
        {
            if (0 == reloadMilli || currentTime.TotalGameTime.TotalMilliseconds - reloadMilli > reloadCooldown)
            {
                return true;
            }
            return false;
        }

        public void reload(GameTime currentTime)
        {
            if (ammoSupply > 0 && clipSupply != clipSize)
            {
                int ammoToReplenish = clipSize - clipSupply;
                if (ammoSupply >= ammoToReplenish)
                {
                    clipSupply = clipSize;
                    ammoSupply = ammoSupply - ammoToReplenish;
                }
                else
                {
                    clipSupply = ammoSupply + clipSupply;
                    ammoSupply = 0;
                }
                reloadMilli = currentTime.TotalGameTime.TotalMilliseconds;
            }
        }

        public void removeAmmo(GameTime currentTime)
        {
            clipSupply = clipSupply - 1;
            if (clipSupply == 0) reload(currentTime);
        }

        public bool isFireable(GameTime currentTime)
        {
            return cooldownOver(currentTime) && reloadOver(currentTime) && clipSupply > 0;
        }

        abstract public float GetCrosshairVelocity(double timeElapsed);
    }
}
