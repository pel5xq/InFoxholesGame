using InFoxholes.Friendlies;
using InFoxholes.Layouts;
using InFoxholes.Windows;
using InFoxholes.Waves;
using InFoxholes.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace InFoxholes.Conversations
{
    public class ConversationManager
    {

        public int State;
        // 0 Precomputations
        // 1 Ask who to talk to
        // 2 Ask which question to ask
        // 3 Display response
        // 4 waiting for waveManager to pick thread back up
        WaveManager manager;
        List<String> menNames;
        public bool conversationIsFinished;
        private int lastNumScavengers;
        bool deathThisRound;
        List<Texture2D> portraits;
        List<Button> talkToButtons;
        List<bool> talkToButtonsHover;
        List<Button> talkAboutButtons;
        List<bool> talkAboutButtonsHover;
        int whichButtonHighlighted;
        int manBeingTalkedTo;
        String response;
        List<String> possibleResponses;
        Button endConversationButton;
        bool endConversationHover;
        List<Button> deadNames = new List<Button>();

        /* Magic Numbers */
        List<String> sampleNames = new List<String>() { "Andy", "Evan", "Bill", "Paul" };
        String preQuestion = "Talk about ";
        String deathQuestion = "'s death.";
        List<String> deathResponses = new List<String>() { 
            "\"I feel sick just thinking about it. \nThat could be me tomorrow, you know?\"",
            "\"It isn't right. He didn't deserve\n an end like that. None of us do.\"",
            "\"You shouldn't be asking. It's your\n fault he's gone. How are you going to\n sleep at night?\"",
            "\"Dust to dust. I try not to think about it.\"" };
        String homeQuestion = "'s home.";
        List<String> homeResponses = new List<String>() { 
            "\"Home's in Michigan. What I wouldn't give\n to be fly fishing right now with my brother . . .\"",
            "\"I just hope I have a home to return to\n when all of this is over.\"",
            "\"Not an hour goes by that I don't think\n about it. You never really \nappreciate something great until it's gone.\"",
            "\"I don't want to talk about it. \nI came here to escape that place. \nMy mistake, I guess.\"" };
        String warQuestion = "the war.";
        List<String> warResponses = new List<String>(){
            "\"We're on the right side of this, don't\nyou think? I'm not here just for duty,\nI'm fighting for good.\"",
            "\"It's an evil, no question about it.\nLet's hope it's just a lesser evil.\"",
            "\"I could see the justification for it\nwhen it was an idea. Reality is never\nas kind.\"",
            "\"Well, we're fighting it. No amount of\n thinking about it will change that.\""};
        String killingQuestion = "killing.";
        List<String> killingResponses = new List<String>(){
            "\"There's a reason I give you most of my bullets.\nI'd defend my life if one of them walked right\nthrough the door, but beyond that . . .\"",
            "\"Having second thoughts about being our \npoint man? We all know you are the best shot\nhere, and we trust you with our lives.\nThink on that.\"",
            "\"The killing I can stomach. It's sitting\nin foxholes that's hell. You have a\nweapon, a way to fight fate. But how\ncan I stop the end from coming?\"",
            "\"You seem to like it. I won't stop you.\""};
        String startConversation = "Talk to:";
        Vector2 startConversationPosition = new Vector2(300, 20);
        String dismissQuestion = "Don't say anything.";
        String emptyQuestion = "None left to talk to.";
        String dismissResponse = "You try to sleep the day off instead.";
        String emptyResponse = "All you can do is remember them.";
        Vector2 responsePosition = new Vector2(300, 100);
        List<Vector2> namePositionsL = new List<Vector2>() { 
            new Vector2(300, 50),
            new Vector2(300, 150),
            new Vector2(300, 250),
            new Vector2(300, 350),
            new Vector2(300, 430)};
        List<Vector2> namePositionsR = new List<Vector2>() { 
            new Vector2(375, 90),
            new Vector2(375, 190),
            new Vector2(350, 290),
            new Vector2(375, 390),
            new Vector2(520, 470)};
        List<Vector2> questionPositionsL = new List<Vector2>() { 
            new Vector2(300, 50),
            new Vector2(300, 150),
            new Vector2(300, 250),
            new Vector2(300, 350)};
        List<Vector2> questionPositionsR = new List<Vector2>() { 
            new Vector2(600, 90),
            new Vector2(600, 190),
            new Vector2(600, 290),
            new Vector2(600, 390)};
        Vector2 nameOffset = new Vector2(10, 5);
        Vector2 questionOffset = new Vector2(10, 5);
        float thresholdLength = .8f;
        Vector2 endConversationL = new Vector2(345, 415);
        Vector2 endConversationR = new Vector2(470, 465);
        String endConversationText = "OK";
        Vector2 endConversationOffset = new Vector2(50f, 10f);
        Vector2 portraitPosition = new Vector2(10, 50);
        float portraitScale = .5f;
        String deathAddition = " (Gone)";

        public void Initialize(ContentManager content, WaveManager waveManager)
        {
            manager = waveManager;
            menNames = sampleNames;
            State = 0;
            conversationIsFinished = false;
            deathThisRound = false;
            lastNumScavengers = -1;
            portraits = new List<Texture2D>();
            portraits.Add(content.Load<Texture2D>("Graphics\\Andy"));
            portraits.Add(content.Load<Texture2D>("Graphics\\Evan"));
            portraits.Add(content.Load<Texture2D>("Graphics\\Bill"));
            portraits.Add(content.Load<Texture2D>("Graphics\\Paul"));
            talkToButtons = new List<Button>();
            talkToButtonsHover = new List<bool>();
            talkAboutButtons = new List<Button>();
            talkAboutButtonsHover = new List<bool>();
            possibleResponses = new List<String>();
            endConversationButton = new Button(endConversationL, endConversationR, endConversationText, endConversationOffset);
            endConversationHover = false;
        }

        public void Update(GameTime gametime, ScavengerManager scavengerManager)
        {
            if (State == 0)
            {
                if (lastNumScavengers == -1)
                {
                    lastNumScavengers = scavengerManager.numberOfLiveScavengers();
                    deathThisRound = (lastNumScavengers < MainGame.numStartingLives);
                }
                else
                {
                    deathThisRound = scavengerManager.numberOfLiveScavengers() < lastNumScavengers;
                    lastNumScavengers = scavengerManager.numberOfLiveScavengers();
                }

                if (lastNumScavengers == 0)
                {
                    talkToButtons.Add(new Button(namePositionsL[sampleNames.Count], namePositionsR[sampleNames.Count], emptyQuestion, nameOffset));
                    talkToButtonsHover.Add(false);
                    for (int i = 0; i < MainGame.numStartingLives; i++)
                    {
                        deadNames.Add(new Button(namePositionsL[i], namePositionsR[i], sampleNames[i] + deathAddition, nameOffset));
                    }
                }
                else
                {
                    //Don't let dead scavnger's names be clickable
                    for (int i = 0; i < MainGame.numStartingLives - lastNumScavengers; i++)
                    {
                        deadNames.Add(new Button(namePositionsL[i], namePositionsR[i], sampleNames[i]+deathAddition, nameOffset));   
                    }
                    for (int i = MainGame.numStartingLives - lastNumScavengers; i < sampleNames.Count; i++)
                    {
                        talkToButtons.Add(new Button(namePositionsL[i], namePositionsR[i], sampleNames[i], nameOffset));
                        talkToButtonsHover.Add(false);
                    }
                    talkToButtons.Add(new Button(namePositionsL[sampleNames.Count], namePositionsR[sampleNames.Count], dismissQuestion, nameOffset));
                    talkToButtonsHover.Add(false);
                }

                whichButtonHighlighted = 0;
                State = 1;
            }

            if (State == 1)
            {
                if (MainGame.currentGamepadState.IsConnected)
                {
                    if (MainGame.currentGamepadState.DPad.Up == ButtonState.Pressed && MainGame.previousGamepadState.DPad.Up != ButtonState.Pressed ||
                        MainGame.currentGamepadState.DPad.Left == ButtonState.Pressed && MainGame.previousGamepadState.DPad.Left != ButtonState.Pressed ||
                        (MainGame.currentGamepadState.ThumbSticks.Left.Length() > thresholdLength && !(MainGame.previousGamepadState.ThumbSticks.Left.Length() > thresholdLength)
                        && MainGame.currentGamepadState.ThumbSticks.Left.Y > 0))
                    {
                        Menu.scrollClickEffectInstance.Play();
                        if (whichButtonHighlighted == 0) whichButtonHighlighted = talkToButtons.Count - 1;
                        else whichButtonHighlighted--;
                    }
                    else if (MainGame.currentGamepadState.DPad.Down == ButtonState.Pressed && MainGame.previousGamepadState.DPad.Down != ButtonState.Pressed ||
                        MainGame.currentGamepadState.DPad.Right == ButtonState.Pressed && MainGame.previousGamepadState.DPad.Right != ButtonState.Pressed ||
                        (MainGame.currentGamepadState.ThumbSticks.Left.Length() > thresholdLength && !(MainGame.previousGamepadState.ThumbSticks.Left.Length() > thresholdLength)
                        && MainGame.currentGamepadState.ThumbSticks.Left.Y < 0)) 
                    {
                        Menu.scrollClickEffectInstance.Play();
                        if (whichButtonHighlighted == talkToButtons.Count - 1) whichButtonHighlighted = 0;
                        else whichButtonHighlighted++;
                    }

                    for (int i = 0; i < talkToButtons.Count; i++)
                    {
                        if (i == whichButtonHighlighted) talkToButtonsHover[i] = true;
                        else talkToButtonsHover[i] = false;
                    }

                    if (MainGame.currentGamepadState.Buttons.A == ButtonState.Pressed && MainGame.previousGamepadState.Buttons.A != ButtonState.Pressed)
                    {
                        Menu.confirmClickEffectInstance.Play();
                        manBeingTalkedTo = whichButtonHighlighted;
                        whichButtonHighlighted = 0;
                        State = 2;
                    }
                }
                else
                {
                    for (int i = 0; i < talkToButtons.Count; i++)
                    {
                        if (talkToButtons[i].mouseIsOverButton(MainGame.currentMouseState.X, MainGame.currentMouseState.Y))
                        {
                            if (MainGame.currentMouseState.LeftButton == ButtonState.Pressed && MainGame.previousMouseState.LeftButton != ButtonState.Pressed)
                            {
                                Menu.confirmClickEffectInstance.Play();
                                manBeingTalkedTo = i;
                                State = 2;
                                break;
                            }
                            else
                            {
                                if (talkToButtonsHover[i] == false) Menu.scrollClickEffectInstance.Play();
                            }
                            talkToButtonsHover[i] = true;
                        }
                        else
                        {
                            talkToButtonsHover[i] = false;
                        }
                    }
                }
            }
            else if (State == 2)
            {
                //Initialize talkAboutButtons once depending on state
                if (talkAboutButtons.Count == 0)
                {
                    int questionPositionsCounter = 0;
                    if (lastNumScavengers == 0)
                    {
                        //skip right to responses
                        response = emptyResponse;
                        State = 3;
                        return;
                    }
                    else if (manBeingTalkedTo == lastNumScavengers) 
                    {
                        //dismissal, skip to response
                        response = dismissResponse;
                        State = 3;
                        return;
                    }
                    int trueManBeingTalkedTo = manBeingTalkedTo + (MainGame.numStartingLives - lastNumScavengers);
                    if (deathThisRound)
                    {
                        talkAboutButtons.Add(new Button(questionPositionsL[questionPositionsCounter],
                            questionPositionsR[questionPositionsCounter], preQuestion
                            + menNames[MainGame.numStartingLives - lastNumScavengers - 1] 
                            + deathQuestion, questionOffset));
                        possibleResponses.Add(deathResponses[trueManBeingTalkedTo]);
                        questionPositionsCounter++;
                        talkAboutButtonsHover.Add(false);
                    }
                    talkAboutButtons.Add(new Button(questionPositionsL[questionPositionsCounter],
                            questionPositionsR[questionPositionsCounter], preQuestion
                            + menNames[trueManBeingTalkedTo] + homeQuestion, questionOffset));
                    possibleResponses.Add(homeResponses[trueManBeingTalkedTo]);
                    questionPositionsCounter++;
                    talkAboutButtonsHover.Add(false);
                    talkAboutButtons.Add(new Button(questionPositionsL[questionPositionsCounter],
                            questionPositionsR[questionPositionsCounter], preQuestion
                            + warQuestion, questionOffset));
                    possibleResponses.Add(warResponses[trueManBeingTalkedTo]);
                    questionPositionsCounter++;
                    talkAboutButtonsHover.Add(false);
                    talkAboutButtons.Add(new Button(questionPositionsL[questionPositionsCounter],
                            questionPositionsR[questionPositionsCounter], preQuestion
                            + killingQuestion, questionOffset));
                    possibleResponses.Add(killingResponses[trueManBeingTalkedTo]);
                    questionPositionsCounter++;
                    talkAboutButtonsHover.Add(false);
                    //More questions here
                    
                }

                //Then normal checks
                if (MainGame.currentGamepadState.IsConnected)
                {
                    if (MainGame.currentGamepadState.DPad.Up == ButtonState.Pressed && MainGame.previousGamepadState.DPad.Up != ButtonState.Pressed ||
                        MainGame.currentGamepadState.DPad.Left == ButtonState.Pressed && MainGame.previousGamepadState.DPad.Left != ButtonState.Pressed ||
                        (MainGame.currentGamepadState.ThumbSticks.Left.Length() > thresholdLength && !(MainGame.previousGamepadState.ThumbSticks.Left.Length() > thresholdLength)
                        && MainGame.currentGamepadState.ThumbSticks.Left.Y > 0))
                    {
                        Menu.scrollClickEffectInstance.Play();
                        if (whichButtonHighlighted == 0) whichButtonHighlighted = talkAboutButtons.Count - 1;
                        else whichButtonHighlighted--;
                    }
                    else if (MainGame.currentGamepadState.DPad.Down == ButtonState.Pressed && MainGame.previousGamepadState.DPad.Down != ButtonState.Pressed ||
                        MainGame.currentGamepadState.DPad.Right == ButtonState.Pressed && MainGame.previousGamepadState.DPad.Right != ButtonState.Pressed ||
                        (MainGame.currentGamepadState.ThumbSticks.Left.Length() > thresholdLength && !(MainGame.previousGamepadState.ThumbSticks.Left.Length() > thresholdLength)
                        && MainGame.currentGamepadState.ThumbSticks.Left.Y < 0))
                    {
                        Menu.scrollClickEffectInstance.Play();
                        if (whichButtonHighlighted == talkAboutButtons.Count - 1) whichButtonHighlighted = 0;
                        else whichButtonHighlighted++;
                    }

                    for (int i = 0; i < talkAboutButtons.Count; i++)
                    {
                        if (i == whichButtonHighlighted) talkAboutButtonsHover[i] = true;
                        else talkAboutButtonsHover[i] = false;
                    }

                    if (MainGame.currentGamepadState.Buttons.A == ButtonState.Pressed && MainGame.previousGamepadState.Buttons.A != ButtonState.Pressed)
                    {
                        Menu.confirmClickEffectInstance.Play();
                        response = possibleResponses[whichButtonHighlighted];
                        whichButtonHighlighted = 0;
                        State = 3;
                    }
                }
                else
                {
                    for (int i = 0; i < talkAboutButtons.Count; i++)
                    {
                        if (talkAboutButtons[i].mouseIsOverButton(MainGame.currentMouseState.X, MainGame.currentMouseState.Y))
                        {
                            if (MainGame.currentMouseState.LeftButton == ButtonState.Pressed && MainGame.previousMouseState.LeftButton != ButtonState.Pressed)
                            {
                                Menu.confirmClickEffectInstance.Play();
                                response = possibleResponses[i];
                                State = 3;
                                break;
                            }
                            else
                            {
                                if (talkAboutButtonsHover[i] == false) Menu.scrollClickEffectInstance.Play();
                            }
                            talkAboutButtonsHover[i] = true;
                        }
                        else
                        {
                            talkAboutButtonsHover[i] = false;
                        }
                    }
                }
            }
            else if (State == 3)
            {
                //Listen for proceed button
                if (MainGame.currentGamepadState.IsConnected)
                {
                    endConversationHover = true;

                    if (MainGame.currentGamepadState.Buttons.A == ButtonState.Pressed && MainGame.previousGamepadState.Buttons.A != ButtonState.Pressed)
                    {
                        Menu.confirmClickEffectInstance.Play();
                        State = 4;
                    }
                }
                else
                {
                        if (endConversationButton.mouseIsOverButton(MainGame.currentMouseState.X, MainGame.currentMouseState.Y))
                        {
                            if (MainGame.currentMouseState.LeftButton == ButtonState.Pressed && MainGame.previousMouseState.LeftButton != ButtonState.Pressed)
                            {
                                Menu.confirmClickEffectInstance.Play();
                                State = 4;
                            }
                            else
                            {
                                if (endConversationHover == false) Menu.scrollClickEffectInstance.Play();
                            }
                            endConversationHover = true;
                        }
                        else
                        {
                            endConversationHover = false;
                        }
                }
            }
            else if (State == 4)
            {
                if (conversationIsFinished == false)
                {
                    talkToButtons.Clear();
                    talkToButtonsHover.Clear();
                    talkAboutButtons.Clear();
                    talkAboutButtonsHover.Clear();
                    possibleResponses.Clear();
                    deadNames.Clear();
                    manBeingTalkedTo = -1;
                    conversationIsFinished = true;
                }
            }
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(manager.blankScreen, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            if (State == 1)
            {
                spriteBatch.DrawString(MainGame.font, startConversation, startConversationPosition, Color.White);
                for (int i = 0; i < deadNames.Count; i++)
                {
                    spriteBatch.DrawString(MainGame.font, deadNames[i].text, 
                        Vector2.Add(deadNames[i].topLeft, deadNames[i].textOffset), Color.Red*.8f);
                }
                for (int i = 0; i < talkToButtons.Count; i++)
                {
                    talkToButtons[i].Draw(spriteBatch, Menu.pixel, talkToButtonsHover[i], Color.White);
                    int trueManBeingTalkedTo = i + (MainGame.numStartingLives - lastNumScavengers);
                    if (talkToButtonsHover[i] && trueManBeingTalkedTo < portraits.Count)
                        spriteBatch.Draw(portraits[trueManBeingTalkedTo], portraitPosition, null, Color.White, 0f, Vector2.Zero, portraitScale, SpriteEffects.None, 0f);

                }
            }
            else if (State == 2)
            {
                for (int i = 0; i < talkAboutButtons.Count; i++)
                {
                    talkAboutButtons[i].Draw(spriteBatch, Menu.pixel, talkAboutButtonsHover[i], Color.White);
                }
                int trueManBeingTalkedTo = manBeingTalkedTo + (MainGame.numStartingLives - lastNumScavengers);
                if (trueManBeingTalkedTo < portraits.Count)
                    spriteBatch.Draw(portraits[trueManBeingTalkedTo], portraitPosition, null, Color.White, 0f, Vector2.Zero, portraitScale, SpriteEffects.None, 0f);
            }
            //Draw portraits too
            else if (State == 3)
            {
                spriteBatch.DrawString(MainGame.font, response, responsePosition, Color.White);
                endConversationButton.Draw(spriteBatch, Menu.pixel, endConversationHover, Color.White);
                int trueManBeingTalkedTo = manBeingTalkedTo + (MainGame.numStartingLives - lastNumScavengers);
                if (trueManBeingTalkedTo < portraits.Count)
                    spriteBatch.Draw(portraits[trueManBeingTalkedTo], portraitPosition, null, Color.White, 0f, Vector2.Zero, portraitScale, SpriteEffects.None, 0f);
            }
            
        }
        
    }
}
