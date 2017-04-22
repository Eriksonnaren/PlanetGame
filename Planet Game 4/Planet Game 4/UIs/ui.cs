using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planet_Game_4
{
    public interface ui
    {
        // Updates the interface, like physics or something
        void update();

        // Displays the menu
        void show();

        // Some functions that can be useful when updating the menu
        void resize();
        void mousePressed();
        void mouseReleased();
        void mouseWheel(double delta);
        void keyPressed(char key);
        void keyReleased(char key);

    }
}
