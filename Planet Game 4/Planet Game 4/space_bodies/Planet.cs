using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planet_Game_4
{
    public class Planet : ISpace_bodies
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

        public body_shape shape;

        public Planet(Vector pos, double radius, planet_type type)
        {
            position = pos;
            this.type = type;

            this.radius = radius;

            mass = 10000;
            shape = new body_shape(4);
        }

        /// <summary>
        /// PLANET PHYSICS and other stuff too
        /// </summary>
        public void update()
        {
            
        }

        public static void show(Graphics g, Planet p)
        {

            p.shape.render(g, (int)p.position.X, (int)p.position.Y, (int)p.radius);

        }

    }
}
