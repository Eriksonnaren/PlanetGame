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
        BufferedGraphics BG;
        PictureBox PB;
        public theGame ui;
        
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
            theGame temp = new theGame(BG.Graphics);
            ui=temp;


            //start the timer
            T.Interval = 20;
            T.Tick += T_Tick;
            T.Start();
        }

        private void T_Tick(object sender, EventArgs e)//main loop
        {
            ui.update();
            ui.show();

            theGame.TileMinimumSize *= 1.01;

            BG.Render();
        }

        public static int lerp(int a, int b, double t)
        {
            return (int)(a+(b-a)*t);
        }
    }
}
