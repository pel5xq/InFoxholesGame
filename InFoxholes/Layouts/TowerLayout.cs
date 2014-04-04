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
    public class TowerLayout : Layout
    {
        bool spawnSwitch;

        /* Magic Numbers */
        List<Vector2> enemySpawns = new List<Vector2>() { 
            new Vector2(800, 380),
            new Vector2(-50, 380) };
        List<Vector2> scavengerSpawns = new List<Vector2>() { 
            new Vector2(170, 193), 
            new Vector2(505, 193), //225, 182
            new Vector2(415, 193), 
            new Vector2(470, 193) };
        List<Vector2> HUDPositions = new List<Vector2>() { 
            new Vector2(10, 10), 
            new Vector2(448, 10), 
            new Vector2(10, 50), 
            new Vector2(10, 100) };
        Vector2 offscreenposition = new Vector2(-150, -150);
        Vector2 countdownposition = new Vector2(475f, 50f);
        Vector2 weapongunpoint = new Vector2(345, 82);
        Vector2 weaponposition = new Vector2(-95, -180);
        Vector2 scavengerspawnposition = new Vector2(315, 180);
        float angleadjust = .25f;
        float distanceadjust = -15f;
        float burstadjustX = -5;
        float burstadjustY = -5;
        float crosshairadjustX = -15;
        float crosshairadjustY = -15;

        public TowerLayout()
        {
            pather = new TowerPather();
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
            layoutBackdrop = new AnimatedSprite(Content.Load<Texture2D>("Graphics\\TowerBackdrop"), 1, 1, 10);
            gameOverSprite = new AnimatedSprite(Content.Load<Texture2D>("Graphics\\TowerBackdropGameOver"), 1, 1, 10);
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
            spawnSwitch = !spawnSwitch;
            if (spawnSwitch) return enemySpawns[0];
            else return enemySpawns[1];
        }
        public override bool isOnGround(Vector2 position, int width, int height)
        {
            return position.Y >= enemySpawns[0].Y;
        }
    }
}
