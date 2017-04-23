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
        public BufferedGraphics BG;
        public PictureBox PB;
        static public theGame ui;

        public Vector MousePos;
        public Vector MousePosPrev;
        
        Timer T = new Timer();

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

        private void T_Tick(object sender, EventArgs e)//main loop
        {
            // Set the mouse position
            MousePosPrev = MousePos;
            MousePos = new Vector(PointToClient(MousePosition));

            ui.update();
            ui.show();

            BG.Render();
        }

        public static int lerp(int a, int b, double t)
        {
            return (int)(a+(b-a)*t);
        }

        public static double lerp(double a, double b, double t)
        {
            return (a + (b - a) * t);
        }

        public static Color lerpC(Color C1,Color C2,double t)
        {
            return Color.FromArgb(lerp(C1.A,C2.A,t), lerp(C1.R, C2.R, t), lerp(C1.G, C2.G, t), lerp(C1.B, C2.B, t));
        }



        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            PB.Dock = DockStyle.Fill;
            BufferedGraphicsContext Context = BufferedGraphicsManager.Current;
            BG = Context.Allocate(PB.CreateGraphics(), PB.DisplayRectangle);

            if (ui != null)
            {
                ui.resize();
                
                ui.graphics = BG.Graphics;
            }
            
        }

        public static double constrain(double num, double min, double max)
        {
            if(num < min)
            {
                num = min;
            }else if(num > max)
            {
                num = max;
            }

            return num;
        }

    }
}
