using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Planet_Game_4
{
    public interface window
    {

        Bitmap I { get; set; }
        Graphics G { get; set; }

        windowSection section { get; set; }

        void update();
        void render();

        void resize(Vector size);

    }
}
