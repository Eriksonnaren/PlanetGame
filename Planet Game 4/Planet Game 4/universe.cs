using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planet_Game_4
{
    public class universe
    {

        public List<Planet> planets;

        /// <summary>
        /// The universe will generate without any specific seed and stuff
        /// </summary>
        public universe()
        {
            this.planets = new List<Planet>();
            Planet P1 = new Planet(new Vector(500, 300), 50, 10, Planet.planet_type.rock);
            Orbit O = new Orbit(P1);
            O.Generate(0,1,200,1);
            Planet P2 = new Planet(O, 25, 5, Planet.planet_type.rock);
            this.planets.Add(P1);
            this.planets.Add(P2);
            
        }

        /// <summary>
        /// Put some planets in a universe. The rest of the universe will generate automagically.
        /// </summary>
        /// <param name="planets"></param>
        public universe(List<Planet> planets)
        {
            this.planets = planets;
        }

    }
}
