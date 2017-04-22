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
            this.planets.Add(new Planet(new Vector(500, 300), 150, 20, Planet.planet_type.rock));
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
