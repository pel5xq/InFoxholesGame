using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameName1
{
    abstract class Weapon
    {
        public Texture2D WeaponTexture;
        public Vector2 Position;
        public Vector2 ShotPoint;
        public Texture2D pixel;
        public Texture2D burst;
        public Vector2 burstPoint;
        public Vector2 GunPoint;
        public Vector2 hudPosition;
        public double lastShotMilli;
        public double milliCooldown;
        public bool isSelected;
        public int clipSize;
        public int ammoSupply;

        /* Magic Numbers */
        private int hudPadding = 5;
        private float lineThickness = 2f;
        private float halfPi = (float)(Math.PI / 2);

        virtual public void Initialize(SpriteBatch spriteBatch, Vector2 gunPoint, Texture2D rifleBurst, 
            Texture2D texture, Vector2 position, Vector2 HUDPosition, int ammosupply)
        {
            pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            burst = rifleBurst;
            pixel.SetData(new[] { Color.White });
            GunPoint = gunPoint;
            burstPoint = new Vector2(GunPoint.X - burst.Width / 2, GunPoint.Y - burst.Height / 2);
            ShotPoint = Vector2.Zero;
            lastShotMilli = 0;
            WeaponTexture = texture;
            Position = position;
            hudPosition = HUDPosition;
            ammoSupply = ammosupply;
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
        }

        public void startShotCooldown(GameTime shotTime)
        {
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

        abstract public float GetCrosshairVelocity(double timeElapsed);
        //Add methods on ammo capacity, bullet texture, etc.
    }
}
