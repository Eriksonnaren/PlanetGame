using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Planet_Game_4
{
    public class Orbit
    {
        ISpace_bodies Parent;
        /// <summary>
        /// Points from parent to apoapsis
        /// </summary>
        Vector EccentrcityVector;
        Vector Center;
        double FocalDist;
        double Eccentrcity;
        /// <summary>
        /// Longest distance
        /// </summary>
        double Apoapsis;
        /// <summary>
        /// Closest distance
        /// </summary>
        double Periapsis;
        double SemiMajor;
        double SemiMinor;
        /// <summary>
        /// Clockwise if false
        /// </summary>
        bool Prograde;
        /// <summary>
        /// Gravity*Mass
        /// </summary>
        double GM;
        double Energy;

        public Orbit(ISpace_bodies Parent)
        {
            this.Parent = Parent;
            GM = Parent.mass * theGame.Gravity;
        }
        public void Generate(Vector Pos,Vector Vel)
        {
            double VelSq = Vel.MagSq();
            double Rad = Pos.Mag();
            EccentrcityVector = Pos * (VelSq/GM-1/Rad) - Vel * (Vel.Dot(Pos)/GM);
            Eccentrcity = EccentrcityVector.Mag();
            Energy = VelSq / 2 - GM / Rad;
            SemiMajor = -GM / (2 * Energy);
            SemiMinor = SemiMajor * Math.Sqrt(1-Eccentrcity*Eccentrcity);
            FocalDist = Math.Sqrt(SemiMajor*SemiMajor-SemiMinor*SemiMinor);
            Center = EccentrcityVector.setMag(FocalDist);

        }
        public void Show(Graphics G)
        {
            G.TranslateTransform((float)Parent.position.X, (float)Parent.position.Y);
            G.RotateTransform((float)(EccentrcityVector.Angle() * 180 /Math.PI+180));
            G.DrawEllipse(new Pen(Color.Blue,2),(float)(FocalDist-SemiMajor),(float)(-SemiMinor),(float)SemiMajor*2,(float)SemiMinor*2);
            G.ResetTransform();
        }

    }
}
