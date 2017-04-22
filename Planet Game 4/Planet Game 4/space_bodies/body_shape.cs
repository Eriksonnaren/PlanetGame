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

        public int[][] shape;

        public body_shape(int shapeLayers)
        {
            shape = new int[shapeLayers][];

            for(int i = 0; i < shapeLayers; i++)
            {
                int rings = (int)Math.Floor(Math.Pow(2, i+3));
                shape[i] = new int[rings];

                for(int j = 0; j < rings; j++)
                {
                    shape[i][j] = 0;
                }
            }
        }

        public void render(Graphics g, int x, int y, int radius)
        {

            int layerRadius = radius / shape.Length;

            for(int i = 0; i < shape.Length; i++)
            {
                int r = layerRadius * (i+1);
                g.DrawEllipse(new Pen(Color.FromArgb(255, 255, 255)), x - r, y - r, r * 2, r * 2);

                double angle = 0;
                double angleMovement = Math.PI * 2 / shape[i].Length;

                for(int j = 0; j < shape[i].Length; j++)
                {
                    double si = Math.Sin(angle);
                    double co = Math.Cos(angle);

                    g.DrawLine(new Pen(Color.White), (float)(si * layerRadius * i + x), (float)(co * layerRadius * i + y), (float)(si * r + x), (float)(co * r + y));

                    angle += angleMovement;
                }

            }

        }

    }
}
