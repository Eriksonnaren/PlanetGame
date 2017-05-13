using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Planet_Game_4
{
    public class GUI_Element_Test : ui
    {

        public Graphics Graphics { get; set; }

        Form1 parent;

        public List<GUI_element> elements;

        float elementHeight = 80;
        float elementWidth = 100;

        float elementSpacing = 10;

        public GUI_Element_Test(Graphics G, Form1 parent)
        {
            elements = new List<GUI_element>();

            elements.Add(new Button(buttonPressed));
            elements.Add(new Button(buttonPressed));
            elements.Add(new Button(buttonPressed));
            elements.Add(new Button(buttonPressed));
            elements.Add(new Button(buttonPressed));

            this.Graphics = G;

            this.parent = parent;

        }

        void buttonPressed(object sender, EventArgs e)
        {
            Console.WriteLine("Pressed");
        }

        // Updates the interface, like physics or something
        public void update()
        {

        }

        // Animate the menu
        public void animate(double speed)
        {

        }

        // Displays the menu
        public void show()
        {
            Graphics.Clear(Color.White);

            for(int i = 0; i < elements.Count(); i++)
            {
                elements[i].Render(Graphics, 0, i * (elementHeight + elementSpacing), elementWidth, elementHeight);
            }
        }

        // Some functions that can be useful when updating the menu
        public void resize() { }
        public MouseButtons mouseDown { get; set; }
        public void mouseHold(MouseButtons B) { }
        public void mousePressed(MouseButtons B)
        {
            for (int i = 0; i < elements.Count(); i++)
            {
                if(elements[i].onElement((float)parent.MousePos.X, (float)parent.MousePos.Y, 0, i * (elementHeight+elementSpacing), elementWidth, elementHeight))
                {
                    elements[i].Clicked();

                }
            }
        }
        public void mouseReleased(MouseButtons B) { }
        public void mouseWheel(object sender, MouseEventArgs e) { }
        public void keyPressed(char key) { }
        public void keyReleased(char key) { }

    }
}
