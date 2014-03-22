using Microsoft.Xna.Framework;


namespace InFoxholes.Layouts
{
    public class TowerPather : Pather
    {
        /* Magic numbers */
        /*static int startY = 150;
        static int hillBottomX = 270;
        static int hillTopX = 210;
        static int ladderTopX = 122;
        static int hillBottomY = 145 + startY;
        static int hillTopY = 15 + startY;
        static int ladderTopY = 15 + startY;
        static float slope = (hillTopY - hillBottomY)/(hillTopX - hillBottomX);*/

        static int halfwayX = 320;
        Vector2 trenchentranceposition = new Vector2(320, 165);
        int trenchentrancewidth = 75;
        int trenchentranceheight = 60;

        public override void Initialize()
        {
            trenchEntrancePosition = trenchentranceposition;
            trenchEntranceWidth = trenchentrancewidth;
            trenchEntranceHeight = trenchentranceheight;
            base.Initialize();
        }
        public override Vector2 Move(Vector2 unitPosition, bool isTowardsTrench, float speed)
        {
            /*Vector2 result = new Vector2();
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
            return result;*/
            Vector2 result = new Vector2();
            float correctedSpeed;
            if (isForward(unitPosition, isTowardsTrench, speed)) correctedSpeed = speed;
            else correctedSpeed = -1 * speed;
            result.X = unitPosition.X + correctedSpeed;
            result.Y = unitPosition.Y;
            return result;
        }
        public override bool isForward(Vector2 unitPosition, bool isTowardsTrench, float speed)
        {
            if (isTowardsTrench) return unitPosition.X < halfwayX;
            else return unitPosition.X >= halfwayX;
        }
    }
}
