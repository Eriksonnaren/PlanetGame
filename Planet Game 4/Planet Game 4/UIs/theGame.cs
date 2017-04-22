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

        Graphics graphics;

        Vector camPos = new Vector(0, 0);
        Vector camOrigin = new Vector(400, 300);
        Vector camRotation = new Vector(1, 0);

        public double camRot = 0;
        public double zoom = 1;

        // The universe that you are playing inside. Isn't that cool?
        public universe universe;

        public theGame(Graphics graphics)
        {
            // You do need an object to display all the stuff to the screen, do you not?
            this.graphics = graphics;

            universe = new universe();

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

            setRotation(camRot + 0.1);
            //camPos.X += 1;
        }

        /// <summary>
        /// Rotate the camera
        /// </summary>
        public void setRotation(double rotation)
        {
            camRot = rotation;
            camRotation.X = Math.Cos(rotation);
            camRotation.Y = Math.Sin(rotation);
        }

        public Vector getRotationVector(double rotation)
        {
            return new Vector(Math.Cos(rotation), Math.Sin(rotation));
        }

        // Do visual calculations and display all the things
        public void show()
        {
            graphics.Clear(Color.Black);

            for(int i = universe.planets.Count-1; i >= 0; i--)
            {
                universe.planets[i].show(graphics, this);
            }

            Vector whereItShouldBe = new Vector(300, 300);
            Vector point = worldToPixel(pixelToWorld(whereItShouldBe));
            graphics.FillEllipse(new SolidBrush(Color.White), (float)point.X, (float)point.Y, 20, 20);
            graphics.FillEllipse(new SolidBrush(Color.White), (float)whereItShouldBe.X, (float)whereItShouldBe.Y, 20, 20);
        }

        public Vector worldToPixel(Vector w)
        {
            return (w + camPos).Rot(camRotation) * zoom + camOrigin;
        }

        public Vector pixelToWorld(Vector p)
        {
            return ((p - camOrigin) / zoom).Rot(-camRotation) - camPos;
        }

    }
}