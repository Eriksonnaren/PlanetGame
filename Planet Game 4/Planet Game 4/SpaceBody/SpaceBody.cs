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
        public Vector position;
        public Vector velocity;

        public double radius;
        public double mass;
        public double rotation;
        public double dayTime;
        public body_shape shape;
        public RingSystem rings;

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
            rings = new RingSystem(this,RingSystem.RingType.ice);
            orbit.update(0);
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
            if(rings !=null)
            {
                rings.update();
            }
        }

        public void show(Graphics g, Form1 form, theGame parent)
        {
            if (orbit != null)
            {
                orbit.Show(g, parent, new Pen(Color.Blue,2),true);
            }
            if(rings !=null)
            {
                rings.show(g);
            }
            Vector pixelPos = parent.worldToPixel(position);

            if (pixelPos.X >= -radius * parent.zoom && pixelPos.X <= form.PB.Width + radius * parent.zoom && pixelPos.Y >= -radius * parent.zoom && pixelPos.Y <= form.PB.Height + radius * parent.zoom)
            {
                shape.render(g, (int)pixelPos.X, (int)pixelPos.Y, (int)(radius * parent.zoom), rotation - parent.camRot);
            }

        }

    }
}
