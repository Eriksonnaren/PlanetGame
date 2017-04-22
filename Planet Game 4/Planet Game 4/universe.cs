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

            //this.planets = new List<SpaceBody>();
            bodies = Generate(new Vector(0,0),3,20,100000);
            /*SpaceBody P1 = new SpaceBody(new Vector(500, 300), 50, 10,SpaceBody.Body_type.sun);
            Orbit O = new Orbit(P1);
            O.Generate(0.5,0,200,1);
            SpaceBody P2 = new SpaceBody(O, 25, 5, SpaceBody.Body_type.rock);

            this.planets.Add(P1);
            this.planets.Add(P2);*/
            

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
            SpaceBody Sun = new SpaceBody(Pos, SunSize, 30, SpaceBody.Body_type.sun,SunMass);
            List<SpaceBody> Bodys=new List<SpaceBody> { Sun };
            double Dist = 100;
            for (int i = 0; i < PlanetAmount; i++)
            {
                int MoonAmount = i;
                SpaceBody Planet = getPlanet(Sun, i, Dist, 10);
                double MoonDist = 30;
                Bodys.Add(Planet);
                for (int j = 0; j < MoonAmount; j++)
                {
                    Bodys.Add(getPlanet(Planet,j,MoonDist,6));
                    MoonDist += 20;
                }
                
                
                Dist += 150;
            }
            return Bodys;
        }
        SpaceBody getPlanet(SpaceBody Parent, int id,double Dist,double Size)
        {
            Orbit O = new Orbit(Parent);
            int MoonAmount = id;
            O.Generate(Form1.rnd.NextDouble() * (0.1 / Math.Sqrt(id + 1)), Form1.rnd.NextDouble() * Math.PI * 2, Dist, Form1.rnd.NextDouble() * Math.PI * 2);
            SpaceBody Planet = new SpaceBody(O, Size, (int)(Size / 0.1), SpaceBody.Body_type.rock, 1000);
            return Planet;
        }

    }
}
