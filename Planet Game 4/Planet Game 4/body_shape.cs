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
                    shape[index] = new body_shape_layer((rings >> 1) * initialDivisions, size);
                    sizeSum += size;
                    index++;
                }
                prevRings = rings;
            }

            // Normalize
            for(int i = 0; i < shapeLayers; i++)
            {
                shape[i].size *= 1 / sizeSum;
            }
        }

        public void render(Graphics g, int x, int y, int levels, int radius, double rotation)
        {
            
            levels = Math.Min(levels, shape.Length);

            double radii = 0;

            int rings = 1;

            for (int i = 0; i < levels; i++)
            {
                if (shape[i] == null) continue;
                double r = ((1.0 / levels) * radius);

                //g.DrawEllipse(new Pen(Color.FromArgb(255, 255, 255)), x - (r+radii), y - (r+radii), (r+radii) * 2, (r+radii) * 2);

                double angle = rotation;
                double angleMovement = Math.PI * 2 / shape[i].slices;

                rings = rings << 1;

                for (int j = 0; j < initialDivisions * (rings >> 1); j++)
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

                    int _i = Form1.lerp(0, shape.Length - 1, i / (double)(levels - 1));
                    int _j = Form1.lerp(0, shape[_i].slices - 1, (double)(j) / ((rings>>1) * initialDivisions));
                    g.FillPolygon(new SolidBrush(shape[_i].pieces[_j].c), points);

                    angle += angleMovement;
                }

                radii += r;

            }

        }

        public void render(Graphics g, int x, int y, int radius, double rotation)
        {

            render(g, x, y, (int)Math.Max((radius / theGame.TileMinimumSize), 2), radius, rotation);

        }

    }
}
