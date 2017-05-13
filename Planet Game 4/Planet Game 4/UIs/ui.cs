using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Planet_Game_4
{
    public interface ui
    {
        // The graphics object
        Graphics Graphics { get; set; }

        // Updates the interface, like physics or something
        void update();

        // Animate the menu
        void animate(double speed);

        // Displays the menu
        void show();

        // Some functions that can be useful when updating the menu
        void resize();
        MouseButtons mouseDown { get; set; }
        void mouseHold(MouseButtons B);
        void mousePressed(MouseButtons B);
        void mouseReleased(MouseButtons B);
        void mouseWheel(object sender,MouseEventArgs e);
        void keyPressed(char key);
        void keyReleased(char key);

    }
}
