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

        public body_shape(int r, int g, int b, int rv, int gv, int bv, bool realAverage)
        {
            root = new body_shape_recursive[4];

            for(int i = 0; i < root.Length; i++)
            {
                root[i] = new body_shape_recursive(new HSLColor((double)r, g, b), rv, gv, bv, realAverage, body_shape_recursive.tile_type.center, 4);
                //root[i].calculateAverages();
            }
        }

        public void render(Graphics g, int x, int y, int levels, int radius, double rotation)
        {
            
            double angle = -rotation;
            double angleChange = Math.PI * 2 / root.Length;

            for(int i = 0; i < root.Length; i++)
            {
                angle += angleChange;

                root[i].render(g, x, y, 0, radius-2, angle, angle + angleChange, true, Math.Log(levels, 2));
            }

        }

        public void render(Graphics g, int x, int y, int radius, double rotation)
        {

            render(g, x, y, (int)Math.Max((radius / theGame.TileMinimumSize), 2), radius, rotation);

        }

    }
}
