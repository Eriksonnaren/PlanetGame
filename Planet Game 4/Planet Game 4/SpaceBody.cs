using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planet_Game_4
{
    public class SpaceBody
    {

        public enum Body_type
        {
            sun,
            life,
            gas,
            rock,
            ice
        }

        public Body_type type;

        public Orbit orbit;
        public Vector position { get; set; }
        public Vector velocity;

        public double radius { get; set; }
        public double mass { get; set; }
        public double rotation { get; set; }

        public body_shape shape { get; set; }

        public SpaceBody(Vector pos, double radius, int layers, Body_type type,double Mass)
        {
            position = pos;
            this.type = type;
            this.radius = radius;
            mass = Mass;

            setShape(layers, type);
        }
        public SpaceBody(Orbit orbit, double radius, int layers, Body_type type,double Mass)
        {
            this.orbit = orbit;
            this.type = type;
            this.radius = radius;
            mass = Mass;

            setShape(layers, type);
        }

        public void setShape(int layers, Body_type type)
        {
            if (type == Body_type.sun)
            {
                shape = new body_shape(200, 200, 0, 55, 55, 0, false);
            }
            else
            {
                shape = new body_shape(200, 200, 200, 20, 20, 20, true);
            }
        }

        /// <summary>
        /// PLANET PHYSICS and other stuff too
        /// </summary>
        public void update()
        {
            rotation += 0.01;
            if (orbit != null)
            {
                orbit.update(1);
                position = orbit.getPos();
                
            }
        }

        public void show(Graphics g, Form1 form, theGame parent)
        {
            if (orbit != null)
            {
                // TODO ERIK: Fixa så orbits också blir affectade av kamerans position och rotation
                orbit.Show(g, parent, new Pen(Color.Blue,2),true);
            }

            Vector pixelPos = parent.worldToPixel(position);

            if (pixelPos.X >= -radius * parent.zoom && pixelPos.X <= form.PB.Width + radius * parent.zoom && pixelPos.Y >= -radius * parent.zoom && pixelPos.Y <= form.PB.Height + radius * parent.zoom)
            {
                shape.render(g, (int)pixelPos.X, (int)pixelPos.Y, (int)(radius * parent.zoom), rotation - parent.camRot);
            }

        }

    }
}
