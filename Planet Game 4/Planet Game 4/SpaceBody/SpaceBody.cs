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
        public Shadow Shadow;


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
        public Vector velocity;//TODO: get this from orbit

        
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
        /// Time to rotate once
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
            if (type != Body_type.sun)
                Shadow = new Shadow(this);
            // Set some of the variables to the paramaters put into the constructor
            position = pos;
            this.type = type;
            this.radius = radius;
            mass = Mass;

            // Set the shape
            setShape(layers, type);
        }
        
        ///<summary>
        /// Set the body up by only giving an orbit. It sure is strange with Erik stuff
        ///</summary>
        public SpaceBody(Orbit orbit, double radius, int layers, Body_type type,double Mass,RingSystem.RingType RingType)
        {
            if(type!=Body_type.sun)
            Shadow = new Shadow(this);
            // Set some of the variables to the paramaters put into the constructor
            this.orbit = orbit;
            this.type = type;
            this.radius = radius;
            mass = Mass;
            
            // Set the shape
            setShape(layers, type);
            
            // Initiate the rings.
            if(RingType!=RingSystem.RingType.Empty)
                rings = new RingSystem(this,RingType,15,200,radius*1.5,radius*2);
            
            // Initiate the orbit values by updating it
            orbit.update(0);
            
            position = orbit.getPos();// --- Try that there, just to make sure that the position is not null
        }

        
        ///<summary>
        /// Sets the shape of the planet. The body type determines the generation algorithm used.
        ///</summary>
        public void setShape(int layers, Body_type type)
        {
            // TODO: Make this a switch statement
            if (type == Body_type.sun)
            {
                HSLColor c = new HSLColor(255, 255, 0);
                shape = new body_shape((int)c.Hue, (int)c.Saturation, (int)c.Luminosity, 0, 20, 20, false);
            }
            else
            {
                HSLColor c = new HSLColor(200, 200, 200);
                shape = new body_shape((int)c.Hue, (int)c.Saturation, (int)c.Luminosity, 0, 20, 20, false);
            }
        }

        /// <summary>
        /// PLANET PHYSICS and other stuff too
        /// </summary>
        public void update()
        {
            // Rotate the body
            rotation += rotationSpeed* Form1.ui.gameSpeed;
            
            // If there is an orbit, then update the orbit and set the position to what the orbit says it should be
            if (orbit != null)
            {
                orbit.update(Form1.ui.gameSpeed);
                position = orbit.getPos();
            }
            
            // If there are any rings, update those too
            if(rings != null)
            {
                rings.update(Form1.ui.gameSpeed);
            }
            // update shadow, sun has no shadow
            if(Shadow!=null)
            {
                Shadow.update();
            }
        }
        
        
        ///<summary>
        /// Shows the orbit, if there is an orbit
        ///</summary>
        public void showOrbit(Graphics g,theGame parent)
        {
            // Make sure that the orbit exists
            if (orbit != null)
            {
                // Show the orbit
                orbit.Show(g, parent, new Pen(Color.Blue, 2), true);
            }
        }
        
        ///<summary>
        /// Shows the rings, if there are any rings, that is
        ///</summary>
        public void showRings(Graphics g)
        {
            // Make sure that the rings exist
            if (rings != null)
            {
                // Show the rings
                rings.show(g);
            }
        }
        
        
        ///<summary>
        /// Shows the body
        ///</summary>
        public void showBody(Graphics g, Form1 form, theGame parent)
        {
            // Calculate where the planet should be rendered by moving from world to pixel coordinates.
            Vector pixelPos = parent.worldToPixel(position);

            // Make sure that the planet is actually inside the screen so that you don't render it when it's outside the screen
            if (pixelPos.X >= -radius * parent.zoom && pixelPos.X <= form.PB.Width + radius * parent.zoom && pixelPos.Y >= -radius * parent.zoom && pixelPos.Y <= form.PB.Height + radius * parent.zoom)
            {
                // Render the body
                shape.render(g, parent, (int)pixelPos.X, (int)pixelPos.Y, (int)(radius * parent.zoom), rotation - parent.camRot);
            }

        }
        public void showShadow(Graphics g, Form1 form, theGame parent)
        {
            if (Shadow != null)
                Shadow.show(g,form,parent);
            
        }
    }
}
