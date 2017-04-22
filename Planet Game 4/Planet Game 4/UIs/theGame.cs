using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planet_Game_4
{
    public class theGame : ui
    {
        public static double Gravity = 0.0001;
        public static double TileMinimumSize = 3;

        public Graphics graphics;
        public Form1 parent;

        public double camRot = 0;
        public double zoom = 1;

        public bool cameraDrag = false;

        Vector camPos = new Vector(0, 0);
        Vector camOrigin = new Vector(400, 300);
        Vector camRotation;
        Vector negCamRotation;

        // The universe that you are playing inside. Isn't that cool?
        public universe universe;

        public theGame(Graphics graphics, Form1 parent)
        {
            // You do need an object to display all the stuff to the screen, do you not?
            this.graphics = graphics;
            this.parent = parent;

            camOrigin = new Vector(parent.PB.Width / 2.0, parent.PB.Height / 2.0);

            universe = new universe();

            // Make sure that the camera rotation is not null
            setRotation(0);
        }

        // Do physics and calculations
        public void update()
        {
            foreach (SpaceBody P in universe.planets)
            {
                P.update();
                if (P.orbit!=null)
                {
                    P.position = P.orbit.getPos();
                }
            }
            
        }

        /// <summary>
        /// Rotate the camera
        /// </summary>
        public void setRotation(double rotation)
        {
            camRot = rotation;
            camRotation = getRotationVector(rotation);
            negCamRotation = getRotationVector(-rotation);
        }

        public Vector getRotationVector(double rotation)
        {
            return new Vector(Math.Cos(rotation), Math.Sin(rotation));
        }

        // Do visual calculations and display all the things
        public void show()
        {
            graphics.Clear(Color.Black);

            if (!cameraDrag)
            {
                for (int i = universe.planets.Count - 1; i >= 0; i--)
                {
                    universe.planets[i].show(graphics, this);
                }
            }
        }

        public void resize()
        {
            camOrigin = new Vector(parent.PB.Width / 2.0, parent.PB.Height / 2.0);
        }

        public void mousePressed()
        {
            cameraDrag = true;
        }

        public void mouseReleased()
        {
            cameraDrag = false;
        }

        public void mouseWheel(double delta) { }
        public void keyPressed(char key)
        {

        }

        public void keyReleased(char key) { }

        public Vector worldToPixel(Vector w)
        {
            return (w + camPos).Rot(camRotation) * zoom + camOrigin;
        }

        public Vector pixelToWorld(Vector p)
        {
            return ((p - camOrigin) / zoom).Rot(negCamRotation) - camPos;
        }

    }
}