using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planet_Game_4
{
    public interface infoObject
    {

        bool mouseOn(Vector mouse);
        void show(Graphics g, int x, int y, int sx, int sy);

        String getName();
        String[] getInterestingInfo();

    }
}
