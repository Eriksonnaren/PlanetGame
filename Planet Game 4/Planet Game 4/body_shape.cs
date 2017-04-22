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

        public body_shape_layer[] shape;

        public body_shape(int shapeLayers)
        {
            //shapeLayers = shapeLayers;
            shape = new body_shape_layer[shapeLayers << 1];

            int rings = 1;
            int prevRings = 0;

            double sizeSum = 0;

            int index = 0;

            for(int i = 0; i < shapeLayers; i++)
            {
                rings = rings << 1;
                //double size = 1.0 / shapeLayers * (rings - prevRings);
                double size = 1.0 / shapeLayers;
                for (int j = 0; j < (rings - prevRings) >> 1; j++)
                {
                    shape[index] = new body_shape_layer(rings * 4, size);
                    sizeSum += size;
                    index++;
                }
                prevRings = rings;
            }

            Console.WriteLine(index);

            // Normalize
            for(int i = 0; i < shapeLayers << 1; i++)
            {
                shape[i].size *= 1 / sizeSum;
            }
        }

        public void render(Graphics g, int x, int y, int radius, double rotation)
        {

            int radii = 0;

            for (int i = 0; i < shape.Length; i++)
            {
                int r = (int)(shape[i].size * radius);
                
                g.DrawEllipse(new Pen(Color.FromArgb(255, 255, 255)), x - (r+radii), y - (r+radii), (r+radii) * 2, (r+radii) * 2);

                double angle = rotation;
                double angleMovement = Math.PI * 2 / shape[i].slices;

                for(int j = 0; j < shape[i].slices; j++)
                {
                    double si = Math.Sin(angle);
                    double co = Math.Cos(angle);

                    g.DrawLine(new Pen(Color.White), (float)(si * radii + x), (float)(co * radii + y), (float)(si * (r+radii) + x), (float)(co * (r+radii) + y));

                    angle += angleMovement;
                }

                radii += r;

            }

        }

    }
}
