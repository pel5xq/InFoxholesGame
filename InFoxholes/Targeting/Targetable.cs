using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using InFoxholes;
using InFoxholes.Enemies;
using InFoxholes.Friendlies;
using InFoxholes.Looting;
using InFoxholes.Targeting;
using InFoxholes.Util;
using InFoxholes.Waves;
using InFoxholes.Weapons;
using InFoxholes.Windows;

namespace InFoxholes.Targeting
{
    public interface Targetable
    {
        bool isHit(Vector2 crosshairPosition);
    }
}
