using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planet_Game_4
{
    public class theGame : ui
    {
        public static double Gravity = 0.0001;
        public static double TileMinimumSize = 3;

        Graphics graphics;

        // The universe that you are playing inside. Isn't that cool?
        public universe universe;

        public theGame(Graphics graphics)
        {
            // You do need an object to display all the stuff to the screen, do you not?
            this.graphics = graphics;

            universe = new universe();

        }

        // Do physics and calculations
        public void update()
        {
            foreach (SpaceBody P in universe.planets)
            {
                P.update();
                if (P.orbit!=null)
                {
                    P.position = P.orbit.getPos();
                }
            }
        }

        // Do visual calculations and display all the things
        public void show()
        {
            graphics.Clear(Color.Black);

            for(int i = universe.planets.Count-1; i >= 0; i--)
            {
                universe.planets[i].show(graphics);
            }

        }

    }
}
