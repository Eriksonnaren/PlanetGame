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
        
        // TODO: Fix up these variables so that they are grouped in a way that makes sense + comment them
        public double camRot = 0;
        public double zoom = 0.0001;
        public double toZoom;
        public double startZoom;
        public double gameSpeed = 5;

        public Vector toZoomPos = new Vector(0,0);
        public Vector startZoomPos = new Vector(0,0);
        public Vector MousePos1 = new Vector(0,0);
        public Vector MousePos2 = new Vector(0,0);

        public Vector camPos = new Vector(0, 0);
        public Vector camOrigin = new Vector(400, 300);
        public Vector camRotation;
        public Vector negCamRotation;

        // The universe that you are playing inside. Isn't that cool?
        public universe universe;

        public theGame(Graphics graphics, Form1 parent)
        {
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
                P.update();
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

        // Do visual calculations and display all the things
        public void show()
        {
            graphics.Clear(Color.Black);
            
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
            
        }

        public void resize()
        {
            // Change the cameras origin to the center of the screen when the screen is resized
            camOrigin = new Vector(parent.PB.Width / 2.0, parent.PB.Height / 2.0);
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
