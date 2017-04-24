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
            Top = Parent.radius;
            Bottom = -Parent.radius;
        }
        public void CheckPlanets(List<SpaceBody> System)
        {
            Vector MinDist = null;
            SpaceBody Closest=null;
            for (int i = 0; i < System.Count; i++)
            {
                SpaceBody S = System[i];
                if(S!=Parent)
                {
                    Vector Dist = intersectPlanet(S);
                    if(Dist!=null)
                    {
                        if(MinDist==null||Dist.X<MinDist.X)
                        {
                            MinDist = Dist;
                            Closest = S;
                        }
                    }
                }
            }
            if (Closest != null)
            {
                End = MinDist.X;
                Closest.Shadow.refresh(MinDist.Y,this);
            }
            else
                End = 0;
        }
        public Vector intersectPlanet(SpaceBody S)
        {
            double Dist = S.radius;
            Vector V = S.position;
            V -= Parent.position;
            V = V.RotRev(Angle);
            V += Parent.position;
            bool intersect = (V.X > Parent.position.X && V.Y - Parent.position.Y < Top + Dist && V.Y - Parent.position.Y > Bottom - Dist);
            if (intersect)
            {
                return V - Parent.position;
            }
            else
                return null;
        }
        public void refresh(double D,Shadow parent)
        {
            Top = Math.Max(Top, parent.Top-D);
            Bottom = Math.Min(Bottom, parent.Bottom-D);
        }
        public bool intersect(Vector V)
        {
            V-= Parent.position;
            V=V.RotRev(Angle);
            V += Parent.position;
            if(End>0)
                return V.X > Parent.position.X && V.X < Parent.position.X + End && V.Y - Parent.position.Y < Top && V.Y-Parent.position.Y > Bottom;
            else
                return V.X > Parent.position.X&& V.Y - Parent.position.Y < Top && V.Y - Parent.position.Y > Bottom;
        }
        
        public bool intersect(Vector V,double Dist)
        {
            V -= Parent.position;
            V = V.RotRev(Angle);
            V += Parent.position;
            if(End>0)
                return (V.X > Parent.position.X && V.X < Parent.position.X + End && V.Y - Parent.position.Y < Top+Dist && V.Y - Parent.position.Y > Bottom-Dist);
            else
                return (V.X > Parent.position.X && V.Y - Parent.position.Y < Top + Dist && V.Y - Parent.position.Y > Bottom - Dist);
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
            if (End > 0)
                maxDist = Math.Min(maxDist, End * game.zoom + Pos.X);
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
