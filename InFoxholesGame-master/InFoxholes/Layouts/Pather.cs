using Microsoft.Xna.Framework;


namespace InFoxholes.Layouts
{
    public class Pather
    {
        public Vector2 trenchEntrancePosition;
        public int trenchEntranceWidth;
        public int trenchEntranceHeight;


        public virtual void Initialize()
        {

        }
        public virtual Vector2 Move(Vector2 unitPosition, bool isTowardsTrench, float speed)
        {
            return Vector2.Zero;
        }
        public virtual bool isForward(Vector2 unitPosition, bool isTowardsTrench, float speed)
        {
            return true;
        }
        public virtual bool atTrenchEntrance(Vector2 unitPosition, int unitWidth, int unitHeight)
        {
            return intersectsWith(unitPosition, unitWidth, unitHeight, trenchEntrancePosition, trenchEntranceWidth,trenchEntranceHeight);
        }
        public virtual bool intersectsWith(Vector2 obj1Position, int obj1Width, int obj1Height,
            Vector2 obj2Position, int obj2Width, int obj2Height)
        {
            Vector2 rightMid1 = new Vector2(obj1Position.X + obj1Width, obj1Position.Y + obj1Height/2);
            Vector2 leftMid1 = new Vector2(obj1Position.X, obj1Position.Y + obj1Height / 2);

            if (rightMid1.X >= obj2Position.X &&
                rightMid1.Y >= obj2Position.Y &&
                rightMid1.X <= obj2Position.X + obj2Width &&
                rightMid1.Y <= obj2Position.Y + obj2Height)
            {
                return true;
            }
            if (leftMid1.X >= obj2Position.X &&
                leftMid1.Y >= obj2Position.Y &&
                leftMid1.X <= obj2Position.X + obj2Width &&
                leftMid1.Y <= obj2Position.Y + obj2Height)
            {
                return true;
            }
            return false;
        }
    }
}
