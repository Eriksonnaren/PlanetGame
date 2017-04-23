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

        bool blackAndWhite;

        int level;
        
        public body_shape_recursive(Color baseC, int rv, int gv, int bv, bool blackAndWhite, int req)
        {
            this.blackAndWhite = blackAndWhite;

            int r = (int)Form1.constrain(baseC.R + Form1.rnd.Next(-rv, rv) - 0, 0, 255);
            int g = (int)Form1.constrain(baseC.G + Form1.rnd.Next(-gv, gv) - 0, 0, 255);
            int b = (int)Form1.constrain(baseC.B + Form1.rnd.Next(-bv, bv) - 0, 0, 255);

            average = Color.FromArgb(r, g, b);

            if (req > 0)
            {
                head1 = (new body_shape_recursive(this.average, rv, gv, bv, blackAndWhite, req - 1));
                head2 = (new body_shape_recursive(this.average, rv, gv, bv, blackAndWhite, req - 1));
                tail = (new body_shape_recursive(this.average, rv, gv, bv, blackAndWhite, req - 1));
            }
            else
            {

            }

            level = req;

        }

        public Color calculateAverages()
        {
            if(head1 != null && head2 != null && tail != null)
            {
                Color a = head1.calculateAverages();
                Color b = head2.calculateAverages();
                Color c = tail.calculateAverages();

                int ar;
                int ab;
                int ag;

                ar = (a.R + b.R + c.R) / 3;
                ag = (a.G + b.G + c.G) / 3;
                ab = (a.B + b.B + c.B) / 3;

                //ar = calculateCoolAverage(a.R, b.R, c.R);
                //ab = calculateCoolAverage(a.B, b.B, c.B);
                //ag = calculateCoolAverage(a.G, b.G, c.G);

                if (blackAndWhite)
                {
                    average = Color.FromArgb(ar, ar, ar);
                }
                else {
                    average = Color.FromArgb(ar, ag, ab);
                }
                return average;
            }
            else
            {
                return average;
            }
        }

        public static int calculateCoolAverage(int a, int b, int c)
        {
            int max = a;
            
            if (b > max) { max = b; }
            if (c > max) { max = c; }

            int min = a;
            
            if (b < min) { max = b; }
            if (c < min) { max = c; }

            int mid = max / 2 + min / 2;
            int areUnder = 0;
            
            if (a < mid) { areUnder++; }
            if (b < mid) { areUnder++; }
            if (c < mid) { areUnder++; }

            if(areUnder >= 2)
            {
                return min;
            }

            return max;
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
                    new Point((int)(x + Math.Cos(startAngle) * (endR+2)), (int)(y + Math.Sin(startAngle) * (endR+2))),
                    new Point((int)(x + Math.Cos(endAngle / 2 + startAngle / 2) * (endR+2)), (int)(y + Math.Sin(endAngle / 2 + startAngle / 2) * (endR+2))),
                    new Point((int)(x + Math.Cos(endAngle) * (endR+2)), (int)(y + Math.Sin(endAngle) * (endR+2))),
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
