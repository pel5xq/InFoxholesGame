using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using InFoxholes.Util;

namespace InFoxholes.Layouts
{
    public class LadderLayout : Layout
    {
        bool spawnSwitch;

        /* Magic Numbers */
        Vector2 enemySpawns = new Vector2(280, 465); 
        List<Vector2> scavengerSpawns = new List<Vector2>() { 
            new Vector2(15, 121), 
            new Vector2(55, 121), //225, 182
            new Vector2(95, 121), 
            new Vector2(135, 121) };
        List<Vector2> HUDPositions = new List<Vector2>() { 
            new Vector2(10, 10), 
            new Vector2(448, 10), 
            new Vector2(10, 50), 
            new Vector2(10, 100) };
        Vector2 offscreenposition = new Vector2(-150, -150);
        Vector2 countdownposition = new Vector2(475f, 50f);
        Vector2 weapongunpoint = new Vector2(150, 100);
        Vector2 weaponposition = new Vector2(-1000, -1000);
        Vector2 scavengerspawnposition = new Vector2(135, 90);
        float angleadjust = .25f;
        float distanceadjust = -15f;
        float burstadjustX = -5;
        float burstadjustY = -5;
        float crosshairadjustX = -15;
        float crosshairadjustY = -15;

        public LadderLayout()
        {
            pather = new LadderPather();
            offscreenPosition = offscreenposition;
            countdownPosition = countdownposition;
            weaponPosition = weaponposition;
            weaponGunpoint = weapongunpoint;
            scavengerSpawnPosition = scavengerspawnposition;
            angleAdjust = angleadjust;
            distanceAdjust = distanceadjust;
            spawnSwitch = false;
            burstAdjustX = burstadjustX;
            burstAdjustY = burstadjustY;
            crosshairAdjustX = crosshairadjustX;
            crosshairAdjustY = crosshairadjustY;
        }
        public override void Initialize(ContentManager Content)
        {
            layoutBackdrop = new AnimatedSprite(Content.Load<Texture2D>("Graphics\\LadderLevel"), 1, 1, 10);
            gameOverSprite = new AnimatedSprite(Content.Load<Texture2D>("Graphics\\LadderGameOver"), 1, 1, 10);
            base.Initialize(Content);
        }
        public override Vector2 getScavengerTrenchPlacement(int seat)
        {
            if (seat < scavengerSpawns.Count) return scavengerSpawns[seat];
            else return scavengerSpawns[scavengerSpawns.Count - 1];
        }
        public override Vector2 getHUDPlacement(int seat)
        {
            if (seat < HUDPositions.Count) return HUDPositions[seat];
            else return HUDPositions[HUDPositions.Count - 1];
        }
        public override bool checkAimingVector(Vector2 aimingVector)
        {
            //return aimingVector.Y > 0;
            return true;
        }
        public override Vector2 enemySpawnPoint()
        {
            return enemySpawns;
        }
        public override bool isOnGround(Vector2 position, int width, int height)
        {
            return position.Y >= 90; 
        }
    }
}
