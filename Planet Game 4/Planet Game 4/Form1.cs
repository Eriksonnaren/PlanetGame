using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Planet_Game_4
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// The graphics object that is used on this form
        /// </summary>
        public BufferedGraphics BG;
        
        /// <summary>
        /// The picturebox object that is used to get the width and height of the window
        /// </summary>
        public PictureBox PB;
        
        /// <summary>
        /// The ui currently displayed on the form
        /// </summary>
        static public theGame ui; // TODO: Gör om theGame till ui typ

        /// <summary>
        /// The current position of the mouse
        /// </summary>
        public Vector MousePos;
        
        /// <summary>
        /// The previous position of the mouse
        /// </summary>
        public Vector MousePosPrev;
        
        /// <summary>
        /// The timer that is making the game update
        /// </summary>
        Timer T = new Timer();

        /// <summary>
        /// Global random number generator
        /// </summary>
        public static Random rnd;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            //initiate graphics
            PB = new PictureBox();
            PB.Parent = this;
            PB.Location = new Point(0, 0);
            WindowState = FormWindowState.Maximized;
            PB.Dock = DockStyle.Fill;
            BufferedGraphicsContext Context = BufferedGraphicsManager.Current;
            BG = Context.Allocate(PB.CreateGraphics(),PB.DisplayRectangle);

            // Make a new random object
            rnd = new Random();

            // Initiate the ui
            ui = new theGame(BG.Graphics, this);

            //start the timer
            T.Interval = 20;
            T.Tick += T_Tick;
            T.Start();
        }

        // A tick of the game
        private void T_Tick(object sender, EventArgs e)//main loop
        {
            // Set the mouse position and the previous mouse position
            MousePosPrev = MousePos;
            MousePos = new Vector(PointToClient(MousePosition));

            // Update and show the ui
            ui.update();
            ui.show();

            // Render the graphics to the screen
            BG.Render();
        }

        /// <summary>
        /// Linear Interpolation with integers
        /// </summary>
        public static int lerp(int a, int b, double t)
        {
            return (int)(a+(b-a)*t);
        }

        /// <summary>
        /// Linera Interpolation with doubles
        /// </summary>
        public static double lerp(double a, double b, double t)
        {
            return a + (b - a) * t;
        }

        /// <summary>
        /// Linear Interpolation with two colors
        /// </summary>
        public static Color lerpC(Color C1,Color C2,double t)
        {
            return Color.FromArgb(lerp(C1.A,C2.A,t), lerp(C1.R, C2.R, t), lerp(C1.G, C2.G, t), lerp(C1.B, C2.B, t));
        }

        // Function gets called when the form is resized.
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            // Reinitiate the graphics
            PB.Dock = DockStyle.Fill;
            BufferedGraphicsContext Context = BufferedGraphicsManager.Current;
            BG = Context.Allocate(PB.CreateGraphics(), PB.DisplayRectangle);

            if (ui != null)
            {
                // Tell the ui that the form has been resized
                ui.resize();
                
                // Set the ui graphics object to the new graphics object
                ui.graphics = BG.Graphics;
            }
            
        }

        /// <summary>
        /// Constrains a value between two other values
        /// </summary>
        public static double constrain(double num, double min, double max)
        {
            // Constrain the number between min and max
            if(num < min)
            {
                num = min;
            }else if(num > max)
            {
                num = max;
            }

            // Return that number
            return num;
        }
        
        /// <summary>
        /// Returns A squared
        /// </summary>
        public static double Sq(double A){ return A * A; } // TODO: Call this function Square or Sqr, not Sq
        
        /// <summary>
        /// Returns true if the vector is inside the window
        /// </summary>
        public static bool isInsideWindow(Vector V,double offset)
        {
            return V.X >= -offset && V.X < ui.Size.Width+offset && V.Y >= -offset && V.Y < ui.Size.Height+offset;
        }

        /// <summary>
        /// Returns a color that is made up of the rgb values of the inputs specified
        /// </summary>
        public static Color getColor(int r, int g, int b)
        {
            return Color.FromArgb((int)constrain(r, 0, 255), (int)constrain(g, 0, 255), (int)constrain(b, 0, 255));
        }
    }
}
