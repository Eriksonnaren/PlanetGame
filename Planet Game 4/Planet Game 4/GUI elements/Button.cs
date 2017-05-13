using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Planet_Game_4
{
    public class Button : GUI_element
    {

        EventHandler onClick;
        EventHandler onRelease;

        public Button(EventHandler onClick)
        {
            this.onClick = onClick;
        }

        public void Clicked()
        {
            if(onClick != null)
            {
                onClick.Invoke(null, null);
            }
        }


        public bool onElement(float pointX, float pointY, float x, float y, float width, float height)
        {
            return (pointX >= x && pointY >= y && pointX <= x + width && pointY <= y + height);
        }

        // Render the button
        public void Render(Graphics G, float x, float y, float width, float height)
        {

            G.FillRectangle(new SolidBrush(Color.Blue), x, y, width, height);

        }

    }
}
