using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planet_Game_4
{
    public class Planet : space_bodies
    {

        public enum planet_type
        {
            life,
            gas,
            rock,
            ice
        }

        public planet_type type;

        public Orbit orbit;
        public Vector position { get; set; }
        public Vector velocity;

        public double radius { get; set; }
        public double mass { get; set; }

        public Planet(Vector pos, double radius, planet_type type)
        {
            position = pos;
            this.type = type;

            this.radius = radius;
            mass = 10000;
        }

        public void update(double dt)
        {
            
        }

        public static void show(Graphics g, Planet p)
        {
            
            // Temporary fill thing
            g.FillEllipse(new SolidBrush(Color.DarkCyan), (int)(p.position.X - p.radius), (int)(p.position.Y - p.radius), (int)p.radius*2, (int)p.radius*2);

        }

    }
}
