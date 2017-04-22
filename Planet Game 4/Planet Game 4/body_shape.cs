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

            shape = new body_shape_layer[shapeLayers];

            int rings = 1;
            int prevRings = 0;

            double sizeSum = 0;

            int index = 0;

            for(int i = 0; i < shapeLayers; i++)
            {
                rings = rings << 1;
                //double size = 1.0 / shapeLayers * (rings - prevRings);
                double size = 1.0 / shapeLayers;
                for (int j = 0; j < (rings - prevRings) >> 1 && index < shapeLayers; j++)
                {
                    shape[index] = new body_shape_layer((rings >> 1) * 3, size);
                    sizeSum += size;
                    index++;
                }
                prevRings = rings;
            }

            Console.WriteLine(index);

            // Normalize
            for(int i = 0; i < shapeLayers; i++)
            {
                shape[i].size *= 1 / sizeSum;
            }
        }

        public void render(Graphics g, int x, int y, int radius, double rotation)
        {

            int radii = 0;

            for (int i = 0; i < shape.Length; i++)
            {
                if (shape[i] == null) continue;
                int r = (int)(shape[i].size * radius);
                
                //g.DrawEllipse(new Pen(Color.FromArgb(255, 255, 255)), x - (r+radii), y - (r+radii), (r+radii) * 2, (r+radii) * 2);

                double angle = rotation;
                double angleMovement = Math.PI * 2 / shape[i].slices;

                for(int j = 0; j < shape[i].slices; j++)
                {
                    double si1 = Math.Sin(angle);
                    double co1 = Math.Cos(angle);
                    double si2 = Math.Sin(angle + angleMovement);
                    double co2 = Math.Cos(angle + angleMovement);

                    //g.DrawLine(new Pen(Color.White), (float)(si1 * radii + x), (float)(co1 * radii + y), (float)(si1 * (r+radii) + x), (float)(co1 * (r+radii) + y));

                    PointF[] points;

                    if (i + 1 < shape.Length && shape[i].slices == shape[i + 1].slices)
                    {
                        points = new PointF[]
                        {
                        new PointF((float)(si1 * (radii) + x), (float)(co1 * (radii) + y)),
                        new PointF((float)(si1 * (r+(radii+1)) + x), (float)(co1 * (r+(radii+1)) + y)),
                        new PointF((float)(si2 * (r+(radii+1)) + x), (float)(co2 * (r+(radii+1)) + y)),
                        new PointF((float)(si2 * (radii) + x), (float)(co2 * (radii) + y)),
                        };
                    }
                    else
                    {
                        double si3 = Math.Sin(angle + angleMovement / 2.0);
                        double co3 = Math.Cos(angle + angleMovement / 2.0);

                        points = new PointF[]
                        {
                        new PointF((float)(si1 * radii + x), (float)(co1 * radii + y)),
                        new PointF((float)(si1 * (r+radii+1) + x), (float)(co1 * (r+radii+1) + y)),
                        new PointF((float)(si3 * (r+radii+1) + x), (float)(co3 * (r+radii+1) + y)),
                        new PointF((float)(si2 * (r+radii+1) + x), (float)(co2 * (r+radii+1) + y)),
                        new PointF((float)(si2 * radii + x), (float)(co2 * radii + y)),
                        };
                    }

                    g.FillPolygon(new SolidBrush(shape[i].pieces[j].c), points);

                    angle += angleMovement;
                }

                radii += r;

            }

        }

    }
}
