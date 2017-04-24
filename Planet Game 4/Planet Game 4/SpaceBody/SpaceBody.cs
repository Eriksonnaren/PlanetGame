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
        public Shadow Shadow;
        public Orbit orbit;
        public Vector position;
        public Vector velocity;

        public double radius;
        public double mass;
        public double rotation;
        public double rotationSpeed = 0.001;
        public double dayTime;
        public body_shape shape;
        public RingSystem rings;

        public SpaceBody(Vector pos, double radius, int layers, Body_type type,double Mass)
        {
            if (type != Body_type.sun)
                Shadow = new Shadow(this);
            position = pos;
            this.type = type;
            this.radius = radius;
            mass = Mass;

            setShape(layers, type);
        }
        public SpaceBody(Orbit orbit, double radius, int layers, Body_type type,double Mass)
        {
            if(type!=Body_type.sun)
            Shadow = new Shadow(this);
            this.orbit = orbit;
            this.type = type;
            this.radius = radius;
            mass = Mass;
            setShape(layers, type);
            rings = new RingSystem(this,RingSystem.RingType.Lava,15,100,radius*1.5,radius*2);
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
            rotation += rotationSpeed* Form1.ui.gameSpeed;
            if (orbit != null)
            {
                orbit.update(Form1.ui.gameSpeed);
                position = orbit.getPos();
            }
            if(rings !=null)
            {
                rings.update(Form1.ui.gameSpeed);
            }
            if(Shadow!=null)
            {
                Shadow.update();
            }
        }
        public void showOrbit(Graphics g,theGame parent)
        {
            if (orbit != null)
            {
                orbit.Show(g, parent, new Pen(Color.Blue, 2), true);
            }
        }
        public void showRings(Graphics g)
        {
            if (rings != null)
            {
                rings.show(g);
            }
        }
        public void showBody(Graphics g, Form1 form, theGame parent)
        {
            Vector pixelPos = parent.worldToPixel(position);

            if (pixelPos.X >= -radius * parent.zoom && pixelPos.X <= form.PB.Width + radius * parent.zoom && pixelPos.Y >= -radius * parent.zoom && pixelPos.Y <= form.PB.Height + radius * parent.zoom)
            {
                shape.render(g, (int)pixelPos.X, (int)pixelPos.Y, (int)(radius * parent.zoom), rotation - parent.camRot);
            }

        }
        public void showShadow(Graphics g, Form1 form, theGame parent)
        {
            if (Shadow != null)
                Shadow.show(g,form,parent);
            
        }
    }
}
