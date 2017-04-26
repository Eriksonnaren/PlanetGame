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
            Empty,
            Ice,
            Rock,
            Lava
        }
        public RingType Type;
        List<RingLayer> Layers=new List<RingLayer>();
        public int Diversity;
        public int LayerDiversity;
        bool visible = true;
        double OuterRadius;
        Color Color;
        public RingSystem(SpaceBody Parent, Color Color, int LayerAmount, int PieceAmount, double InnerRadius, double OuterRadius)
        {
            this.OuterRadius = OuterRadius;
            this.Parent = Parent;
            Diversity = 5;
            LayerDiversity = 25;
            this.Type = RingType.Empty;
            double Thickness = (OuterRadius - InnerRadius) / 2;
            Thickness /= LayerAmount;
            this.Color = Color;
            
            for (int i = 0; i < LayerAmount; i++)
            {
                double Dist = InnerRadius + 2 * i * Thickness;
                Layers.Add(new RingLayer(Dist, Color, Parent, this, Thickness * 1.1, PieceAmount));
            }
        }
        public RingSystem(SpaceBody Parent,RingType Type,int LayerAmount,int PieceAmount,double InnerRadius,double OuterRadius)
        {
            this.OuterRadius = OuterRadius;
            this.Parent = Parent;
            Diversity = 5;
            LayerDiversity = 25;
            this.Type = Type;
            double Thickness = (OuterRadius-InnerRadius)/2;
            Thickness /= LayerAmount;
            Color C;
            switch (Type)
            {
                case RingType.Ice:
                    C = Color.SkyBlue;
                    break;
                case RingType.Rock:
                    C = Color.SandyBrown;
                    break;
                case RingType.Lava:
                    C = Color.OrangeRed;
                    break;
                default:
                    C = Color.Black;
                    break;
            }
            Color = C;
            for (int i = 0; i < LayerAmount; i++)
            {
                double Dist = InnerRadius + 2 * i * Thickness;
                Layers.Add(new RingLayer(Dist,C,Parent,this,Thickness*1.1,PieceAmount));
            }
        }
        public void update(theGame parent, double dt)
        {
            
            if (Form1.isInsideWindow(Form1.ui.worldToPixel(Parent.position),parent.worldDispMin,parent.worldDispMax,OuterRadius))
            {
                visible = true;
                foreach (RingLayer L in Layers)
                {
                    
                    L.update(dt);
                }
            }
            else
                visible = false;
        }
        public void refresh(RingType Type)
        {
            this.Type = Type;
            refresh();
        }
        public void refresh()
        {
            Color C;
            switch (Type)
            {
                case RingType.Ice:
                    C = Color.SkyBlue;
                    break;
                case RingType.Rock:
                    C = Color.SandyBrown;
                    break;
                case RingType.Lava:
                    C = Color.OrangeRed;
                    break;
                default:
                    C = Color.Black;
                    break;
            }
            foreach (RingLayer L in Layers)
            {
                L.refresh(C);
            }
        }
        public void show(Graphics g)
        {
            if(visible)
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
        public Color Color;
        int ColorOffset;
        public RingLayer(double Radius, Color Color, SpaceBody ParentBody, RingSystem ParentSystem, double Thickness,int PieceAmount)
        {
            RingAmount = PieceAmount;
            ColorOffset = Form1.rnd.Next(-ParentSystem.LayerDiversity, ParentSystem.LayerDiversity);
            this.Color = Color.FromArgb(newC(Color.A), newC(Color.R), newC(Color.G), newC(Color.B));
            this.Thickness = Thickness;
            this.Radius = Radius;
            this.ParentBody = ParentBody;
            double GM = ParentBody.mass * theGame.Gravity;
            OrbitSpeed = 1/ Math.Sqrt(Radius * Radius * Radius / GM);
            Angle = -Form1.rnd.NextDouble();
            Pieces = new RingPiece[RingAmount];
            PosInside = new Vector[RingAmount + 1];
            PosOutside = new Vector[RingAmount + 1];
            for (int i = 0; i < RingAmount; i++)
            {
                Pieces[i] = new RingPiece(this.Color,ParentSystem.Diversity);
            }
        }
        
        public void update(double dt)
        {
            Angle -= OrbitSpeed*dt;
            Angle = Angle % (Math.PI*2);
        }
        public void refresh(Color Color)
        {
            this.Color = Color.FromArgb(newC(Color.A), newC(Color.R), newC(Color.G), newC(Color.B));
            for (int i = 0; i < RingAmount; i++)
            {
                Pieces[i].refresh(this.Color);
            }
        }
        public void show(Graphics g)
        {
            for (int i = 0; i < RingAmount; i++)
            {
                double Angle = this.Angle + 2 * Math.PI * i / RingAmount;
                double Rad = Radius - Thickness;
                PosInside[i] = Form1.ui.worldToPixel(new Vector(Rad * Math.Cos(Angle), Rad * Math.Sin(Angle)) + ParentBody.position);
                Rad = Radius + Thickness;
                PosOutside[i] = Form1.ui.worldToPixel(new Vector(Rad * Math.Cos(Angle), Rad * Math.Sin(Angle)) + ParentBody.position);
            }
            PosInside[RingAmount] = PosInside[0];
            PosOutside[RingAmount] = PosOutside[0];
            for (int i = 0; i < RingAmount; i++)
            {
                PointF[] P = new PointF[] {
                PosInside[i+1].toPoint(),
                PosInside[i].toPoint(),
                PosOutside[i].toPoint(),
                PosOutside[i + 1].toPoint(),
                };
                g.FillPolygon(new SolidBrush(Pieces[i].Color), P);
            }
        }

        float toDeg(double Rad)
        {
            return (float)(180 * Rad / Math.PI);
        }
        int newC(int C)
        {
            return Math.Min(Math.Max(C + ColorOffset, 0), 255);
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
