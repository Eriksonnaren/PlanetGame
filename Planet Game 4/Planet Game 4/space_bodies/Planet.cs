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
        public double rotation { get; set; }

        public body_shape shape;

        public Planet(Vector pos, double radius, int layers, planet_type type)
        {
            position = pos;
            this.type = type;
            this.radius = radius;
            mass = 10000;
            shape = new body_shape(layers);
        }
        public Planet(Orbit orbit, double radius, int layers, planet_type type)
        {
            this.orbit = orbit;
            this.type = type;
            this.radius = radius;
            mass = 10000;
            shape = new body_shape(layers);
        }

        /// <summary>
        /// PLANET PHYSICS and other stuff too
        /// </summary>
        public void update()
        {
            rotation += 0.01;
            if (orbit != null)
            {
                orbit.update(50);
                position = orbit.getPos();
                
            }
        }

        public static void show(Graphics g, Planet p)
        {
            if (p.orbit != null)
            {
                p.orbit.Show(g);
            }
            p.shape.render(g, (int)p.position.X, (int)p.position.Y, (int)p.radius, p.rotation);

        }

    }
}
