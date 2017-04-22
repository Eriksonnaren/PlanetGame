using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planet_Game_4
{
    public class body_shape_recursive
    {

        public List<body_shape_recursive> pieces;
        public body_shape_piece piece;

        public Color average;

        double count;

        public body_shape_recursive(body_shape_piece piece, double count, int req)
        {
            this.average = Color.FromArgb(piece.c.R + Form1.rnd.Next(-55, 55), 0, 0);
            this.piece = piece;

            this.count = count;

            pieces = new List<body_shape_recursive>();

            if(req == 1)
            {
                Console.WriteLine("Something happened somewhere!");
            }

            if(req > 0)
            {
                pieces.Add(new body_shape_recursive(piece, count, req - 1));
                //pieces.Add(new body_shape_recursive(piece, count, req - 1));
            }
        }

        public void render(Graphics g, int x, int y, double startR, double endR, double startAngle, double endAngle)
        {
            if (pieces.Count == 0)
            {
                g.FillPolygon(new SolidBrush(average), new Point[] {
                    new Point((int)(x + Math.Cos(startAngle) * startR), (int)(y + Math.Sin(startAngle) * startR)),
                    new Point((int)(x + Math.Cos(startAngle) * endR), (int)(y + Math.Sin(startAngle) * endR)),
                    new Point((int)(x + Math.Cos(endAngle) * endR), (int)(y + Math.Sin(endAngle) * endR)),
                    new Point((int)(x + Math.Cos(endAngle) * startR), (int)(y + Math.Sin(endAngle) * startR))
                });
            }
            else
            {
                Point[] points = new Point[4 + pieces.Count];
                points[0] = new Point((int)(x + Math.Cos(startAngle) * startR), (int)(y + Math.Sin(startAngle) * startR));
                points[1] = new Point((int)(x + Math.Cos(startAngle) * endR), (int)(y + Math.Sin(startAngle) * endR));
                points[points.Length - 2] = new Point((int)(x + Math.Cos(endAngle) * endR), (int)(y + Math.Sin(endAngle) * endR));
                points[points.Length - 1] = new Point((int)(x + Math.Cos(endAngle) * startR), (int)(y + Math.Sin(endAngle) * startR));

                double angleMovement = (endAngle - startAngle) / (pieces.Count);
                double angle = startAngle;

                for (int i = 0; i < pieces.Count; i++)
                {
                    pieces[i].render(g, x, y, endR, endR * 2 - startR, angle, angle + angleMovement);

                    angle += angleMovement;
                    points[i+2] = new Point((int)(x + Math.Cos(angle) * endR), (int)(y + Math.Sin(angle) * endR));
                }

                g.FillPolygon(new SolidBrush(piece.c), points);
                
            }
        }

    }
}
