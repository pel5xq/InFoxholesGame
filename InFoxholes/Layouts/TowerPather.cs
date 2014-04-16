using Microsoft.Xna.Framework;
using System;


namespace InFoxholes.Layouts
{
    public class TowerPather : Pather
    {
        private static Random rnd = new Random();

        /* Magic numbers */
        static int halfwayX = 335;
        Vector2 trenchentranceposition = new Vector2(280, 165);
        int trenchentrancewidth = 115;
        int trenchentranceheight = 60;
        Vector2 columnPosition = new Vector2(265, 115);
        int columnWidth = 70;
        int columnHeight = 300;
        int endOfRoadY = 410;

        public override void Initialize()
        {
            trenchEntrancePosition = trenchentranceposition;
            trenchEntranceWidth = trenchentrancewidth;
            trenchEntranceHeight = trenchentranceheight;
            base.Initialize();
        }
        public override Vector2 Move(Vector2 unitPosition, bool isTowardsTrench, float speed)
        {
            Vector2 result = new Vector2();
            if (isTowardsTrench)
            {
                if (intersectsWith(unitPosition, 1, 1, columnPosition, columnWidth, columnHeight))
                {
                    result.X = unitPosition.X;
                    if (isTowardsTrench) result.Y = unitPosition.Y - speed;
                    else result.Y = unitPosition.Y + speed;
                }
                else
                {
                    result.Y = unitPosition.Y;
                    if (unitPosition.Y >= endOfRoadY)
                    {
                        result.X = unitPosition.X;
                        if (isTowardsTrench)
                        {
                            result.Y = unitPosition.Y - speed;
                        }
                    }
                    else if (isTowardsTrench)
                    {
                        if (isForward(unitPosition, isTowardsTrench, speed))
                        {
                            result.X = unitPosition.X + speed;
                        }
                        else
                        {
                            result.X = unitPosition.X - speed;
                        }
                    }
                    else
                    {
                        if (isForward(unitPosition, isTowardsTrench, speed))
                        {
                            result.X = unitPosition.X - speed;
                        }
                        else
                        {
                            result.X = unitPosition.X + speed;
                        }
                    }
                }
            }
            else
            {
                if (unitPosition.Y < 380)
                {
                    result.X = unitPosition.X;
                    result.Y = unitPosition.Y + speed;
                }
                else
                {
                    if (unitPosition.X < 335)
                    {
                        result.X = unitPosition.X - speed;
                        result.Y = unitPosition.Y;
                    }
                    else if (unitPosition.X > 335)
                    {
                        result.X = unitPosition.X + speed;
                        result.Y = unitPosition.Y;
                    }
                    else
                    {
                        result.X = unitPosition.X + speed;
                        result.Y = unitPosition.Y;
                        if (rnd.Next(2) == 0) result.X = unitPosition.X + speed;
                        else result.X = unitPosition.X - speed;
                    }
                    
                }
            }
            return result;
        }
        public override bool isForward(Vector2 unitPosition, bool isTowardsTrench, float speed)
        {
            if (isTowardsTrench) return unitPosition.X < halfwayX;
            else return unitPosition.X >= halfwayX;
        }
    }
}
