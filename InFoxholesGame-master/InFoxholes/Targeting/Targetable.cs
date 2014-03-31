using Microsoft.Xna.Framework;


namespace InFoxholes.Targeting
{
    public interface Targetable
    {
        bool isHit(Vector2 crosshairPosition);
    }
}
