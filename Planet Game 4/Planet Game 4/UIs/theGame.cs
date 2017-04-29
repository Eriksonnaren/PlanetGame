using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Planet_Game_4
{
    public class theGame : ui
    {
        // Global variables that determine some properties in the game
        public static double Gravity = 6*Math.Pow(10,-11);
        public static double TileMinimumSize = 75;

        public Graphics graphics;
        public Form1 parent;
        public Size Size;

        // The min and max points of the world rendering 
        // NOTE: Only for optimization purposes. It does not actually limit the rendering precisely to that area

        public windowSection worldDisplay;

        // The coordinates for where the info box should be displayed
        public windowSection infoBoxDisplay;
        public infoObject prevInfoBox = null;
        public infoObject infoBoxAbout = null;
        public double infoBoxWidth = 0.175;
        public double infoBoxExtension = 0;

        // Camera variables
        public double camRot = 0;
        public Vector camPos = new Vector(400, 400);
        public Vector camOrigin = new Vector(400, 400);
        public Vector camRotation;
        public Vector negCamRotation;

        // Zooming variables
        public double zoom = 0.0001;
        public double toZoom;
        public double startZoom;
        public Vector toZoomPos = new Vector(0, 0);
        public Vector startZoomPos = new Vector(0, 0);

        // The game speed
        public double gameSpeed = 5;
        
        // Mouse variables
        public Vector MousePos1 = new Vector(0,0);
        public Vector MousePos2 = new Vector(0,0);

        

        // The universe that you are playing inside. Isn't that cool?
        public universe universe;

        public theGame(Graphics graphics, Form1 parent)
        {

            worldDisplay = new windowSection(new Vector(0, 0), new Vector(parent.Width - parent.Width * infoBoxWidth, parent.Height));
            infoBoxDisplay = new windowSection(new Vector(parent.Width - parent.Width * infoBoxWidth, 0), new Vector(parent.Width, parent.Height));

            Size = parent.Size;
            toZoom = zoom;
            
            // You do need an object to display all the stuff to the screen, do you not?
            this.graphics = graphics;
            this.parent = parent;
            parent.MouseWheel += mouseWheel;
            camOrigin = new Vector(parent.PB.Width / 2.0, parent.PB.Height / 2.0);

            universe = new universe();

            // Make sure that the camera rotation is not null
            setRotation(0);
            toZoomPos=camPos = -universe.bodies[1].orbit.getPos();
        }

        // Do physics and calculations
        public void update()
        {
            // Do smooth zooming
            zoom += (toZoom-zoom)*0.3;
            double T=(zoom - startZoom)/ (toZoom - startZoom);
            camPos =startZoomPos.lerp(toZoomPos,T);

            // Do mouse pressing detection
            if (Control.MouseButtons!=MouseButtons.None)//it is pressed
            {
                if(mouseDown==MouseButtons.None)//it has been pressed this tick
                {
                    mouseDown = Control.MouseButtons;
                    mousePressed(mouseDown);
                }
                mouseHold(mouseDown);
            }
            else//it is not pressed
            {
                if (mouseDown != MouseButtons.None)//it has been released this tick
                {
                    mouseReleased(mouseDown);
                    mouseDown = Control.MouseButtons;
                }
            }

            // Update all the space bodies
            foreach (SpaceBody P in universe.bodies)
            {
                P.update(this);
            }
            
            // Update them again TODO: make a name for this update function that makes more sense
            foreach (SpaceBody P in universe.bodies)
            {
                P.update2();
            }
            
            // Rotate the camera
            setRotation(camRot);
            //camPos.X += 1;
            //camOrigin.X += 1;

        }
        /// <summary>
        /// Rotate the camera
        /// </summary>
        public void setRotation(double rotation)
        {
            // TODO: call camRot camRotation, and the vectors Vec at the end, like camRotationVec
            camRot = rotation;
            camRotation = getRotationVector(rotation);
            negCamRotation = getRotationVector(-rotation);
        }

        ///<summary>
        /// Gives you a 1 magnitude vector that has an angle of the rotation
        ///</summary>
        public Vector getRotationVector(double rotation)
        {
            return new Vector(Math.Cos(rotation), Math.Sin(rotation));
        }

        public void animate(double speed)
        {
            // Do animations
            {
                // Animate the infoBar
                if (infoBoxAbout != null)
                {
                    infoBoxExtension += speed / 4.0;
                }
                else
                {
                    infoBoxExtension -= speed / 4.0;
                }

                infoBoxExtension = Form1.constrain(infoBoxExtension, 0, 1);

                // Update the properties of the infoBox
                infoBoxDisplay.min.X = parent.Width - parent.Width * Form1.lerp(0, infoBoxWidth, infoBoxExtension);
                infoBoxDisplay.max.X = infoBoxDisplay.min.X + parent.Width * infoBoxWidth;

                // Set the max of the worldBox
                worldDisplay.max.X = parent.Width - parent.Width * Form1.lerp(0, infoBoxWidth, infoBoxExtension);
            }
        }

        // Do visual calculations and display all the things
        public void show()
        {
            
            graphics.Clear(Color.Black);
            
            // Show the universe
            if (true)
            {
                

                // TODO: Do wierd maths to make the orbits be over the shadows but under everything else, although everything else should be on top of the shadows
                for (int i = universe.bodies.Count - 1; i >= 0; i--)
                {
                    universe.bodies[i].showOrbit(graphics, this);
                }
                for (int i = universe.bodies.Count - 1; i >= 0; i--)
                {
                    universe.bodies[i].showRings(graphics);
                }
                for (int i = universe.bodies.Count - 1; i >= 0; i--)
                {
                    universe.bodies[i].showBody(graphics, parent, this);
                }
                for (int i = universe.bodies.Count - 1; i >= 0; i--)
                {
                    universe.bodies[i].showShadow(graphics, parent, this);
                }
            }


            // Show the infoBox
            if(infoBoxDisplay.min.X < parent.Width)
            {
                if (infoBoxAbout != null)
                {
                    graphics.FillRectangle(new SolidBrush(Color.LightGray), (int)(infoBoxDisplay.min.X), (int)(infoBoxDisplay.min.Y), (int)(infoBoxDisplay.size.X), (int)(infoBoxDisplay.size.Y));

                    Vector size = infoBoxDisplay.size;
                    int intSize = (int)Math.Min(size.X, size.Y) - 20;
                    infoBoxAbout.show(graphics, (int)infoBoxDisplay.min.X + 10, (int)infoBoxDisplay.min.Y + 10, intSize, intSize);
                }else if(prevInfoBox != null)
                {
                    graphics.FillRectangle(new SolidBrush(Color.LightGray), (int)(infoBoxDisplay.min.X), (int)(infoBoxDisplay.min.Y), (int)(infoBoxDisplay.size.X), (int)(infoBoxDisplay.size.Y));

                    Vector size = infoBoxDisplay.size;
                    int intSize = (int)Math.Min(size.X, size.Y) - 20;
                    prevInfoBox.show(graphics, (int)infoBoxDisplay.min.X + 10, (int)infoBoxDisplay.min.Y + 10, intSize, intSize);
                }
            }
            
        }

        public bool onWorldRender(int x, int y)
        {
            return x >= infoBoxDisplay.min.X && y >= infoBoxDisplay.min.Y && x <= infoBoxDisplay.max.X && y <= infoBoxDisplay.max.Y;
        }

        public void resize()
        {
            // Change the cameras origin to the center of the screen when the screen is resized
            camOrigin = new Vector(parent.PB.Width / 2.0, parent.PB.Height / 2.0);
        }

        public void setInfoBox(infoObject info)
        {
            prevInfoBox = infoBoxAbout;
            infoBoxAbout = info;
        }

        //mouse Stuff:
        public MouseButtons mouseDown{ get; set; }
        public void mouseHold(MouseButtons Button)
        {
            if (mouseDown == MouseButtons.Left)
            {
                toZoomPos += (parent.MousePos - parent.MousePosPrev).Rot(negCamRotation)/zoom;
            }
            else if(mouseDown == MouseButtons.Right)
            {
                camRot += (parent.MousePos - camOrigin).Angle() - (parent.MousePosPrev - camOrigin).Angle();
            }
        }
        
        public void mousePressed(MouseButtons Button)
        {
            if(false)
            {

            }
            else
            {
                for (int i = universe.bodies.Count - 1; i >= 0; i--)
                {
                    if(universe.bodies[i].mouseOn(parent.MousePos))
                    {
                        infoObject newSelected = universe.bodies[i] == infoBoxAbout ? null : universe.bodies[i];
                        setInfoBox(newSelected);
                        break;
                    }
                }
            }
        }

        public void mouseReleased(MouseButtons Button)
        {
            
        }

        public void mouseWheel(object sender,MouseEventArgs e)
        {
            Vector OldPos = pixelToWorld(parent.MousePos,toZoom);
            MousePos1 = OldPos;
            double zoomAmount=1.5;
            
            if(e.Delta<0)
            {
                toZoom /= zoomAmount;
            }
            else if(e.Delta>0)
            {
                toZoom *= zoomAmount;
            }else
            {
                return;
            }
            startZoom = zoom;
            startZoomPos = camPos;
            Vector NewPos = pixelToWorld(parent.MousePos,toZoom);
            MousePos2 = NewPos;
            toZoomPos =camPos+ NewPos - OldPos;
        }

        // end mouseStuff
        public void keyPressed(char key)
        {

        }

        public void keyReleased(char key) { }

        public Vector worldToPixel(Vector w)
        {
            return (w + camPos).Rot(camRotation) * zoom + camOrigin;
        }
        public Vector worldToPixel(Vector w,double zoom)
        {
            return (w + camPos).Rot(camRotation) * zoom + camOrigin;
        }
        public Vector pixelToWorld(Vector p)
        {
            return ((p - camOrigin) / zoom).Rot(negCamRotation) - camPos;
        }
        public Vector pixelToWorld(Vector p,double zoom)
        {
            return ((p - camOrigin) / zoom).Rot(negCamRotation) - camPos;
        }
    }
}
