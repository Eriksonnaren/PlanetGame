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
        double True_Anomoly;
        double Eccentric_Anomoly;
        double Mean_Anomoly;
        double Period;
        double MeanMotion;
        double Latus;

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
            SemiMinor = SemiMajor * Math.Sqrt(1-Sq(Eccentrcity));
            Latus = SemiMajor * (1 - Sq(Eccentrcity));
            FocalDist = Math.Sqrt(Sq(SemiMajor)-Sq(SemiMinor));
            Center = EccentrcityVector.setMag(FocalDist);
            True_Anomoly = Pos.Angle();
            //Eccentric_Anomoly = getEccFromTrue();
            //Mean_Anomoly = getMeanFromEcc();
            //Period = 2 * Math.PI * Math.Sqrt(SemiMajor*SemiMajor*SemiMajor/GM);
            MeanMotion = Math.Sqrt(GM/(SemiMajor * SemiMajor * SemiMajor));

        }
        public Vector getPos()
        {
            double Rad = SemiMajor*(1 - Sq(Eccentrcity))/(1+Eccentrcity*Math.Cos(True_Anomoly));
            return new Vector(Rad*Math.Cos(True_Anomoly), Rad*Math.Sin(True_Anomoly)).Rot(EccentrcityVector.Norm());
        }
        public void update(double dt)
        {
            Mean_Anomoly += MeanMotion * dt;
            Eccentric_Anomoly = getEccFromMean();
            True_Anomoly = getTrueFromEcc();
        }
        double getTrueFromEcc()
        {
            return 2*Math.Atan2(Math.Sqrt(1 + Eccentrcity) * Math.Sin(Eccentric_Anomoly / 2), Math.Sqrt(1 - Eccentrcity) * Math.Cos(Eccentric_Anomoly / 2));
        }
        double getEccFromMean()
        {
            double E = Mean_Anomoly;
            for (int i = 0; i < 10; i++)
            {
                E = Mean_Anomoly + Eccentrcity * Math.Sin(E);
            }
            return E;
        }
        double getEccFromTrue()
        {
            double CT = Math.Cos(True_Anomoly);
            return Math.Acos((Eccentrcity + CT) / (1 + Eccentrcity * CT));
        }
        double getMeanFromEcc()
        {
            return Eccentric_Anomoly - Eccentrcity * Math.Sin(Eccentric_Anomoly);
        }
        public void Show(Graphics G)
        {
            G.TranslateTransform((float)Parent.position.X, (float)Parent.position.Y);
            G.RotateTransform((float)(EccentrcityVector.Angle() * 180 /Math.PI));
            G.DrawEllipse(new Pen(Color.Blue,2),(float)(-FocalDist-SemiMajor),(float)(-SemiMinor),(float)SemiMajor*2,(float)SemiMinor*2);
            G.ResetTransform();
        }
        double Sq(double A)
        {
            return A * A;
        }

    }
}
