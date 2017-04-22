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

        public ui ui;

        Timer T = new Timer();

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

            // Initiate the ui
            ui = new theGame(BG.Graphics);

            //start the timer
            T.Interval = 20;
            T.Tick += T_Tick;
            T.Start();
        }

        private void T_Tick(object sender, EventArgs e)//main loop
        {
            ui.show();

            BG.Render();
        }
    }
}
