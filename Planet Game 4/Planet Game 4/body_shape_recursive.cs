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

        public body_shape_recursive head1 = null;
        public body_shape_recursive head2 = null;
        public body_shape_recursive tail = null;
       
        public Color average;

        public body_shape_recursive(body_shape_piece piece)
        {
            this.average = Color.FromArgb(piece.c.R + Form1.rnd.Next(-55, 55), 0, 0);
        }

        public body_shape_recursive(body_shape_piece piece, int req)
        {
            this.average = Color.FromArgb(piece.c.R + Form1.rnd.Next(-55, 55), 0, 0);

            if (req > 0)
            {
                head1 = (new body_shape_recursive(piece, req - 1));
                head2 = (new body_shape_recursive(piece, req - 1));
                tail = (new body_shape_recursive(piece, req - 1));
            }
            else
            {

            }

        }

        public Color calculateAverages()
        {
            if(head1 != null && head2 != null && tail != null)
            {
                Color a = head1.calculateAverages();
                Color b = head2.calculateAverages();
                Color c = tail.calculateAverages();

                int ar = (a.R + b.R + c.R) / 3;
                int ab = (a.B + b.B + c.B) / 3;
                int ag = (a.G + b.G + c.G) / 3;

                average = Color.FromArgb(ar, ab, ag);
                return average;
            }
            else
            {
                return average;
            }
        }

        public void render(Graphics g, int x, int y, double startR, double endR, double startAngle, double endAngle, double req)
        {
            if (head1 == null || head2 == null || tail == null || req <= 0)
            {
                g.FillPolygon(new SolidBrush(average), new Point[] {
                    new Point((int)(x + Math.Cos(startAngle) * startR), (int)(y + Math.Sin(startAngle) * startR)),
                    new Point((int)(x + Math.Cos(startAngle) * endR), (int)(y + Math.Sin(startAngle) * endR)),
                    new Point((int)(x + Math.Cos(endAngle / 2 + startAngle / 2) * endR), (int)(y + Math.Sin(endAngle / 2 + startAngle / 2) * endR)),
                    new Point((int)(x + Math.Cos(endAngle) * endR), (int)(y + Math.Sin(endAngle) * endR)),
                    new Point((int)(x + Math.Cos(endAngle) * startR), (int)(y + Math.Sin(endAngle) * startR))
                });
            }
            else
            {
                
                double angleMovement = (endAngle - startAngle) / (2);

                double halfR = startR / 2 + endR / 2;

                if (req < 1)
                {
                    head1.render(g, x, y, halfR, endR, startAngle, startAngle + angleMovement, req - 1, req, average);
                    head2.render(g, x, y, halfR, endR, Form1.lerp(startAngle, endAngle, 0.5), startAngle + angleMovement * 2, req - 1, req, average);

                    tail.render(g, x, y, startR, halfR, startAngle, endAngle, req - 1, req, average);
                }
                else {
                    head1.render(g, x, y, halfR, endR, startAngle, startAngle + angleMovement, req - 1);
                    head2.render(g, x, y, halfR, endR, Form1.lerp(startAngle, endAngle, 0.5), startAngle + angleMovement * 2, req - 1);

                    tail.render(g, x, y, startR, halfR, startAngle, endAngle, req - 1);
                }
                
            }
        }

        public void render(Graphics g, int x, int y, double startR, double endR, double startAngle, double endAngle, double req, double lerp, Color lerpTo)
        {
            if (head1 == null || head2 == null || tail == null || req <= 0)
            {
                Color c = Color.FromArgb(Form1.lerp(lerpTo.R, average.R, lerp), Form1.lerp(lerpTo.G, average.G, lerp), Form1.lerp(lerpTo.B, average.B, lerp));

                g.FillPolygon(new SolidBrush(c), new Point[] {
                    new Point((int)(x + Math.Cos(startAngle) * startR), (int)(y + Math.Sin(startAngle) * startR)),
                    new Point((int)(x + Math.Cos(startAngle) * endR), (int)(y + Math.Sin(startAngle) * endR)),
                    new Point((int)(x + Math.Cos(endAngle / 2 + startAngle / 2) * endR), (int)(y + Math.Sin(endAngle / 2 + startAngle / 2) * endR)),
                    new Point((int)(x + Math.Cos(endAngle) * endR), (int)(y + Math.Sin(endAngle) * endR)),
                    new Point((int)(x + Math.Cos(endAngle) * startR), (int)(y + Math.Sin(endAngle) * startR))
                });
            }
            else
            {

                double angleMovement = (endAngle - startAngle) / (2);

                double halfR = startR / 2 + endR / 2;

                head1.render(g, x, y, halfR, endR, startAngle, startAngle + angleMovement, req - 1, lerp, lerpTo);
                head2.render(g, x, y, halfR, endR, Form1.lerp(startAngle, endAngle, 0.5), startAngle + angleMovement * 2, req - 1, lerp, lerpTo);

                tail.render(g, x, y, startR, halfR, startAngle, endAngle, req - 1, lerp, lerpTo);

            }
        }

    }
}
