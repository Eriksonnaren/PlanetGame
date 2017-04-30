using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planet_Game_4
{
    public class SpaceBody : infoObject
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

        /// <summary>
        /// 
        /// </summary>
        public universeCam infoGatheringStation;

        // Just a bunch of constructors. So self explanatory I'm not gonna comment them.
        public SpaceBody(Vector pos, double radius, int layers, Body_type type,double Mass, universeCam infoGather)
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

            infoGatheringStation = infoGather;
        }
        
        ///<summary>
        /// Set the body up by only giving an orbit. It sure is strange with Erik stuff
        ///</summary>
        public SpaceBody(Orbit orbit, double radius, int layers, Body_type type,double Mass,RingSystem.RingType RingType, universeCam infoGather)
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
            //rings = new RingSystem(this, Color.Magenta, 15, 200, radius * 1.5, radius * 2);
            // Initiate the orbit values by updating it
            orbit.update(0);
            
            position = orbit.getPos();// --- Try that there, just to make sure that the position is not null

            infoGatheringStation = infoGather;
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
        public void update(universeCam parent)
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
                rings.update(parent, Form1.ui.gameSpeed);
            }
            // update shadow, sun has no shadow
            if(Shadow!=null)
            {
                Shadow.update();
            }
        }
        public void update2()
        {
            if (Shadow != null)
            {
                Shadow.CheckPlanets(Form1.universe.bodies);
            }
        }
        
        
        ///<summary>
        /// Shows the orbit, if there is an orbit
        ///</summary>
        public void showOrbit(Graphics g, universeCam parent)
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
        public void showRings(Graphics g, universeCam parent)
        {
            // Make sure that the rings exist
            if (rings != null)
            {
                // Show the rings
                rings.show(g, parent);
            }
        }
        
        
        ///<summary>
        /// Shows the body
        ///</summary>
        public void showBody(Graphics g, universeCam parent)
        {
            // Calculate where the planet should be rendered by moving from world to pixel coordinates.
            Vector pixelPos = parent.worldToPixel(position);

            // Make sure that the planet is actually inside the screen so that you don't render it when it's outside the screen
            if (pixelPos.X >= -radius * parent.zoom && pixelPos.X <= parent.section.size.X + radius * parent.zoom && pixelPos.Y >= -radius * parent.zoom && pixelPos.Y <= parent.section.size.Y + radius * parent.zoom)
            {
                // Render the body
                shape.render(g, new windowSection(parent.section.min, parent.section.max), (int)pixelPos.X, (int)pixelPos.Y, (int)(radius * parent.zoom), rotation - parent.camRot);
            }

        }

        public bool mouseOn(Vector mouse)
        {
            Vector pixelPos = infoGatheringStation.worldToPixel(position) + infoGatheringStation.section.min;
            double magSqr = (mouse - pixelPos).MagSq();
            return magSqr < radius * radius * infoGatheringStation.zoom * infoGatheringStation.zoom;
        }

        // A function that is used when the body is going to display itself on the sidebar
        public void show(Graphics g, universeCam cam)
        {
            cam.camOrigin = cam.section.size / 2.0;
            cam.camPos = -position;

            double r = radius * 1.2;
            
            if(rings != null)
            {
                r = rings.OuterRadius * 1.2;
            }
            
            double targZoom = 1/r * Math.Min(cam.section.size.X, cam.section.size.Y) / 2;
            cam.zoom = targZoom;

            cam.render();
            g.DrawImage(cam.I, cam.section.min);
        }

        public String getName()
        {
            return "Space Body";
        }

        public String[] getInterestingInfo()
        {
            return new String[0];
        }

        public void showShadow(Graphics g, universeCam parent)
        {
            if (Shadow != null)
                Shadow.show(g,parent);
            
        }
    }
}
