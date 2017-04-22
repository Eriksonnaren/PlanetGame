using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planet_Game_4
{
    public class body_shape
    {

        public int initialDivisions = 4;

        public body_shape_recursive[] root;

        public body_shape(int shapeLayers, int r, int g, int b, int rv, int gv, int bv)
        {
            root = new body_shape_recursive[4];

            for(int i = 0; i < root.Length; i++)
            {
                root[i] = new body_shape_recursive(new body_shape_piece(Color.FromArgb(200, 0, 0)), 5);
            }
        }

        public void render(Graphics g, int x, int y, int levels, int radius, double rotation)
        {
            
            double angle = -rotation;
            double angleChange = Math.PI * 2 / root.Length;

            for(int i = 0; i < root.Length; i++)
            {
                angle += angleChange;

                root[i].render(g, x, y, 0, radius, angle, angle + angleChange, (int)Math.Log(levels, 2));
            }

        }

        public void render(Graphics g, int x, int y, int radius, double rotation)
        {

            render(g, x, y, (int)Math.Max((radius / theGame.TileMinimumSize), 6), radius, rotation);

        }

    }
}
