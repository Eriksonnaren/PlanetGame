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
        public static double Gravity = 0.0001;
        public static double TileMinimumSize = 10;

        public Graphics graphics;
        public Form1 parent;

        public double camRot = 0;
        public double zoom = 1;
        public double toZoom = 1;

        Vector camPos = new Vector(0, 0);
        Vector camOrigin = new Vector(400, 300);
        Vector camRotation;
        Vector negCamRotation;

        // The universe that you are playing inside. Isn't that cool?
        public universe universe;

        public theGame(Graphics graphics, Form1 parent)
        {
            // You do need an object to display all the stuff to the screen, do you not?
            this.graphics = graphics;
            this.parent = parent;
            parent.MouseWheel += mouseWheel;
            camOrigin = new Vector(parent.PB.Width / 2.0, parent.PB.Height / 2.0);

            universe = new universe();

            // Make sure that the camera rotation is not null
            setRotation(0);
        }

        // Do physics and calculations
        public void update()
        {
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

            foreach (SpaceBody P in universe.bodies)
            {
                P.update();
                if (P.orbit != null)
                {
                    P.position = P.orbit.getPos();
                }
            }

            setRotation(camRot);
            //camPos.X += 1;
            //camOrigin.X += 1;

        }

        /// <summary>
        /// Rotate the camera
        /// </summary>
        public void setRotation(double rotation)
        {
            camRot = rotation;
            camRotation = getRotationVector(rotation);
            negCamRotation = getRotationVector(-rotation);
        }

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
                for (int i = universe.bodies.Count - 1; i >= 0; i--)
                {
                    universe.bodies[i].show(graphics, this);
                }
            }
        }

        public void resize()
        {
            camOrigin = new Vector(parent.PB.Width / 2.0, parent.PB.Height / 2.0);
        }

        //mouse Stuff:
        public MouseButtons mouseDown{ get; set; }
        public void mouseHold(MouseButtons Button)
        {
            if (mouseDown == MouseButtons.Left)
            {
                camPos += (parent.MousePos - parent.MousePosPrev).Rot(negCamRotation) / zoom;
            }else if(mouseDown == MouseButtons.Right)
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
            Vector OldPos = pixelToWorld(parent.MousePos);
            if(e.Delta<0)
            {
                
                zoom /= 2;
            }
            else if(e.Delta>0)
            {
                zoom *= 2;
            }else
            {
                return;
            }
            Vector NewPos = pixelToWorld(parent.MousePos);
            camPos += NewPos - OldPos;
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

        public Vector pixelToWorld(Vector p)
        {
            return ((p - camOrigin) / zoom).Rot(negCamRotation) - camPos;
        }

    }
}