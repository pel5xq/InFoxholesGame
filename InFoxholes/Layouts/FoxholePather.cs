using Microsoft.Xna.Framework;


namespace InFoxholes.Layouts
{
    public class FoxholePather : Pather
    {
        /* Magic numbers */
        static int startY = 150;
        static int hillBottomX = 270;
        static int hillTopX = 210;
        static int ladderTopX = 120;
        static int ladderTopY = 175;
        static int hillBottomY = 145 + startY;
        static int hillTopY = 15 + startY;
        static float slope = (hillTopY - hillBottomY)/(hillTopX - hillBottomX);
        Vector2 trenchentranceposition = new Vector2(ladderTopX, ladderTopY);
        int trenchentrancewidth = 10;
        int trenchentranceheight= 100;

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
                result.Y = unitPosition.Y;
            }
            return result;
        }
        public override bool isForward(Vector2 unitPosition, bool isTowardsTrench, float speed)
        {
            return !isTowardsTrench;
        }
    }
}
