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

        ///<summary>
        ///Holds all types of bodies, for example a sun, a gas giant, or an asteroid
        ///</summary>
        public enum Body_type
        {
            sun,
            life,
            gas,
            rock,
            ice,
            asteroid
        }

        ///<summary>
        ///The type of body this body is. This can determine color and generation algorithm
        ///</summary>
        public Body_type type;

        ///<summary>
        /// The orbit(If null there is no orbit) that this body has.
        ///</summary>
        public Orbit orbit;
        
        
        ///<summary>
        /// The position of the body
        ///</summary>
        public Vector position;
        
        ///<summary>
        /// The velocity of the body. If the body has an orbit, this is not used
        ///</summary>
        public Vector velocity;

        
        ///<summary>
        /// The planets radius. Pretty self explanatory
        ///</summary>
        public double radius;
        ///<summary>
        /// The mass of the planet.
        ///</summary>
        public double mass;
        ///<summary>
        /// The planets rotation
        ///</summary>
        public double rotation;
        
        ///<summary>
        /// How fast the planet is rotating. Rotation += rotationSpeed * dt
        ///</summary>
        public double rotationSpeed = 0.001;
        ///<summary>
        /// ERIK: beskriv vad denna variabel gör exakt. Vet ej hur den funkar riktigt
        ///</summary>
        public double dayTime;
        ///<summary>
        /// The shape of the body, the color and the geography. All the important stuff in one place
        ///</summary>
        public body_shape shape;
        ///<summary>
        /// The rings around the planet. If null, there are no rings.
        ///</summary>
        public RingSystem rings;

        // Just a bunch of constructors. So self explanatory I'm not gonna comment them.
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
            rings = new RingSystem(this,RingSystem.RingType.Lava,15,200,radius*1.5,radius*2);
            orbit.update(0);
        }

        
        ///<summary>
        /// Sets the shape of the planet. The body type determines the generation algorithm used.
        ///</summary>
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

    }
}
