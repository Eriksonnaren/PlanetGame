using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Planet_Game_4
{
    public class universeCam : window
    {

        public Bitmap I { get; set; }
        public Graphics G { get; set; }

        public windowSection section { get; set; }

        int x = 0;

        public universe universe;

        public universeCam(windowSection section, universe universe)
        {
            this.section = section;

            this.universe = universe;
            
            I = new Bitmap((int)section.size.X, (int)section.size.Y);

            G = Graphics.FromImage(I);
        }

 
        public void update()
        {

        }

        public void render()
        {
            x += 10;

            G.Clear(Color.White);

            G.FillRectangle(new SolidBrush(Color.Black), x, 0, 50, 50);
        }

        public void resize(Vector size)
        {
            section.max = section.min + size;

            I = new Bitmap((int)section.size.X, (int)section.size.Y);

            G = Graphics.FromImage(I);
        }

    }
}
