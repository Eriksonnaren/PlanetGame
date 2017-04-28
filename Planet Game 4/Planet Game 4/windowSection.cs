using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planet_Game_4
{
    public class windowSection
    {

        public Vector min;
        public Vector max;
        
        public Vector size { get
            {
                return max - min;
            } }

        public windowSection(Vector min, Vector max)
        {
            this.max = max;
            this.min = min;
        }

    }
}
