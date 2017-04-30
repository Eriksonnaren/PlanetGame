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
        public RingType Type=RingType.Empty;
        List<RingLayer> Layers=new List<RingLayer>();
        public int Diversity=30;
        public int LayerDiversity=80;
        int LayerAmount;
        int PieceAmount;
        bool visible = true;
        public double OuterRadius;
        double Thickness;
        public double InnerRadius;
        Color Color;
        Noise LayerNoise=new Noise(4,2,0.5,0);
        public RingSystem(SpaceBody Parent, Color Color, int LayerAmount, int PieceAmount, double InnerRadius, double OuterRadius)
        {
            this.LayerAmount = LayerAmount;
            this.OuterRadius = OuterRadius;
            this.InnerRadius = InnerRadius;
            this.PieceAmount = PieceAmount;
            this.Parent = Parent;
            double Thickness = (OuterRadius - InnerRadius) / 2;
            this.Thickness = Thickness/LayerAmount;
            this.Color = Color;
            generate();
        }
        public RingSystem(SpaceBody Parent,RingType Type,int LayerAmount,int PieceAmount,double InnerRadius,double OuterRadius)
        {
            this.LayerAmount = LayerAmount;
            this.OuterRadius = OuterRadius;
            this.InnerRadius = InnerRadius;
            this.PieceAmount = PieceAmount;
            this.Parent = Parent;
            double Thickness = (OuterRadius - InnerRadius) / 2;
            this.Thickness =Thickness / LayerAmount;
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
            generate();
        }
        public void generate()
        {
            for (int i = 0; i < LayerAmount; i++)
            {
                double Dist = InnerRadius + 2 * i * Thickness;
                Layers.Add(new RingLayer(Dist, Color, Parent, this, Thickness * 1.1, PieceAmount,(int)(LayerDiversity*LayerNoise.Get(i/(float)LayerAmount))));
            }
        }
        public void update(universeCam parent, double dt)
        {
            
            if (Form1.isInsideWindow(parent.worldToPixel(Parent.position),parent.section.min,parent.section.max,OuterRadius))

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
        public void show(Graphics g, universeCam parent)
        {
            if(visible)
            foreach (RingLayer L in Layers)
            {
                L.show(g, parent);
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
        Noise PieceNoise = new Noise(20,4,0.5,0);
        public RingLayer(double Radius, Color Color, SpaceBody ParentBody, RingSystem ParentSystem, double Thickness,int PieceAmount,int ColorOffset)
        {
            RingAmount = PieceAmount;
            this.ColorOffset = ColorOffset;
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
                Pieces[i] = new RingPiece(this.Color,(int)(ParentSystem.Diversity*PieceNoise.Get(i/(double)RingAmount)));
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
        public void show(Graphics g, universeCam parent)
        {
            for (int i = 0; i < RingAmount; i++)
            {
                double Angle = this.Angle + 2 * Math.PI * i / RingAmount;
                double Rad = Radius - Thickness;
                PosInside[i] = parent.worldToPixel(new Vector(Rad * Math.Cos(Angle), Rad * Math.Sin(Angle)) + ParentBody.position);
                Rad = Radius + Thickness;
                PosOutside[i] = parent.worldToPixel(new Vector(Rad * Math.Cos(Angle), Rad * Math.Sin(Angle)) + ParentBody.position);
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
        public RingPiece(Color Color,int ColorOffset)
        {
            this.ColorOffset = ColorOffset;
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
