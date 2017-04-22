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
        Orbit O;
        public theGame ui;
        
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
            theGame temp = new theGame(BG.Graphics);
            ui=temp;
            O = new Orbit(temp.universe.planets[0]);
            O.Generate(new Vector(200,0),new Vector(0,0.02));


            //start the timer
            T.Interval = 20;
            T.Tick += T_Tick;
            T.Start();
        }

        private void T_Tick(object sender, EventArgs e)//main loop
        {
            ui.show();
            Point M = PointToClient(MousePosition);
            O.Generate(new Vector(200, 0), (new Vector(M.X,M.Y)-(ui.universe.planets[0].position+new Vector(200,0)))/2000);
            O.update(200);
            O.Show(BG.Graphics);
            Vector P = O.getPos();
            BG.Graphics.FillEllipse(Brushes.Green,(float)(P.X+ ui.universe.planets[0].position.X)-10,(float)(P.Y + ui.universe.planets[0].position.Y - 10),20,20);
            BG.Render();

        }
    }
}
