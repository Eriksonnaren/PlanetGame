﻿using System;
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
            ui.update();
            ui.show();

            BG.Render();
        }

        public static int lerp(int a, int b, double t)
        {
            return (int)(a+(b-a)*t);
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

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            ui.mousePressed();
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            ui.mouseReleased();
        }

        private void Form1_MouseLeave(object sender, EventArgs e)
        {
            ui.mouseReleased();
        }
    }
}
