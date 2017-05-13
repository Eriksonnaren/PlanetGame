using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planet_Game_4
{
    public interface GUI_element
    {

        void Clicked();

        // Show the gui element
        void Render(Graphics G, float x, float y, float width, float height);

        bool onElement(float pointX, float pointY, float x, float y, float width, float height);

    }
}
