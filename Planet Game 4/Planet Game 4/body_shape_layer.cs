using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planet_Game_4
{
    public class body_shape_layer
    {

        public int slices;
        public double size;

        public body_shape_piece[] pieces; 

        public body_shape_layer(int slices, double size)
        {
            this.slices = slices;
            this.size = size;

            pieces = new body_shape_piece[slices];

            for(int i = 0; i < slices; i++)
            {
                pieces[i] = new body_shape_piece(Color.FromArgb(Form1.rnd.Next(200, 255), 0, 0));
            }
        }

    }
}
