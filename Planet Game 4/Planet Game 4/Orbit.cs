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
        public SpaceBody Parent;
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
        double Radius;

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
        public void Generate(double Eccentricity,double OrbitAngle,double SemiMajor,double PlanetAngle,bool Prograde)
        {
            this.Prograde = Prograde;
            this.Eccentricity = Eccentricity;
            EccentrcityVector = new Vector(Eccentricity*Math.Cos(OrbitAngle), Eccentricity * Math.Sin(OrbitAngle));
            this.SemiMajor = SemiMajor;
            SemiMinor = SemiMajor * Math.Sqrt(1 - Sq(Eccentricity));
            Latus = SemiMajor * (1 - Sq(Eccentricity));
            FocalDist = Math.Sqrt(Sq(SemiMajor) - Sq(SemiMinor));
            Center = EccentrcityVector.setMag(FocalDist);
            Mean_Anomoly = PlanetAngle;
            Period = 2 * Math.PI * Math.Sqrt(SemiMajor * SemiMajor * SemiMajor / GM);
            MeanMotion = Math.Sqrt(GM / (SemiMajor * SemiMajor * SemiMajor));
        }
        public Vector getVel()
        {
            double v = Math.Sqrt(GM*(2/Radius-1/SemiMajor));
            Vector Angle = new Vector(SemiMajor * Math.Sin(True_Anomoly), -SemiMinor * Math.Cos(True_Anomoly));
            return Angle.Norm()*v;
        }
        public Vector getPos()
        {
            double Rad = Radius;
            return (new Vector(Rad*Math.Cos(True_Anomoly), Rad*Math.Sin(True_Anomoly)).Rot(EccentrcityVector.Norm()))+Parent.position;
        }
        public double getRadius()
        {
            return SemiMajor * (1 - Sq(Eccentricity)) / (1 + Eccentricity * Math.Cos(True_Anomoly));
        }
        public Vector getPos(double Angle)
        {
            Angle += True_Anomoly;
            double Rad = SemiMajor * (1 - Sq(Eccentricity)) / (1 + Eccentricity * Math.Cos(Angle));
            return (new Vector(Rad * Math.Cos(Angle), Rad * Math.Sin(Angle)).Rot(EccentrcityVector.Norm())) + Parent.position;
        }
        public void update(double dt)
        {
            if (Prograde)
                dt = -dt;
            Mean_Anomoly += MeanMotion * dt;
            //Mean_Anomoly -= 0.01;
            Eccentric_Anomoly = getEccFromMean();
            True_Anomoly = getTrueFromEcc();
            Radius = getRadius();
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
        public void Show(Graphics G, theGame parent, Pen Pen,bool Fade)
        {
            // If you do not need to show the orbit, don't show it!
            if(Periapsis * parent.zoom > parent.camOrigin.Mag() / 2)
            {
                return;
            }

            int OrbitLines = 200;
            PointF[] Points = new PointF[OrbitLines+1];
            bool[] canSee = new bool[OrbitLines + 1];
            for (int i = 0; i < OrbitLines; i++)
            {
                double Angle = 2*Math.PI*i / OrbitLines;
                Vector V = Form1.ui.worldToPixel(getPos(Angle));
                canSee[i] = Form1.isInsideWindow(V,0);
                Points[i] = V.toPoint();
            }
            Points[OrbitLines] = Points[0];
            canSee[OrbitLines] = canSee[0];
            if (Fade)
            {
                Color Col = Pen.Color;
                Color B = Color.Black;
                double FadeMin = 0.2;
                for (int i = 1; i < OrbitLines + 1; i++)
                {

                    Pen.Color = Form1.lerpC(B, Col, (1 - FadeMin) * (1 - ((i - 1) / (double)OrbitLines)) + FadeMin);
                    if(canSee[i]|| canSee[i-1])
                        G.DrawLine(Pen, Points[i], Points[i - 1]);
                }
            }else
            {
                //G.DrawLines(Pen,Points);
            }
        }
        double Sq(double A)
        {
            return A * A;
        }

    }
}
