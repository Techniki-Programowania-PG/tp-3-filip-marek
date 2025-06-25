using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public class Ludzik
    {
        public int cel;
        public int actual;
        public PictureBox pb;

        public Ludzik(int act, int c, Form1 parent, List<Pietro> pietra)
        {
            actual = act;
            cel = c;
            pb = parent.DodajLudzikaGraficznie(act, pietra);
            parent.dodajdokolejki(act, c);
        }

        ~Ludzik()
        {
            if (pb != null && pb.Parent != null)
            {
                pb.Parent.Controls.Remove(pb);
                pb.Dispose();
            }
        }
    }
}
