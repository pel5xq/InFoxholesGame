﻿using System.Collections.Generic; 
using InFoxholes.Looting;
using InFoxholes.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;


namespace InFoxholes.Enemies
{

    /* 
     * RegionedEnemy Class
     * This class is an extension of the basic enemy class.
     * It allows for creation of enemies that are sensitive 
     * to regioned shots. So, we can differentiate enemy hits
     * based on where they are, like a head shot vs. a chest shot. 
     * 
     * This is done by overriding the isHit method.
     * The player shot is divided over the enemy height. This is 
     * the "region" fraction. This region fraction is compared against 
     * a regions list. This regions list has an accompanying damages list.
     * Both lists along with an enemy health must be declared in child classes.
     * 
     * How to use regions list: 
     * The list should start with the first region, and end with 1
     * So, if we wanted to divide the enemy into two regions (top
     * and bottom) we would declare the list as {.5, 1}
     * The top of the enemy is 0, so .5 refers to the top half of the
     * enemy. 1 refers to the bottom half (from .5 to 1)
     * 
     * Then, for damages list, again go from top down. If we wanted
     * the top half of the enemy to be twice the damage of the bottom,
     * we would declare the damages as {2, 1}
     * 
     **/
    public class RegionedEnemy : Enemy
    {

        public List<double> regions;
        public List<int> damages;
        public List<SoundEffectInstance> sounds;
        public int health;

        override public bool isHit(Vector2 crosshairPosition)
        {
            Vector2 truePosition = Vector2.Subtract(crosshairPosition, Position);
            if (Alive &&
                truePosition.X >= 0 &&
                truePosition.Y >= 0 &&
                truePosition.X <= Width &&
                truePosition.Y <= Height)
            {
                double shotRegion = truePosition.Y / Height;
                int shotDamage = 0; 
                for(int i=0; i < regions.Count; i++) {
                    if(i == regions.Count-1) {
                        shotDamage = damages[i];
                        if (null != sounds[i]) sounds[i].Play();
                        break;
                    }
                    else if (regions[i+1] > shotRegion) {
                        shotDamage = damages[i];
                        if (null != sounds[i]) sounds[i].Play();
                        break;
                    }
                }
                health -= shotDamage; 
                if (health <= 0)
                {
                    Alive = false;
                }
                return true;
            }
            return false;
        }

    }
}
