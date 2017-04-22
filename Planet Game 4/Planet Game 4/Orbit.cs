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
        SpaceBody Parent;
        /// <summary>
        /// Points from parent to apoapsis
        /// </summary>
        Vector EccentrcityVector;
        Vector Center;
        double FocalDist;
        double Eccentricity;
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
        double True_Anomoly;
        double Eccentric_Anomoly;
        double Mean_Anomoly;
        double Period;
        double MeanMotion;
        double Latus;

        public Orbit(SpaceBody Parent)
        {
            this.Parent = Parent;
            GM = Parent.mass * theGame.Gravity;
        }
        public void Generate(Vector Pos,Vector Vel)
        {
            double VelSq = Vel.MagSq();
            double Rad = Pos.Mag();
            EccentrcityVector = Pos * (VelSq/GM-1/Rad) - Vel * (Vel.Dot(Pos)/GM);
            Eccentricity = EccentrcityVector.Mag();
            Energy = VelSq / 2 - GM / Rad;
            SemiMajor = -GM / (2 * Energy);
            SemiMinor = SemiMajor * Math.Sqrt(1-Sq(Eccentricity));
            Latus = SemiMajor * (1 - Sq(Eccentricity));
            FocalDist = Math.Sqrt(Sq(SemiMajor)-Sq(SemiMinor));
            Center = EccentrcityVector.setMag(FocalDist);
            True_Anomoly = Pos.Angle();
            Period = 2 * Math.PI * Math.Sqrt(SemiMajor*SemiMajor*SemiMajor/GM);
            MeanMotion = Math.Sqrt(GM/(SemiMajor * SemiMajor * SemiMajor));
        }
        public void Generate(double Eccentricity,double OrbitAngle,double SemiMajor,double PlanetAngle)
        {
            this.Eccentricity = Eccentricity;
            EccentrcityVector = new Vector(Eccentricity*Math.Cos(OrbitAngle), Eccentricity * Math.Sin(OrbitAngle));
            this.SemiMajor = SemiMajor;
            SemiMinor = SemiMajor * Math.Sqrt(1 - Sq(Eccentricity));
            Latus = SemiMajor * (1 - Sq(Eccentricity));
            FocalDist = Math.Sqrt(Sq(SemiMajor) - Sq(SemiMinor));
            Center = EccentrcityVector.setMag(FocalDist);
            True_Anomoly = PlanetAngle;
            Period = 2 * Math.PI * Math.Sqrt(SemiMajor * SemiMajor * SemiMajor / GM);
            MeanMotion = Math.Sqrt(GM / (SemiMajor * SemiMajor * SemiMajor));
        }
        public Vector getPos()
        {
            double Rad = SemiMajor*(1 - Sq(Eccentricity))/(1+Eccentricity*Math.Cos(True_Anomoly));
            return (new Vector(Rad*Math.Cos(True_Anomoly), Rad*Math.Sin(True_Anomoly)).Rot(EccentrcityVector.Norm()))+Parent.position;
        }
        public Vector getPos(double Angle)
        {
            double Rad = SemiMajor * (1 - Sq(Eccentricity)) / (1 + Eccentricity * Math.Cos(Angle));
            return (new Vector(Rad * Math.Cos(Angle), Rad * Math.Sin(Angle)).Rot(EccentrcityVector.Norm())) + Parent.position;
        }
        public void update(double dt)
        {
            Mean_Anomoly -= MeanMotion * dt;
            Mean_Anomoly -= 0.01;
            Eccentric_Anomoly = getEccFromMean();
            True_Anomoly = getTrueFromEcc();
        }
        double getTrueFromEcc()
        {
            return 2*Math.Atan2(Math.Sqrt(1 + Eccentricity) * Math.Sin(Eccentric_Anomoly / 2), Math.Sqrt(1 - Eccentricity) * Math.Cos(Eccentric_Anomoly / 2));
        }
        double getEccFromMean()
        {
            double E = Mean_Anomoly;
            for (int i = 0; i < 10; i++)
            {
                E = Mean_Anomoly + Eccentricity * Math.Sin(E);
            }
            return E;
        }
        double getEccFromTrue()
        {
            double CT = Math.Cos(True_Anomoly);
            return Math.Acos((Eccentricity + CT) / (1 + Eccentricity * CT));
        }
        double getMeanFromEcc()
        {
            return Eccentric_Anomoly - Eccentricity * Math.Sin(Eccentric_Anomoly);
        }
        public void Show(Graphics G)
        {
            int OrbitLines = 10;
            for (int i = 0; i < OrbitLines; i++)
            {
                double Angle = i / (double)OrbitLines;

            }
        }
        double Sq(double A)
        {
            return A * A;
        }

    }
}
