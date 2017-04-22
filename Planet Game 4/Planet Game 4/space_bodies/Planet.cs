using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planet_Game_4
{
    public class Planet : space_bodies
    {

        public Orbit orbit;
        public Vector position { get; set; }
        public Vector velocity;

        public double radius { get; set; }
        public double mass { get; set; }

        public Planet(Vector pos)
        {
            position = pos;
        }

        public void update(double dt)
        {
            
        }

        public void show()
        {

        }

    }
}
