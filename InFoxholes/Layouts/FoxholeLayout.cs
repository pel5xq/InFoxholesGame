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
    public class FoxholeLayout : Layout
    {

        /* Magic Numbers */
        Vector2 enemySpawn = new Vector2(900, 212);
        List<Vector2> scavengerSpawns = new List<Vector2>() { 
            new Vector2(30, 300), 
            new Vector2(30, 350), 
            new Vector2(0, 320), 
            new Vector2(70, 350) };
        //Trench must be bigger for more scavengers
        List<Vector2> HUDPositions = new List<Vector2>() { 
            new Vector2(10, 10), 
            new Vector2(10, 50), 
            new Vector2(10, 100), 
            new Vector2(10, 150) };
        Vector2 offscreenposition = new Vector2(-150, -150);
        Vector2 countdownposition = new Vector2(350f, 115f);
        Vector2 weapongunpoint = new Vector2(262, 287);
        Vector2 weaponposition = new Vector2(177, 282);
        Vector2 scavengerspawnposition = new Vector2(230, 212);
        float angleadjust = .25f;
        float distanceadjust = -15f;
        float burstadjustX = -5;
        float burstadjustY = -5;
        float crosshairadjustX = 0;
        float crosshairadjustY = -15;

        public FoxholeLayout()
        {
            pather = new FoxholePather();
            offscreenPosition = offscreenposition;
            countdownPosition = countdownposition;
            weaponPosition = weaponposition;
            weaponGunpoint = weapongunpoint;
            scavengerSpawnPosition = scavengerspawnposition;
            angleAdjust = angleadjust;
            distanceAdjust = distanceadjust;
            burstAdjustX = burstadjustX;
            burstAdjustY = burstadjustY;
            crosshairAdjustX = crosshairadjustX;
            crosshairAdjustY = crosshairadjustY;
        }
        public override void Initialize(ContentManager Content)
        {
            layoutBackdrop = new AnimatedSprite(Content.Load<Texture2D>("Graphics\\FoxholeBackdrop"), 2, 2, 10);
            gameOverSprite = new AnimatedSprite(Content.Load<Texture2D>("Graphics\\FoxholeBackdropGameOver"), 1, 1, 10);
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
            return aimingVector.X > 0;
        }
        public override Vector2 enemySpawnPoint()
        {
            return enemySpawn;
        }
        public override bool isOnGround(Vector2 position, int width, int height)
        {
            return position.Y >= enemySpawn.Y;
        }
    }
}
