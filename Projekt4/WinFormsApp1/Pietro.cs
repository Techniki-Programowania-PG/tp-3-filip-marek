using System.Collections.Generic;

namespace WinFormsApp1
{
    public class Pietro
    {
        public int pietroX, pietroY, pietroWidth;
        public List<Ludzik> ludzie = new List<Ludzik>();

        public Pietro(int x, int y, int w)
        {
            pietroX = x;
            pietroY = y;
            pietroWidth = w;
        }
    }
}
