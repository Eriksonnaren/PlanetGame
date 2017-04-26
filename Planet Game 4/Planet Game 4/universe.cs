﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planet_Game_4
{
    public class universe
    {

        /// <summary>
        /// All the bodies in the universe
        /// </summary>
        public List<SpaceBody> bodies;

        /// <summary>
        /// The universe will generate without any specific seed
        /// </summary>
        public universe()
        {
            // Generate the bodies
            bodies = Generate(new Vector(0,0),10,250000000,Math.Pow(10,26));
            
            // Make an orbit of some sort.
            Orbit O = new Orbit(bodies[1]);
            // Generate that orbit
            O.Generate(0,0,O.Parent.radius*4,0.3,true);
            // Add a new body with that orbit.
            bodies.Add(new SpaceBody(O,bodies[1].radius/2,5,SpaceBody.Body_type.rock, Math.Pow(10, 22), RingSystem.RingType.Empty));
        }

        /// <summary>
        /// Put some planets in a universe. The rest of the universe will generate automagically.
        /// </summary>
        /// <param name="planets"></param>
        public universe(List<SpaceBody> planets)
        {
            // Copy the planet array past in to the bodies array in the universe
            this.bodies = planets;
        }
        
        /// <summary>
        /// Generates a solar system
        /// </summary>
        public List<SpaceBody> Generate(Vector Pos,int PlanetAmount,double SunSize,double SunMass)
        {
            // Make a sun
            SpaceBody Sun = new SpaceBody(Pos, SunSize, 10, SpaceBody.Body_type.sun,SunMass);
            
            // Get an initial distance from the sun to use
            double Dist = 10 * Math.Pow(10, 9);
            
            // Initiate a list of bodies, with the sun inside too.
            List<SpaceBody> Bodys=new List<SpaceBody> { Sun };
            
            // Run this loop for as many planets as there are and then add a planet in each cycle
            for (int i = 0; i < PlanetAmount; i++)
            {
                // The amount of moons the planet is going to have
                int MoonAmount = 0;
                
                // The planet that the game is going to add
                SpaceBody Planet = getPlanet(Sun, i, Dist, 600000);
                
                // The distance the first moon is going to have from the planet
                double MoonDist = 30;
                
                // Add the planet to the bodies array
                Bodys.Add(Planet);
                
                // Loop through this loop as many times as there are moons and add a moon each iteration
                for (int j = 0; j < MoonAmount; j++)
                {
                    // Add the moon to the array
                    Bodys.Add(getPlanet(Planet,j,MoonDist,6));
                    
                    // Increase the moon distance
                    MoonDist *= 2.2;
                }
                
                // Increase the planet distance from the sun
                Dist *= 2.2;
            }
            
            // Return all the bodies in the solar system
            return Bodys;
        }
        
        /// <summary>
        /// Generates a planet orbiting a parent body.
        /// </summary>
        SpaceBody getPlanet(SpaceBody Parent, int id,double Dist,double Size)
        {
            // Get a new orbit orbiting the parent
            Orbit O = new Orbit(Parent);
            
            // Make a moonamount?
            int MoonAmount = id;
            
            // Generate the orbit with a lot of wierd space-math
            O.Generate(Form1.rnd.NextDouble() * (0 / Math.Sqrt(id + 1)), Form1.rnd.NextDouble() * Math.PI * 0, Dist, Form1.rnd.NextDouble() * Math.PI * 0,true);
            
            // Make a planet with that orbit
            SpaceBody Planet = new SpaceBody(O, Size, 5, SpaceBody.Body_type.rock, 5*Math.Pow(10,22), RingSystem.RingType.Ice);
            
            // Return the planet
            return Planet;
        }

    }
}
