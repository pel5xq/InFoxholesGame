using InFoxholes.Friendlies;
using InFoxholes.Layouts;
using InFoxholes.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace InFoxholes.Waves
{
    public class WaveManager
    {
        public List<Wave> waves;
        public int currentWave;
        public int State; 
        // 0 = Wave Introduction, 1 = Wave Day
        // 2 = End of Day Countdown, 3 = Wave Conclusion
        public Texture2D blankScreen;
        Vector2 topleft;
        Vector2 botright;
        public Texture2D pixel;
        bool hoverFlag = false;
        double endGracePeriod;
        int secondsLeft;

        /* Magic Numbers */
        double gracePeriodLength = 15000;
        int numberOfImplementedWaves = 2;
        static int startButtonLX = 345;
        static int startButtonLY = 415;
        static int startButtonRX = 470;
        static int startButtonRY = 465;
        int startButtonWidth = startButtonRX - startButtonLX;
        int startButtonHeight = startButtonRY - startButtonLY;
        private float lineThickness = 4f;
        private float halfPi = (float)(Math.PI / 2);
        Vector2 startTextPosition = new Vector2(395f, 425f);
        String buttonText = "OK";
        Vector2 mainTextPosition = new Vector2(190f, 100f);
        String countdownText = "End of Day in: ";

        public void Initialize(ContentManager content)
        {
            waves = new List<Wave>();
            currentWave = 0;
            State = 0;
            waves.Add(new Wave1());
            waves.Add(new Wave2());
            for (int i = 0; i < waves.Count; i++)
            {
                waves[i].Initialize(content, this);
            }
            blankScreen = content.Load<Texture2D>("Graphics\\BlackScreen");
            topleft = new Vector2(startButtonLX, startButtonLY);
            botright = new Vector2(startButtonRX, startButtonRY);
            endGracePeriod = 0;
            secondsLeft = (int) (gracePeriodLength / 1000);
            getWave().applyModes();
        }

        public bool isHit(Vector2 crosshairPosition)
        {
            return getWave().isHit(crosshairPosition);
        }

        public void Update(GameTime gametime, ScavengerManager scavengerManager)
        {
            if (State == 0)
            {
                if (overStartButton(Mouse.GetState().X, Mouse.GetState().Y))
                {
                    hoverFlag = true;
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed && MainGame.previousMouseState.LeftButton != ButtonState.Pressed)
                    {
                        State = 1;
                    }
                }
                else
                {
                    hoverFlag = false;
                }
            }
            else if (State == 1)
            {
                getWave().Update(gametime, scavengerManager);
                if (getWave().isOver())
                {
                    State = 2;
                    endGracePeriod = gametime.TotalGameTime.TotalMilliseconds + gracePeriodLength;
                    secondsLeft = (int)(gracePeriodLength / 1000);
                }
            }
            else if (State == 2)
            {
                getWave().Update(gametime, scavengerManager);
                secondsLeft = (int)((endGracePeriod - gametime.TotalGameTime.TotalMilliseconds) / 1000);
                if (gametime.TotalGameTime.TotalMilliseconds > endGracePeriod)
                {
                    scavengerManager.returnToTrench();
                    nextWave();
                    State = 0;
                    MainGame.resetScavengeCommand();
                    scavengerManager.cleanUpBodies();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (pixel == null)
            {
                pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                pixel.SetData(new[] { Color.White });
            }
            if (State == 0)
            {
                spriteBatch.Draw(blankScreen, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(MainGame.font, buttonText, startTextPosition, Color.White);
                spriteBatch.DrawString(MainGame.font, getWave().openingText, mainTextPosition, Color.White);
                if (hoverFlag)
                {
                    spriteBatch.Draw(pixel, topleft, null, Color.White, 0, Vector2.Zero, new Vector2(125, lineThickness),
                        SpriteEffects.None, 0);
                    spriteBatch.Draw(pixel, topleft, null, Color.White, halfPi, Vector2.Zero, new Vector2(50, lineThickness),
                        SpriteEffects.None, 0);
                    spriteBatch.Draw(pixel, botright, null, Color.White, 2 * halfPi, Vector2.Zero, new Vector2(125, lineThickness),
                        SpriteEffects.None, 0);
                    spriteBatch.Draw(pixel, botright, null, Color.White, -1 * halfPi, Vector2.Zero, new Vector2(50, lineThickness),
                        SpriteEffects.None, 0);
                }
            }
            else if (State == 1)
            {
                getWave().Draw(spriteBatch);
            }
            else if (State == 2)
            {
                getWave().Draw(spriteBatch);
                spriteBatch.DrawString(MainGame.font, countdownText + secondsLeft, getWave().layout.countdownPosition, Color.Black);
            }
        }

        public Wave getWave()
        {
            return waves[currentWave];
        }

        public void nextWave()
        {
            if (currentWave + 1 < numberOfImplementedWaves)
            {
                currentWave++;
                getWave().applyModes();
            }
        }
        private bool overStartButton(int mouseX, int mouseY)
        {
            if (mouseX <= startButtonRX && mouseX >= startButtonLX
                && mouseY <= startButtonRY && mouseY >= startButtonLY)
            {
                return true;
            }
            return false;
        }
    }
}
