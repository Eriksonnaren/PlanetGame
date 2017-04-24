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
        ///<summary>
        /// The x position stored in the vector
        ///</summary>
        public double X;
        ///<summary>
        /// The y position stored in the vector
        ///</summary>
        public double Y;

        ///<summary>
        /// This constructor literally just makes the object. Nothing to pass in.
        ///</summary>
        public Vector()
        {
         
        }

        ///<summary>
        /// This constructor just copies the paramaters to the values inside the vector
        ///</summary>
        public Vector(double X,double Y)
        {
            this.X = X;
            this.Y = Y;
        }
        ///<summary>
        /// This constructor copies another vector
        ///</summary>
        public Vector(Vector V)
        {
            X = V.X;
            Y = V.Y;
        }
        ///<summary>
        /// This constructor copies the values from a point. TODO: do the same thing with pointF
        ///</summary>
        public Vector(Point V)
        {
            X = V.X;
            Y = V.Y;
        }
        
        ///<summary>
        /// This is what happens when you add two vectors together.
        ///</summary>
        static public Vector operator +(Vector V1,Vector V2)
        {
            // It just returns the sum of the two vectors. Straight-forward!
            return new Vector(V1.X + V2.X, V1.Y+V2.Y);
        }
        
        ///<summary>
        /// This is what happens when you subtract two vector
        ///</summary>
        static public Vector operator -(Vector V1, Vector V2)
        {
            // It just returns the difference of the two vectors. Again, straight-forward
            return new Vector(V1.X - V2.X, V1.Y - V2.Y);
        }
        
        ///<summary>
        /// This is what happens when you invert the sign of a vector
        ///</summary>
        static public Vector operator -(Vector V1)
        {
            // It literally just inverts the x and y property. This is too straight-forward!
            return new Vector(-V1.X, -V1.Y);
        }
        
        ///<summary>
        /// This scales a vector by a M
        ///</summary>
        static public Vector operator *(Vector V, double M)
        {
            // The x and y values get multiplied by M
            return new Vector(V.X*M,V.Y*M);
        }
        
        ///<summary>
        /// This scales a vector by 1/M
        ///</summary>
        static public Vector operator /(Vector V, double M)
        {
            // Just divides the x and y values of the vector by M
            return new Vector(V.X / M, V.Y / M);
        }
        
        ///<summary>
        /// Returns the magnitude^2. This is more efficient than just the magnitude, since it doesn't need a sqrt()
        ///</summary>
        public double MagSq()
        {
            // Using phytagoras without the sqrt to make it squared
            return X * X + Y * Y;
        }
        
        /// <summary>
        /// Returns the magnitude of the vector.
        /// </summary>
        public double Mag()
        {
            // Simply phytagoras
            return Math.Sqrt(X * X + Y * Y); // Note to Erik: I think it's more messy to make a function call instead of just doing the humble math once again. It's not much math, anyway.
        }
        
        /// <summary>
        /// Makes the vectors magnitude one.
        /// </summary>
        public Vector Norm()
        {
            // If the x and y values are 0, return a simple vector. This is to avoid divide by zero error
            if (X == 0 && Y == 0)
                return new Vector(1, 0);
            
            // Return the normalized version of this vector.
            return this / Mag();
        }
        
        /// <summary>
        /// Sets the magnitude of the vector to M
        /// </summary>
        public Vector setMag(double M)
        {
            // Simply normalize to length 1 and then scale by M. Simple and elegant :D
            return Norm() * M;
        }
        
        /// <summary>
        /// Returns the dot product of the Vector
        /// </summary>
        public double Dot(Vector V)
        {
            return X * V.X + Y * V.Y;
        }
        
        /// <summary>
        /// Returns the angle of the vector
        /// </summary>
        public double Angle()
        {
            return Math.Atan2(Y,X);
        }
        
        /// <summary>
        /// Rotates the vector (The input is a vector with the x being cos(angle) and the y being sin(angle))
        /// </summary>
        public Vector Rot(Vector V)
        {
            return new Vector(X*V.X - Y * V.Y, Y * V.X + X * V.Y);
        }
        
        /// <summary>
        /// Rotates the vector with an angle as paramater
        /// </summary>
        public Vector Rot(double rotation)
        {
            Rot(new Vector(Math.Cos(rotation), Math.Sin(rotation)));
        }
        
        /// <summary>
        /// Gives a vector that is lerped between this and the paramater vector
        /// </summary>
        public Vector lerp(Vector V,double t)
        {
            return new Vector(Form1.lerp(X,V.X,t), Form1.lerp(Y, V.Y, t));
        }
        
        /// <summary>
        /// Returns a pointF point with the same x and y values as the vector
        /// </summary>
        public PointF toPoint()
        {
            return new PointF((float)X, (float)Y);
        }
    }
}
