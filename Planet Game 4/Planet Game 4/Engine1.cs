using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Planet_Game_4
{
    class Engine1:Part
    {
        EngineType Type;
        public Vector Pos { get; set; }
        public Vector Hitbox { get { return new Vector(1, 1); } }
        public Grid Parent;
        public float Angle;
        public Engine1()
        {
            Type = new EngineType();
        }
        public void Update()
        {
            
        }
        public void Show(Graphics G,PointF Pos,float Z,float Angle)
        {
            Matrix M=G.Transform;
            G.TranslateTransform(Pos.X,Pos.Y);
            G.RotateTransform(Angle);

            G.FillRectangle(Brushes.LightGray,-Z,-Z,2*Z,Z);
            PointF[] P = new PointF[] {
                new PointF(-Z*0.3F,0),
                new PointF(Z*0.3F,0),
                new PointF(Z,Z),
                new PointF(-Z,Z),
            };
            G.FillPolygon(Brushes.Gray,P);
            G.Transform = M;
        }
    }
}
