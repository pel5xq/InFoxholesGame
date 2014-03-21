using Microsoft.Xna.Framework;


namespace InFoxholes.Layouts
{
    public class Pather
    {
        public virtual Vector2 Move(Vector2 unitPosition, bool isTowardsTrench, float speed)
        {
            return Vector2.Zero;
        }

        public virtual bool atTrenchEntrance(Vector2 unitPosition, int unitWidth, int unitHeight)
        {
            return false;
        }
    }
}
