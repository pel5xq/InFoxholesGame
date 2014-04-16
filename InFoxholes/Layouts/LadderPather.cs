using Microsoft.Xna.Framework;
using System;


namespace InFoxholes.Layouts
{
    public class LadderPather : Pather
    {
        /* Magic numbers */
        Vector2 trenchentranceposition = new Vector2(165, 50);
        int trenchentrancewidth = 50;
        int trenchentranceheight = 100;
        Vector2 columnPosition = new Vector2(265, 115);
        float xFactor = .94F;
        float yFactor = .34F; 

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
            // if in first section of ladder
            if (unitPosition.Y > 295)
            {
                if (!isTowardsTrench)
                {
                    result.X = unitPosition.X - (xFactor*speed);
                    result.Y = unitPosition.Y + (yFactor * speed);
                }
                else
                {
                    result.X = unitPosition.X + (xFactor*speed);
                    result.Y = unitPosition.Y - (yFactor * speed);
                }
            }
            // if in second section of ladder
            else if (unitPosition.Y > 185)
            {
                if (!isTowardsTrench)
                {
                    result.X = unitPosition.X + (xFactor * speed);
                    result.Y = unitPosition.Y + (yFactor * speed);
                }
                else
                {
                    result.X = unitPosition.X - (xFactor * speed);
                    result.Y = unitPosition.Y - (yFactor * speed);
                }
            }
            // if in third section of ladder
            else if (unitPosition.Y > 90)
            {
                if (!isTowardsTrench)
                {
                    result.X = unitPosition.X - (xFactor * speed);
                    result.Y = unitPosition.Y + (yFactor * speed);
                }
                else
                {
                    result.X = unitPosition.X + (xFactor * speed);
                    result.Y = unitPosition.Y - (yFactor * speed);
                }
            }
            // if on top platform
            else
            {
                if (!isTowardsTrench)
                {
                    if (unitPosition.X > 700)
                    {
                        result.X = unitPosition.X - (speed * xFactor);
                        result.Y = unitPosition.Y + (speed * yFactor);
                    }
                    else
                    {
                        result.X = unitPosition.X + speed;
                        result.Y = unitPosition.Y;
                    }
                }
                else
                {
                    result.X = unitPosition.X - speed;
                    result.Y = unitPosition.Y; 
                }
            }
            return result;
        }
        public override bool isForward(Vector2 unitPosition, bool isTowardsTrench, float speed)
        {
            if (unitPosition.Y > 295)
            {
                if (isTowardsTrench)
                {
                    return true; 
                }
                else
                {
                    return false; 
                }
            }
            // if in second section of ladder
            else if (unitPosition.Y > 185)
            {
                if (isTowardsTrench)
                {
                    return false; 
                }
                else
                {
                    return true; 
                }
            }
            // if in third section of ladder
            else if (unitPosition.Y > 90)
            {
                if (isTowardsTrench)
                {
                    return true; 
                }
                else
                {
                    return false; 
                }
            }
            // if on top platform
            else
            {
                if (isTowardsTrench)
                {
                    return false; 
                }
                else
                {
                    return true; 
                }
            }



        }
    }
}
