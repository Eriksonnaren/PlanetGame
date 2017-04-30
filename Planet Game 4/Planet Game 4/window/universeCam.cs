using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Planet_Game_4
{
    public class universeCam : window
    {

        public Bitmap I { get; set; }
        public Graphics G { get; set; }

        public windowSection section { get; set; }

        int x = 0;

        // Camera variables
        public double camRot = 0;
        public Vector camPos;
        public Vector camOrigin;
        public Vector camRotation;
        public Vector negCamRotation;

        // Zooming variables
        public double zoom = 0.00001;

        public universeCam(windowSection section)
        {
            this.section = section;

            camPos = new Vector(0, 0);
            camOrigin = section.size / 2;
            camRot = 0;
            camRotation = Vector.getRotationVector(camRot);
            negCamRotation = Vector.getRotationVector(camRot + Math.PI);

            I = new Bitmap((int)section.size.X, (int)section.size.Y);

            G = Graphics.FromImage(I);
        }

 
        public void update()
        {

        }

        public void render()
        {
            G.Clear(Color.Black);

            // TODO: Do wierd maths to make the orbits be over the shadows but under everything else, although everything else should be on top of the shadows
            for (int i = Form1.universe.bodies.Count - 1; i >= 0; i--)
            {
                Form1.universe.bodies[i].showOrbit(G, this);
            }
            for (int i = Form1.universe.bodies.Count - 1; i >= 0; i--)
            {
                Form1.universe.bodies[i].showRings(G, this);
            }
            for (int i = Form1.universe.bodies.Count - 1; i >= 0; i--)
            {
                Form1.universe.bodies[i].showBody(G, this);
            }
            for (int i = Form1.universe.bodies.Count - 1; i >= 0; i--)
            {
                Form1.universe.bodies[i].showShadow(G, this);
            }
        }

        public void resize(Vector size)
        {
            section.max = section.min + size;

            I = new Bitmap((int)size.X, (int)size.Y);

            G = Graphics.FromImage(I);

        }

        public Vector worldToPixel(Vector w)
        {
            return (w + camPos).Rot(camRotation) * zoom + camOrigin;
        }
        public Vector worldToPixel(Vector w, double zoom)
        {
            return (w + camPos).Rot(camRotation) * zoom + camOrigin;
        }
        public Vector pixelToWorld(Vector p)
        {
            return ((p - camOrigin) / zoom).Rot(negCamRotation) - camPos;
        }
        public Vector pixelToWorld(Vector p, double zoom)
        {
            return ((p - camOrigin) / zoom).Rot(negCamRotation) - camPos;
        }

    }
}
