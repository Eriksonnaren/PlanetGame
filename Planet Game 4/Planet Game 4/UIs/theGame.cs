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

        public universeCam worldDisplay;

        // The coordinates for where the info box should be displayed
        public windowSection infoBoxDisplay;
        public infoObject prevInfoBox = null;
        public infoObject infoBoxAbout = null;
        public double infoBoxWidth = 0.175;
        public double infoBoxExtension = 0;
        public universeCam infoCam = null;

        // Zooming variables
        public double toZoom;
        public double startZoom;
        public Vector toZoomPos = new Vector(0, 0);
        public Vector startZoomPos = new Vector(0, 0);

        // The game speed
        public double gameSpeed = 10;
        
        // Mouse variables
        public Vector MousePos1 = new Vector(0,0);
        public Vector MousePos2 = new Vector(0,0);

        public theGame(Graphics graphics, Form1 parent)
        {

            worldDisplay = new universeCam(new windowSection(new Vector(0, 0), new Vector(parent.Width - parent.Width * infoBoxWidth, parent.Height)));

            Form1.universe = new universe(worldDisplay);
            
            infoBoxDisplay = new windowSection(new Vector(parent.Width - parent.Width * infoBoxWidth, 0), new Vector(parent.Width, parent.Height));

            Size = parent.Size;
            toZoom = worldDisplay.zoom;
            
            // You do need an object to display all the stuff to the screen, do you not?
            this.graphics = graphics;
            this.parent = parent;
            parent.MouseWheel += mouseWheel;
            worldDisplay.camOrigin = new Vector(parent.PB.Width / 2.0, parent.PB.Height / 2.0);

            // Make sure that the camera rotation is not null
            setRotation(0);
            toZoomPos = worldDisplay.camPos = -Form1.universe.bodies[1].orbit.getPos();
        }

        // Do physics and calculations
        public void update()
        {
            // Do smooth zooming
            worldDisplay.zoom += (toZoom- worldDisplay.zoom)*0.3;
            double T=(worldDisplay.zoom - startZoom)/ (toZoom - startZoom);
            worldDisplay.camPos =startZoomPos.lerp(toZoomPos,T);

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
            foreach (SpaceBody P in Form1.universe.bodies)
            {
                P.update(worldDisplay);
            }
            
            // Update them again TODO: make a name for this update function that makes more sense
            foreach (SpaceBody P in Form1.universe.bodies)
            {
                P.update2();
            }
            
            // Rotate the camera
            setRotation(worldDisplay.camRot);
            //camPos.X += 1;
            //camOrigin.X += 1;

        }
        /// <summary>
        /// Rotate the camera
        /// </summary>
        public void setRotation(double rotation)
        {
            // TODO: call camRot camRotation, and the vectors Vec at the end, like camRotationVec
            worldDisplay.camRot = rotation;
            worldDisplay.camRotation = Vector.getRotationVector(rotation);
            worldDisplay.negCamRotation = Vector.getRotationVector(-rotation);
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
                infoBoxDisplay.min.X = parent.PB.Width - parent.PB.Width * Form1.lerp(0, infoBoxWidth, infoBoxExtension);
                infoBoxDisplay.max.X = infoBoxDisplay.min.X + parent.PB.Width * infoBoxWidth;

                // Set the max of the worldBox
                worldDisplay.resize(new Vector(parent.PB.Width - parent.PB.Width * Form1.lerp(0, infoBoxWidth, infoBoxExtension), worldDisplay.section.size.Y));
            }
        }
        Engine1 E = new Engine1();
        // Do visual calculations and display all the things
        public void show()
        {
            
            graphics.Clear(Color.Black);
            
            // Show the universe
            if (true)
            {
                worldDisplay.render();

                graphics.DrawImage(worldDisplay.I, worldDisplay.section.min);
            }

            // Show the infoBox
            if (infoBoxDisplay.min.X < parent.Width)
            {
                if(infoCam == null)
                {
                    infoCam = new universeCam(new windowSection(new Vector(100, 100), new Vector(300, 300)));
                }

                Vector size = infoBoxDisplay.size;
                int intSize = (int)Math.Min(size.X, size.Y) - 10;

                infoCam.section.min = new Vector(infoBoxDisplay.min.X + 10, infoBoxDisplay.min.Y + 10);
                infoCam.resize(new Vector(intSize-10,intSize-10));

                if (infoBoxAbout != null)
                {
                    graphics.FillRectangle(new SolidBrush(Color.LightGray), (int)(infoBoxDisplay.min.X), (int)(infoBoxDisplay.min.Y), (int)(infoBoxDisplay.size.X), (int)(infoBoxDisplay.size.Y));
                    
                    infoBoxAbout.show(graphics, infoCam);
                }else if(prevInfoBox != null)
                {
                    graphics.FillRectangle(new SolidBrush(Color.LightGray), (int)(infoBoxDisplay.min.X), (int)(infoBoxDisplay.min.Y), (int)(infoBoxDisplay.size.X), (int)(infoBoxDisplay.size.Y));

                    
                    prevInfoBox.show(graphics, infoCam);
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
            worldDisplay.camOrigin = new Vector(parent.PB.Width / 2.0, parent.PB.Height / 2.0);
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
                toZoomPos += (parent.MousePos - parent.MousePosPrev).Rot(worldDisplay.negCamRotation)/ worldDisplay.zoom;
            }
            else if(mouseDown == MouseButtons.Right)
            {
                worldDisplay.camRot += (parent.MousePos - worldDisplay.camOrigin).Angle() - (parent.MousePosPrev - worldDisplay.camOrigin).Angle();
            }
        }
        
        public void mousePressed(MouseButtons Button)
        {
            if(false)
            {

            }
            else
            {
                for (int i = Form1.universe.bodies.Count - 1; i >= 0; i--)
                {
                    if(Form1.universe.bodies[i].mouseOn(parent.MousePos))
                    {
                        infoObject newSelected = Form1.universe.bodies[i] == infoBoxAbout ? null : Form1.universe.bodies[i];
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
            Vector OldPos = worldDisplay.pixelToWorld(parent.MousePos,toZoom);
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
            startZoom = worldDisplay.zoom;
            startZoomPos = worldDisplay.camPos;
            Vector NewPos = worldDisplay.pixelToWorld(parent.MousePos,toZoom);
            MousePos2 = NewPos;
            toZoomPos = worldDisplay.camPos + NewPos - OldPos;
        }

        // end mouseStuff
        public void keyPressed(char key)
        {

        }

        public void keyReleased(char key) { }

        
    }
}
