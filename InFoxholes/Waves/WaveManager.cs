using InFoxholes.Friendlies;
using InFoxholes.Layouts;
using InFoxholes.Windows;
using InFoxholes.Util;
using InFoxholes.Conversations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
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
        Button continueButton;
        public Texture2D pixel;
        bool hoverFlag = false;
        double endGracePeriod;
        int secondsLeft;
        public ConversationManager conversationManager;
        bool delegateToConvo;
        ScavengerManager scavengerManager;

        /* Magic Numbers */
        double gracePeriodLength = 15000;
        static int startButtonLX = 345;
        static int startButtonLY = 415;
        static int startButtonRX = 470;
        static int startButtonRY = 465;
        Vector2 continueTextOffset = new Vector2(50f, 10f);
        String buttonText = "OK";
        Vector2 mainTextPosition = new Vector2(190f, 100f);
        String countdownText = "End of Day in: ";
        Vector2 helpTexturePosition = new Vector2(50, 195);

        public void Initialize(ContentManager content)
        {
            conversationManager = new ConversationManager();
            conversationManager.Initialize(content, this);
            delegateToConvo = false;
            waves = new List<Wave>();
            currentWave = 0;
            State = 0;
            waves.Add(new TutorialWave1());
            waves.Add(new TutorialWave2());
            waves.Add(new TutorialWave3());
            waves.Add(new OpeningWave());
            //waves.Add(new Wave1());
            //waves.Add(new Wave2());
            for (int i = 0; i < waves.Count; i++)
            {
                waves[i].Initialize(content, this);
            }
            blankScreen = content.Load<Texture2D>("Graphics\\BlackScreen");
            continueButton = new Button(new Vector2(startButtonLX, startButtonLY), 
                new Vector2(startButtonRX, startButtonRY), buttonText, continueTextOffset);
            endGracePeriod = 0;
            secondsLeft = (int) (gracePeriodLength / 1000);
            getWave().applyModes();
        }

        public void setScavengerManager(ScavengerManager scavengermanager) {
            scavengerManager = scavengermanager;
        }

        public bool isHit(Vector2 crosshairPosition)
        {
            return getWave().isHit(crosshairPosition);
        }

        public void Update(GameTime gametime)
        {
            if (State == 0)
            {
                if (MainGame.backgroundMusic.State != SoundState.Playing)
                {
                    /*if (MainGame.backgroundMusic.State == SoundState.Paused) {
                        MainGame.backgroundMusic.Resume();
                        Menu.windEffectInstance.Resume();
                    }
                    else {
                        MainGame.backgroundMusic.Play();
                        Menu.windEffectInstance.Play();
                    }*/

                    MainGame.backgroundMusic.Play();
                    Menu.windEffectInstance.Play();
                }
                if (MainGame.currentGamepadState.Buttons.A == ButtonState.Pressed && 
                    !(MainGame.previousGamepadState.Buttons.A == ButtonState.Pressed))
                {
                    Menu.confirmClickEffectInstance.Play();
                    State = 1;
                }
                if (continueButton.mouseIsOverButton(Mouse.GetState().X, Mouse.GetState().Y))
                {
                    if (hoverFlag == false) Menu.scrollClickEffectInstance.Play();
                    hoverFlag = true;
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed 
                        && MainGame.previousMouseState.LeftButton != ButtonState.Pressed)
                    {
                        Menu.confirmClickEffectInstance.Play();
                        State = 1;
                    }
                }
                else
                {
                    if (MainGame.currentGamepadState.IsConnected) hoverFlag = true;
                    else hoverFlag = false;
                }
            }
            else if (State == 1)
            {
                getWave().Update(gametime, scavengerManager);
                if (getWave().isOver(scavengerManager))
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
                    /*if (MainGame.backgroundMusic.State == SoundState.Playing)
                    {
                        MainGame.backgroundMusic.Pause();
                        Menu.windEffectInstance.Pause();
                    }*/
                    if (conversationManager.conversationIsFinished)
                    {
                        delegateToConvo = false;
                        nextWave();
                        scavengerManager.returnToTrench();
                        State = 0;
                        MainGame.resetScavengeCommand();
                        scavengerManager.cleanUpBodies();
                        conversationManager.conversationIsFinished = false;
                        conversationManager.State = 0;
                    }
                    else
                    {
                        delegateToConvo = true;
                        conversationManager.Update(gametime, scavengerManager);
                    }
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
                spriteBatch.DrawString(MainGame.font, getWave().openingText, mainTextPosition, Color.White);
                continueButton.Draw(spriteBatch, pixel, hoverFlag, Color.White);
                if (MainGame.currentGamepadState.IsConnected) { 
                    if(null != getWave().helpTextureController)
                        spriteBatch.Draw(getWave().helpTextureController, helpTexturePosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }
                else if (null != getWave().helpTexture) 
                    spriteBatch.Draw(getWave().helpTexture, helpTexturePosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
            else if (State == 1)
            {
                getWave().Draw(spriteBatch);
            }
            else if (State == 2)
            {
                if (delegateToConvo)
                {
                    conversationManager.Draw(spriteBatch);
                }
                else
                {
                    getWave().Draw(spriteBatch);
                    spriteBatch.DrawString(MainGame.font, countdownText + secondsLeft, getWave().layout.countdownPosition, Color.Black);
                }
            }
        }

        public Wave getWave()
        {
            return waves[currentWave];
        }

        public void nextWave()
        {
            if (currentWave + 1 < waves.Count)
            {
                currentWave++;
                getWave().applyModes();
            }
        }

        public void skipTutorialWaves()
        {
            while (getWave().isTutorialWave)
            {
                nextWave();
                scavengerManager.returnToTrench();
            }
        }
    }
}
