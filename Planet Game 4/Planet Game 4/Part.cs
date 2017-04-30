using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Planet_Game_4
{
    interface Part
    {
        Vector Pos { get; set; }
        Vector Hitbox { get; }
        void Update();
        void Show(Graphics G,PointF Pos, float Z, float Angle);
    }
}
