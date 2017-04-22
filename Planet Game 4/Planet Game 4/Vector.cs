﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planet_Game_4
{
    class Vector
    {
        double X;
        double Y;
        public Vector()
        {

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
        static public Vector operator +(Vector V1,Vector V2)
        {
            return new Vector(V1.X + V2.X, V1.Y+V2.Y);
        }
        static public Vector operator -(Vector V1, Vector V2)
        {
            return new Vector(V1.X - V2.X, V1.Y - V2.Y);
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
            return this / Mag();
        }
        public double Dot(Vector V)
        {
            return X * V.X + Y * V.Y;
        }
        public double Angle()
        {
            return Math.Atan2(Y,X);
        }
    }
}
