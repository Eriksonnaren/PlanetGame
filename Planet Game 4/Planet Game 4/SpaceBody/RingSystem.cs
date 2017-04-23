using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Planet_Game_4
{
    public class RingSystem
    {
        public SpaceBody Parent;
        public enum RingType
        {
            ice,
            rock
        }
        RingType Type;
        List<RingLayer> Layers=new List<RingLayer>();
        public int Diversity;
        public RingSystem(SpaceBody Parent,RingType Type)
        {
            Diversity = 4;
            this.Type = Type;
            double Dist = Parent.radius * 2;
            double Thickness = Parent.radius * 0.5;
            int amount = 15;
            Thickness /= amount;
            for (int i = 0; i < amount; i++)
            {
                Layers.Add(new RingLayer(Dist,Color.SkyBlue,Parent,this,Thickness));
                Dist += Thickness * (1.75);// +Form1.rnd.NextDouble()*0.5);
            }
        }
        public void update()
        {
            foreach (RingLayer L in Layers)
            {
                L.update(50);
            }
        }
        public void refresh(Color C)
        {
            foreach (RingLayer L in Layers)
            {
                L.refresh(C);
            }
        }
        public void show(Graphics g)
        {
            foreach (RingLayer L in Layers)
            {
                L.show(g);
            }
        }
    }
    class RingLayer
    {

        SpaceBody ParentBody;
        RingSystem ParentSystem;
        RingPiece[] Pieces;
        Vector[] PosInside;
        Vector[] PosOutside;
        int RingAmount;
        double OrbitSpeed;//in rad/tick
        double Angle;
        double Radius;
        double Thickness;
        public RingLayer(double Radius, Color Color, SpaceBody ParentBody, RingSystem ParentSystem, double Thickness)
        {
            this.Thickness = Thickness;
            RingAmount = 100;
            this.Radius = Radius;
            this.ParentBody = ParentBody;
            double GM = ParentBody.mass * theGame.Gravity;
            double Period = 2 * Math.PI * Math.Sqrt(Radius * Radius * Radius / GM);
            OrbitSpeed = 1 / Period;
            Pieces = new RingPiece[RingAmount];
            PosInside = new Vector[RingAmount + 1];
            PosOutside = new Vector[RingAmount + 1];
            for (int i = 0; i < RingAmount; i++)
            {
                Pieces[i] = new RingPiece(Color,ParentSystem.Diversity);
            }
        }
        
        public void update(double dt)
        {
            Angle -= OrbitSpeed*dt;
            for (int i = 0; i < RingAmount; i++)
            {
                double Angle = this.Angle+2 * Math.PI * i / RingAmount;
                double Rad = Radius - Thickness;
                PosInside[i] = Form1.ui.worldToPixel(new Vector(Rad * Math.Cos(Angle), Rad * Math.Sin(Angle))+ParentBody.position);
                Rad = Radius + Thickness;
                PosOutside[i] = Form1.ui.worldToPixel(new Vector(Rad * Math.Cos(Angle), Rad * Math.Sin(Angle)) + ParentBody.position);
            }
            PosInside[RingAmount] = PosInside[0];
            PosOutside[RingAmount] = PosOutside[0];
        }
        public void refresh(Color C)
        {
            for (int i = 0; i < RingAmount; i++)
            {
                Pieces[i].refresh(C);
            }
        }
        public void show(Graphics g)
        {
            for (int i = 0; i < RingAmount; i++)
            {
                PointF[] P = new PointF[] {
                PosInside[i+1].toPoint(),
                PosInside[i].toPoint(),
                PosOutside[i].toPoint(),
                PosOutside[i + 1].toPoint(),
                };
                g.FillPolygon(new SolidBrush(Pieces[i].Color),P);
            }
        }
    }
    class RingPiece
    {
        public Color Color;
        public int ColorOffset;
        public RingPiece(Color Color,int Div)
        {
            ColorOffset = Form1.rnd.Next(-Div,Div);
            this.Color=Color.FromArgb(newC(Color.A), newC(Color.R), newC(Color.G), newC(Color.B));
        }
        public void refresh(Color Color)
        {
            this.Color = Color.FromArgb(newC(Color.A), newC(Color.R), newC(Color.G), newC(Color.B));
        }
        int newC(int C)
        {
            return Math.Min(Math.Max(C + ColorOffset, 0), 255);
        }
    }
}
