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
        space_bodies Parent;
        /// <summary>
        /// Points from parent to apoapsis
        /// </summary>
        Vector EccentrcityVector;
        Vector Center;
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

        public Orbit(space_bodies Parent)
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

        }
        public void Show(Graphics G)
        {
            
        }

    }
}
