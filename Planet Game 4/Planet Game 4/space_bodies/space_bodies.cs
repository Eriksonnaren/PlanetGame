using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planet_Game_4
{
    public interface space_bodies
    {
        

        Vector position { get; set; }
        
        double radius { get; set; }
        double mass { get; set; }

    }
}
