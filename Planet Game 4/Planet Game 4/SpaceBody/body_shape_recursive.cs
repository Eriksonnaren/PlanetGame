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

        public body_shape_recursive tail1 = null;
        public body_shape_recursive tail2 = null;

        public enum tile_type
        {
            quadrupel,
            center
        }

        private tile_type type;
        
        public HSLColor average;

        bool blackAndWhite;

        int level;
        
        public body_shape_recursive(HSLColor baseC, int rv, int gv, int bv, bool blackAndWhite, tile_type type, int req)
        {
            this.blackAndWhite = blackAndWhite;

            double h = baseC.Hue;
            double s = baseC.Saturation + (Form1.rnd.NextDouble()-0.5)*gv*2;
            double l = baseC.Luminosity + (Form1.rnd.NextDouble()-0.5)*bv*2;

            average = new HSLColor(h, s, l);

            this.type = type;

            if (req > 0)
            {
                head1 = (new body_shape_recursive(this.average, rv, gv, bv, blackAndWhite, tile_type.quadrupel, req - 1));
                head2 = (new body_shape_recursive(this.average, rv, gv, bv, blackAndWhite, tile_type.quadrupel, req - 1));

                if (type == tile_type.center)
                {
                    tail1 = (new body_shape_recursive(this.average, rv, gv, bv, blackAndWhite, tile_type.center, req - 1));
                }else if(type == tile_type.quadrupel)
                {
                    tail1 = (new body_shape_recursive(this.average, rv, gv, bv, blackAndWhite, tile_type.quadrupel, req - 1));
                    tail2 = (new body_shape_recursive(this.average, rv, gv, bv, blackAndWhite, tile_type.quadrupel, req - 1));
                }
            }
            else
            {

            }

            level = req;

        }

        public HSLColor calculateAverages()
        {

            if(head1 != null && head2 != null && tail1 != null)
            {
                HSLColor a = head1.calculateAverages();
                HSLColor b = head2.calculateAverages();
                HSLColor c = tail1.calculateAverages();

                /*int ar;
                int ab;
                int ag;*/

                double ar = (a.Hue + b.Hue + c.Hue) / 3;
                double ag = (a.Saturation + b.Saturation + c.Saturation) / 3;
                double ab = (a.Luminosity + b.Luminosity + c.Luminosity) / 3;

                //ar = calculateCoolAverage(a.R, b.R, c.R);
                //ab = calculateCoolAverage(a.B, b.B, c.B);
                //ag = calculateCoolAverage(a.G, b.G, c.G);

                if (blackAndWhite)
                {
                    average = new HSLColor(ar, ar, ar);
                }
                else {
                    average = new HSLColor(ar, ag, ab);
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

        public void render(Graphics g, theGame parent, int x, int y, double startR, double endR, double startAngle, double endAngle, bool doTip, double req)
        {

            // Make sure that this part is inside the screen. Otherwise don't render it
            if(true)
            {
                Vector[] corners = new Vector[]
                {
                    //new Vector(x + Math.Cos(endAngle) * endR, y + Math.Sin(endAngle) * endR),
                    new Vector(x + Math.Cos(startAngle) * startR, y + Math.Sin(startAngle) * startR),
                    new Vector(x + Math.Cos(startAngle) * (endR+2), y + Math.Sin(startAngle) * (endR+2)),
                    new Vector(x + Math.Cos(startAngle/2+endAngle/2) * endR * 1.5, y + Math.Sin(startAngle/2+endAngle/2) * endR * 1.5),
                    new Vector(x + Math.Cos(endAngle) * (endR+2), y + Math.Sin(endAngle) * (endR+2)),
                    new Vector(x + Math.Cos(endAngle) * startR, y + Math.Sin(endAngle) * startR)
                };

                Vector min = new Vector(corners[0]);

                if (corners[1].X < min.X) { min.X = corners[1].X; }
                if (corners[2].X < min.X) { min.X = corners[2].X; }
                if (corners[3].X < min.X) { min.X = corners[3].X; }
                if (corners[4].X < min.X) { min.X = corners[4].X; }
                //if (corners[5].X < min.X) { min.X = corners[5].X; }

                if (corners[1].Y < min.Y) { min.Y = corners[1].Y; }
                if (corners[2].Y < min.Y) { min.Y = corners[2].Y; }
                if (corners[3].Y < min.Y) { min.Y = corners[3].Y; }
                if (corners[4].Y < min.Y) { min.Y = corners[4].Y; }
                //if (corners[5].Y < min.Y) { min.Y = corners[5].Y; }

                Vector max = new Vector(corners[0]);

                if (corners[1].X > max.X) { max.X = corners[1].X; }
                if (corners[2].X > max.X) { max.X = corners[2].X; }
                if (corners[3].X > max.X) { max.X = corners[3].X; }
                if (corners[4].X > max.X) { max.X = corners[4].X; }
                //if (corners[5].X > max.X) { max.X = corners[5].X; }

                if (corners[1].Y > max.Y) { max.Y = corners[1].Y; }
                if (corners[2].Y > max.Y) { max.Y = corners[2].Y; }
                if (corners[3].Y > max.Y) { max.Y = corners[3].Y; }
                if (corners[4].Y > max.Y) { max.Y = corners[4].Y; }
                //if (corners[5].Y > max.Y) { max.Y = corners[5].Y; }

                if (!(min.X <= parent.worldDispMax.X && max.X >= parent.worldDispMin.X && min.Y <= parent.worldDispMax.Y && max.Y >= parent.worldDispMin.Y))
                {
                    return;
                }

            }

            if (head1 == null || head2 == null || tail1 == null || req <= 0)
            {

                if (doTip)
                {
                    g.FillPolygon(new SolidBrush(average), new Point[] {
                        new Point((int)(x + Math.Cos(startAngle) * startR), (int)(y + Math.Sin(startAngle) * startR)),
                        new Point((int)(x + Math.Cos(startAngle) * (endR+2)), (int)(y + Math.Sin(startAngle) * (endR+2))),
                        new Point((int)(x + Math.Cos(endAngle / 2 + startAngle / 2) * (endR+2)), (int)(y + Math.Sin(endAngle / 2 + startAngle / 2) * (endR+2))),
                        new Point((int)(x + Math.Cos(endAngle) * (endR+2)), (int)(y + Math.Sin(endAngle) * (endR+2))),
                        new Point((int)(x + Math.Cos(endAngle) * startR), (int)(y + Math.Sin(endAngle) * startR))
                    });
                }
                else
                {
                    g.FillPolygon(new SolidBrush(average), new Point[] {
                        new Point((int)(x + Math.Cos(startAngle) * startR), (int)(y + Math.Sin(startAngle) * startR)),
                        new Point((int)(x + Math.Cos(startAngle) * (endR+2)), (int)(y + Math.Sin(startAngle) * (endR+2))),
                        new Point((int)(x + Math.Cos(endAngle) * (endR+2)), (int)(y + Math.Sin(endAngle) * (endR+2))),
                        new Point((int)(x + Math.Cos(endAngle) * startR), (int)(y + Math.Sin(endAngle) * startR))
                    });
                }
                
            }
            else
            {
                
                double angleMovement = (endAngle - startAngle) / (2);

                double halfR = startR / 2 + endR / 2;

                if (req < 1)
                {
                    if (type == tile_type.center)
                    {
                        head1.render(g, parent, x, y, halfR, endR, startAngle, startAngle + angleMovement, req - 1, req, true, average);
                        head2.render(g, parent, x, y, halfR, endR, startAngle + angleMovement, endAngle, req - 1, req, true, average);

                        tail1.render(g, parent, x, y, startR, halfR, startAngle, endAngle, req - 1, req, true, average);
                    }
                    else
                    {
                        head1.render(g, parent, x, y, halfR, endR, startAngle, startAngle + angleMovement, req - 1, req, true, average);
                        head2.render(g, parent, x, y, halfR, endR, (startAngle + endAngle) / 2, startAngle + angleMovement * 2, req - 1, req, true, average);

                        tail1.render(g, parent, x, y, startR, halfR, startAngle, startAngle + angleMovement, req - 1, req, true, average);
                        tail2.render(g, parent, x, y, startR, halfR, startAngle + angleMovement, endAngle, req - 1, req, true, average);
                    }
                }
                else {
                    if (type == tile_type.center)
                    {
                        head1.render(g, parent, x, y, halfR, endR, startAngle, startAngle + angleMovement, req - 1, req, true, average);
                        head2.render(g, parent, x, y, halfR, endR, startAngle + angleMovement, endAngle, req - 1, req, true, average);

                        tail1.render(g, parent, x, y, startR, halfR, startAngle, endAngle, req - 1, req, true, average);
                    }
                    else
                    {
                        head1.render(g, parent, x, y, halfR, endR, startAngle, startAngle + angleMovement, req - 1, req, true, average);
                        head2.render(g, parent, x, y, halfR, endR, (startAngle + endAngle) / 2, startAngle + angleMovement * 2, req - 1, req, true, average);

                        tail1.render(g, parent, x, y, startR, halfR, startAngle, startAngle + angleMovement, req - 1, req, true, average);
                        tail2.render(g, parent, x, y, startR, halfR, startAngle + angleMovement, endAngle, req - 1, req, true, average);
                    }
                }
                
            }
        }

        public void render(Graphics g, theGame parent, int x, int y, double startR, double endR, double startAngle, double endAngle, double req, double lerp, bool doTip, HSLColor lerpTo)
        {
            // Make sure that this part is inside the screen. Otherwise don't render it
            if (true)
            {
                Vector[] corners = new Vector[]
                {
                    //new Vector(x + Math.Cos(endAngle) * endR, y + Math.Sin(endAngle) * endR),
                    new Vector(x + Math.Cos(startAngle) * startR, y + Math.Sin(startAngle) * startR),
                    new Vector(x + Math.Cos(startAngle) * endR, y + Math.Sin(startAngle) * endR),
                    new Vector(x + Math.Cos(startAngle/2+endAngle/2) * endR * 1.5, y + Math.Sin(startAngle/2+endAngle/2) * endR * 1.5),
                    new Vector(x + Math.Cos(endAngle) * endR, y + Math.Sin(endAngle) * endR),
                    new Vector(x + Math.Cos(endAngle) * startR, y + Math.Sin(endAngle) * startR)
                };

                Vector min = new Vector(corners[0]);

                if (corners[1].X < min.X) { min.X = corners[1].X; }
                if (corners[2].X < min.X) { min.X = corners[2].X; }
                if (corners[3].X < min.X) { min.X = corners[3].X; }
                if (corners[4].X < min.X) { min.X = corners[4].X; }
                //if (corners[5].X < min.X) { min.X = corners[5].X; }

                if (corners[1].Y < min.Y) { min.Y = corners[1].Y; }
                if (corners[2].Y < min.Y) { min.Y = corners[2].Y; }
                if (corners[3].Y < min.Y) { min.Y = corners[3].Y; }
                if (corners[4].Y < min.Y) { min.Y = corners[4].Y; }
                //if (corners[5].Y < min.Y) { min.Y = corners[5].Y; }

                Vector max = new Vector(corners[0]);

                if (corners[1].X > max.X) { max.X = corners[1].X; }
                if (corners[2].X > max.X) { max.X = corners[2].X; }
                if (corners[3].X > max.X) { max.X = corners[3].X; }
                if (corners[4].X > max.X) { max.X = corners[4].X; }
                //if (corners[5].X > max.X) { max.X = corners[5].X; }

                if (corners[1].Y > max.Y) { max.Y = corners[1].Y; }
                if (corners[2].Y > max.Y) { max.Y = corners[2].Y; }
                if (corners[3].Y > max.Y) { max.Y = corners[3].Y; }
                if (corners[4].Y > max.Y) { max.Y = corners[4].Y; }
                //if (corners[5].Y > max.Y) { max.Y = corners[5].Y; }

                if (!(min.X <= parent.worldDispMax.X && max.X >= parent.worldDispMin.X && min.Y <= parent.worldDispMax.Y && max.Y >= parent.worldDispMin.Y))
                {
                    return;
                }

            }

            lerp = Form1.constrain(lerp, 0, 1);

            if (head1 == null || head2 == null || tail1 == null || req <= 0)
            {
                HSLColor c = new HSLColor(Form1.lerp(lerpTo.Hue, average.Hue, lerp), Form1.lerp(lerpTo.Saturation, average.Saturation, lerp), Form1.lerp(lerpTo.Luminosity, average.Luminosity, lerp));

                if (doTip)
                {
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
                    g.FillPolygon(new SolidBrush(c), new Point[] {
                        new Point((int)(x + Math.Cos(startAngle) * startR), (int)(y + Math.Sin(startAngle) * startR)),
                        new Point((int)(x + Math.Cos(startAngle) * (endR+2)), (int)(y + Math.Sin(startAngle) * (endR+2))),
                        new Point((int)(x + Math.Cos(endAngle) * (endR+2)), (int)(y + Math.Sin(endAngle) * (endR+2))),
                        new Point((int)(x + Math.Cos(endAngle) * startR), (int)(y + Math.Sin(endAngle) * startR))
                    });
                }
            }
            else
            {

                double angleMovement = (endAngle - startAngle) / (2);

                double halfR = startR / 2 + endR / 2;

                //head1.render(g, x, y, halfR, endR, startAngle, startAngle + angleMovement, req - 1, lerp, true, lerpTo);
                    if (type == tile_type.center)
                    {
                        head1.render(g, parent, x, y, halfR, endR, startAngle, startAngle + angleMovement, req - 1, req, true, average);
                        head2.render(g, parent, x, y, halfR, endR, startAngle + angleMovement, endAngle, req - 1, req, true, average);

                        tail1.render(g, parent, x, y, startR, halfR, startAngle, endAngle, req - 1, req, true, average);
                    }
                    else
                    {
                        head1.render(g, parent, x, y, halfR, endR, startAngle, startAngle + angleMovement, req - 1, req, true, average);
                        head2.render(g, parent, x, y, halfR, endR, (startAngle + endAngle) / 2, startAngle + angleMovement * 2, req - 1, req, true, average);

                        tail1.render(g, parent, x, y, startR, halfR, startAngle, startAngle + angleMovement, req - 1, req, true, average);
                        tail2.render(g, parent, x, y, startR, halfR, startAngle + angleMovement, endAngle, req - 1, req, true, average);
                    }

                //head2.render(g, x, y, halfR, endR, Form1.lerp(startAngle, endAngle, 0.5), startAngle + angleMovement * 2, req - 1, lerp, true, lerpTo);

                //tail1.render(g, x, y, startR, halfR, startAngle, endAngle, req - 1, lerp, true, lerpTo);

            }
        }

    }
}
