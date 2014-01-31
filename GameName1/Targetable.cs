using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameName1
{
    interface Targetable
    {
        bool isHit(Vector2 crosshairPosition);
    }
}
