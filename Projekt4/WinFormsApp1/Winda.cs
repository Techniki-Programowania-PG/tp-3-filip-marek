using System.Collections.Generic;

namespace WinFormsApp1
{
    public class Winda
    {
        public int WindaX { get; set; }
        public int WindaY { get; set; }
        public int WindaWidth { get; set; }
        public int WindaHeight { get; set; }
        public bool dogory = true;
        public int waga = 0;
        public int? aktualnePietro = null;

        public List<Ludzik> ludzie = new List<Ludzik>();
        public Queue<Ludzik> zalegle = new Queue<Ludzik>();

        public Winda(int windaX, int windaY, int windaWidth, int windaHeight)
        {
            WindaX = windaX;
            WindaY = windaY;
            WindaWidth = windaWidth;
            WindaHeight = windaHeight;
        }
    }
}
