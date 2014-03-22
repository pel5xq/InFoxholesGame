﻿using Microsoft.Xna.Framework;


namespace InFoxholes.Layouts
{
    public class FoxholePather : Pather
    {
        /* Magic numbers */
        static int startY = 150;
        static int hillBottomX = 270;
        static int hillTopX = 210;
        static int ladderTopX = 122;
        static int hillBottomY = 145 + startY;
        static int hillTopY = 15 + startY;
        static int ladderTopY = 15 + startY;
        static float slope = (hillTopY - hillBottomY)/(hillTopX - hillBottomX);


        public override Vector2 Move(Vector2 unitPosition, bool isTowardsTrench, float speed)
        {
            Vector2 result = new Vector2();
            float correctedSpeed;
            if (isTowardsTrench) correctedSpeed = -1 * speed;
            else correctedSpeed = speed;
            result.X = unitPosition.X + correctedSpeed;
            if (unitPosition.X > hillBottomX)
            {
                result.Y = unitPosition.Y;
            }
            else if (unitPosition.X > hillTopX)
            {
                result.Y = hillTopY + slope * (result.X - hillTopX);
            }
            else if (unitPosition.X > ladderTopX)
            {
                result.Y = unitPosition.Y;
            }
            else
            {
                result = unitPosition;
            }
            return result;
        }
        public override bool isForward(Vector2 unitPosition, bool isTowardsTrench, float speed)
        {
            return !isTowardsTrench;
        }
        public override bool atTrenchEntrance(Vector2 unitPosition, int unitWidth, int unitHeight)
        {
            return unitPosition.X <= ladderTopX;
        }
    }
}
