using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Planet_Game_4
{
    public class Shadow
    {
        SpaceBody Parent;
        Vector Angle;
        double Top;
        double Bottom;
        Vector Start;
        double End;
        public Shadow(SpaceBody Parent)
        {
            this.Parent = Parent;
        }
        public void update()
        {
            SpaceBody Sun = Parent;
            while (Sun.type!= SpaceBody.Body_type.sun)
            {
                Sun = Sun.orbit.Parent;
            }
            Angle = (Parent.position - Sun.position).Norm();
            Start = Parent.position;
            End = 0;
            Top = Parent.radius;
            Bottom = -Parent.radius;
            
        }
        public void refresh()
        {

        }
        public bool intersect(Vector V)
        {
            return false;
        }
        public void show(Graphics g,Form1 form,theGame game)
        {
            Vector Pos = game.worldToPixel(Parent.position);
            Vector C = new Vector(form.Width / 2, form.Height / 2);
            Vector C2 = (C - Pos).RotRev(Angle).RotRev(game.camRotation) + Pos;
            int Alpha = 200;
            Vector[] Corners = new Vector[4];
            
            double Dist = Math.Max(form.Width, form.Height);
            double maxDist = C2.X + Dist;
            double minDist = Math.Max(C2.X - Dist,Pos.X);
            double T = Top * game.zoom + Pos.Y;
            double B = Bottom * game.zoom + Pos.Y;
            if (minDist < maxDist)//&&(Math.Abs(T - C.Y) <Dist||Math.Abs(B - C.Y)<Dist))
            {
                Corners[0] = new Vector(minDist, T);
                Corners[1] = new Vector(maxDist, T);
                Corners[2] = new Vector(maxDist, B);
                Corners[3] = new Vector(minDist, B);
                PointF[] Points = new PointF[4];
                for (int i = 0; i < 4; i++)
                {
                    Vector V = Corners[i];
                    V=V - Pos;
                    V=V.Rot(Angle).Rot(game.camRotation);
                    V = V + Pos;

                    Points[i] = (V).toPoint();
                }
                g.FillPolygon(new SolidBrush(Color.FromArgb(Alpha, Color.Black)), Points);
            }
        }
    }
}
