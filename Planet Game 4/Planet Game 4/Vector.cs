using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Planet_Game_4
{
    public class Vector
    {
        public double X;
        public double Y;

        public Vector()
        {
            X = Y = 0;
        }

        public Vector(double X,double Y)
        {
            this.X = X;
            this.Y = Y;
        }
        public Vector(Vector V)
        {
            X = V.X;
            Y = V.Y;
        }
        public Vector(Point V)
        {
            X = V.X;
            Y = V.Y;
        }
        static public Vector operator +(Vector V1,Vector V2)
        {
            return new Vector(V1.X + V2.X, V1.Y+V2.Y);
        }
        static public Vector operator -(Vector V1, Vector V2)
        {
            return new Vector(V1.X - V2.X, V1.Y - V2.Y);
        }
        static public Vector operator -(Vector V1)
        {
            return new Vector(-V1.X, -V1.Y);
        }
        static public Vector operator *(Vector V, double M)
        {
            return new Vector(V.X*M,V.Y*M);
        }
        static public Vector operator /(Vector V, double M)
        {
            return new Vector(V.X / M, V.Y / M);
        }
        public double MagSq()
        {
            return X * X + Y * Y;
        }
        public double Mag()
        {
            return Math.Sqrt(MagSq());
        }
        public Vector Norm()
        {
            if (X == 0 && Y == 0)
                return new Vector(1, 0);
            return this / Mag();
        }
        public Vector setMag(double M)
        {
            return Norm() * M;
        }
        public double Dot(Vector V)
        {
            return X * V.X + Y * V.Y;
        }
        public double Angle()
        {
            return Math.Atan2(Y,X);
        }
        public Vector Rot(Vector V)
        {
            return new Vector(X * V.X - Y * V.Y, Y * V.X + X * V.Y);
        }
        public Vector RotRev(Vector V)
        {
            return new Vector(X * V.X + Y * V.Y, Y * V.X - X * V.Y);
        }
        public Vector lerp(Vector V,double t)
        {
            return new Vector(Form1.lerp(X,V.X,t), Form1.lerp(Y, V.Y, t));
        }
        public PointF toPoint()
        {
            return new PointF((float)X, (float)Y);
        }
    }
}
