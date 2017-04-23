using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planet_Game_4
{
    public class universe
    {

        public List<SpaceBody> bodies;

        /// <summary>
        /// The universe will generate without any specific seed and stuff
        /// </summary>
        public universe()
        {

            bodies = Generate(new Vector(0,0),1,250000000,Math.Pow(10,26));
            Orbit O = new Orbit(bodies[1]);
            O.Generate(0,0,O.Parent.radius*7,0,true);
            bodies.Add(new SpaceBody(O,bodies[1].radius/2,5,SpaceBody.Body_type.rock, Math.Pow(10, 22)));
            O = new Orbit(bodies[2]);
            O.Generate(0, 0, O.Parent.radius * 5, 3, true);
            bodies.Add(new SpaceBody(O, bodies[2].radius / 2, 5, SpaceBody.Body_type.rock, Math.Pow(10, 21)));
            bodies[2].rings.refresh(RingSystem.RingType.Rock);
            bodies[3].rings.refresh(RingSystem.RingType.Ice);
        }

        /// <summary>
        /// Put some planets in a universe. The rest of the universe will generate automagically.
        /// </summary>
        /// <param name="planets"></param>
        public universe(List<SpaceBody> planets)
        {
            this.bodies = planets;
        }
        public List<SpaceBody> Generate(Vector Pos,int PlanetAmount,double SunSize,double SunMass)
        {
            SpaceBody Sun = new SpaceBody(Pos, SunSize, 10, SpaceBody.Body_type.sun,SunMass);
            double Dist = 10 * Math.Pow(10, 9);
            List<SpaceBody> Bodys=new List<SpaceBody> { Sun };
            
            for (int i = 0; i < PlanetAmount; i++)
            {
                int MoonAmount = 0;
                SpaceBody Planet = getPlanet(Sun, i, Dist, 600000);
                double MoonDist = 30;
                Bodys.Add(Planet);
                for (int j = 0; j < MoonAmount; j++)
                {
                    Bodys.Add(getPlanet(Planet,j,MoonDist,6));
                    MoonDist *= 2.2;
                }
                
                
                Dist *= 2.2;
            }
            return Bodys;
        }
        SpaceBody getPlanet(SpaceBody Parent, int id,double Dist,double Size)
        {
            Orbit O = new Orbit(Parent);
            int MoonAmount = id;
            O.Generate(Form1.rnd.NextDouble() * (0 / Math.Sqrt(id + 1)), Form1.rnd.NextDouble() * Math.PI * 2, Dist, Form1.rnd.NextDouble() * Math.PI * 2,true);
            SpaceBody Planet = new SpaceBody(O, Size, 5, SpaceBody.Body_type.rock, 5*Math.Pow(10,22));
            return Planet;
        }

    }
}
