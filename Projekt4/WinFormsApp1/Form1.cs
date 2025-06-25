using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        //rozmiary windy
        private const int WindaWidth = 200;
        private const int WindaHeight = 100;

        private int WindaY;
        private int WindaX;

        private const int step = 3;
        private int counter = 0;
        private PriorityQueue<int, int>[] queues = new[]//tablica kolejek, 0- do gory, 1- w dol, 2- do gory oczekujacy, 3- w dol oczekujacy
        {
            new PriorityQueue<int,int>(),
            new PriorityQueue<int,int>(),
            new PriorityQueue<int,int>(),
            new PriorityQueue<int,int>()
        };
        private List<Pietro> pietra = new List<Pietro>();//kazde pietro jest osobnym obiektem, ktory przechowuje ludzikow na nim i ich pozycje
        private Queue<Ludzik> kolejkacelow = new Queue<Ludzik>();
        private Winda winda;


        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }
        private void Form1_Load(object sender, EventArgs e)//poczatkowe ustawienia, ustawienie przyciskow i pieter
        {
            timer1.Interval = 50;
            WindaY = panel5.Location.Y - WindaHeight;
            WindaX = (this.ClientSize.Width - WindaWidth) / 2;
            panel1.Location = new Point(0, 150);
            panel1.Size = new Size(WindaX, 2);

            panel2.Location = new Point(WindaX + WindaWidth, 250);
            panel2.Size = new Size(WindaX, 2);

            panel3.Location = new Point(0, 350);
            panel3.Size = new Size(WindaX, 2);

            panel4.Location = new Point(WindaX + WindaWidth, 450);
            panel4.Size = new Size(WindaX, 2);

            panel5.Location = new Point(0, 550);
            panel5.Size = new Size(WindaX, 2);

            foreach (Control ctrl in this.Controls)//zdarzenie klikniecia przyciskow
            {
                if (ctrl is Button btn && btn.Name.StartsWith("button"))
                {
                    btn.Click += buttonclick;
                }
            }
            winda = new Winda(WindaX, WindaY, WindaWidth, WindaHeight);
            pietra.Add(new Pietro(panel5.Location.X, panel5.Location.Y, panel5.Width));
            pietra.Add(new Pietro(panel4.Location.X, panel4.Location.Y, panel4.Width));
            pietra.Add(new Pietro(panel3.Location.X, panel3.Location.Y, panel3.Width));
            pietra.Add(new Pietro(panel2.Location.X, panel2.Location.Y, panel2.Width));
            pietra.Add(new Pietro(panel1.Location.X, panel1.Location.Y, panel1.Width));
        }

        private void buttonclick(object sender, EventArgs e) // Obsługuje kliknięcia przycisków do dodawania ludzików
        {
            var btn = (Button)sender;
            var parts = btn.Name.Replace("button", "").Split('_');
            if (parts.Length == 2 && int.TryParse(parts[0], out int curr) && int.TryParse(parts[1], out int dest))
                pietra[curr].ludzie.Add(new Ludzik(curr, dest, this, pietra));
        }

        private void timer1_Tick(object sender, EventArgs e)//funkcja wykonywana co 50ms (timer1.Interval), odpowiada za ruch windy i ludzikow oraz aktualizacje kolejek
        {
            winda.waga = winda.ludzie.Count() * 70;
            wybierzcel();
            windarusz();
            textBox1.Text = (winda.waga).ToString(); //wyswietlanie wagi calkowitej pasazerów w windzie 
            Invalidate();
        }
        private bool kolejkazawiera(PriorityQueue<int, int> queue, int value)//sprawdza czy kolejka zawiera wartosc
        {
            foreach (var entry in queue.UnorderedItems)
            {
                if (entry.Element == value)
                    return true;
            }
            return false;
        }
        private bool niemaludzikow()//sprawdza istnienie ludzikow w calym budunku
        {
            foreach (var p in pietra)
            {
                if (p.ludzie.Count > 0)
                {
                    return false;
                }
            }
            if (winda.ludzie.Count>0 || winda.zalegle.Count >0)
            {
                return false;
            }
            return true;
        }
        private void sprawdzczypusto() //czysci kolejki i resetuje winde jesli nigdzie nie ma ludzikow
        {
            if (niemaludzikow())
            {
                queues[0].Clear();
                queues[1].Clear();
                queues[2].Clear();
                queues[3].Clear();
                winda.aktualnePietro = null; // Resetujemy aktualne piętro windy
                winda.dogory = true; 
            }
        }
        private void wybierzcel()
        {
            if (winda.waga < 530 && winda.zalegle.Count > 0) //przypomnienie o ludzikach ktore sie nie zmiescily do windy 
            {
                var toenter = new List<Ludzik>(winda.zalegle);
                foreach (var l in toenter)
                {
                    dodajdokolejki(l.actual, l.cel,true);
                    winda.zalegle.Dequeue();
                    pietra[l.actual].ludzie.Add(l);
                }
            }

            if (kolejkacelow.Count > 0)
            {
                int licznik = kolejkacelow.Count;
                for (int i = 0; i < licznik; i++)
                {
                    var l = kolejkacelow.Dequeue();

                    if (winda.dogory)
                    {
                        if (!kolejkazawiera(queues[0], l.cel))
                            queues[0].Enqueue(l.cel, l.cel);
                    }
                    else
                    {
                        if (!kolejkazawiera(queues[1], l.cel))
                            queues[1].Enqueue(l.cel, -l.cel);
                    }
                }
            }
            if (queues[0].Count > 0 && !(!winda.dogory && queues[1].Count > 0))
            {
                winda.dogory = true;
                winda.aktualnePietro = queues[0].Peek();
                return;
            }

            if (queues[1].Count > 0)
            {
                winda.dogory = false;
                winda.aktualnePietro = queues[1].Peek();
                return;
            }
            sprawdzczypusto();
            if (queues[2].Count > 0)
            {
                zmienkolejki(0);
                winda.dogory = true;
                winda.aktualnePietro = queues[0].Peek();
                return;
            }
            if (queues[3].Count > 0)
            {
                zmienkolejki(1);
                winda.dogory = false;
                winda.aktualnePietro = queues[1].Peek();
                return;
            }
            if (queues[0].Count == 0 && queues[1].Count == 0) //jesli 5 sekund minie to zjezdza do zera
            {
                counter++;
                if (counter == 5000 / timer1.Interval)
                {
                    queues[1].Enqueue(0, 1);
                    counter = 0;
                }
                return;
            }
            else
            {
                counter = 0; 
            }
        }
        private void ustawianiegraficzne(List<Ludzik> list,Pietro floor,int index)
        {
            int i = 0;
            foreach (Ludzik l in list) 
            {
                l.pb.Location = new Point(i * l.pb.Width+WindaX+3, l.pb.Location.Y); //ustawienie ludzika na odpowiednim pietrze
                i++;
            }
            int j = 0;
            foreach (Ludzik l in floor.ludzie) //ustawienie ludzika na odpowiednim pietrze
            {
                l.pb.Location = new Point(index%2==0?WindaX-(j+1)*l.pb.Width:WindaX+WindaWidth+j*l.pb.Width, l.pb.Location.Y);
                j++;
            }
        }

        private void zmienkolejki(int index)
        {
            while (queues[index + 2].Count > 0)
            {
                int item = queues[index + 2].Dequeue();
                int priority = (index == 1) ? -item : item;
                queues[index].Enqueue(item, priority);
            }
        }

        private void windarusz()
        {
            if (!winda.aktualnePietro.HasValue) 
                return;
            int floor = winda.aktualnePietro.Value;
            int targetY = pietra[floor].pietroY - WindaHeight;
            int delta = 0;

            if (WindaY < targetY)
                delta = Math.Min(step, targetY - WindaY);
            else if (WindaY > targetY)
                delta = -Math.Min(step, WindaY - targetY);

            if (delta != 0)
            {
                WindaY += delta;
                ludzierusz(delta);
                winda.WindaY = WindaY;
            }
            else
            {
                queues[winda.dogory ? 0 : 1].Dequeue();
                ludziewejsc(floor);
                ludziewyjsc(floor);
                ustawianiegraficzne(winda.ludzie, pietra[floor],floor);
                winda.aktualnePietro = null;
            }
        }

        private void ludzierusz(int deltaY)
        {
            foreach (var l in winda.ludzie)
                l.pb.Location = new Point(l.pb.Location.X, l.pb.Location.Y + deltaY);
        }

        private void ludziewejsc(int p)
        {
            var doWejscia = pietra[p].ludzie.ToList();
            doWejscia = pietra[p].ludzie.ToList();

            doWejscia = pietra[p].ludzie.ToList();
            foreach (var l in doWejscia)
            {
                if ((((l.cel > l.actual) && (winda.dogory == true)) || ((l.cel < l.actual) && (winda.dogory != true))) && winda.waga < 530)
                {
                    pietra[p].ludzie.Remove(l);
                    winda.ludzie.Add(l);
                    int slot = winda.ludzie.Count();
                    kolejkacelow.Enqueue(l);
                    winda.waga = winda.ludzie.Count() * 70;
                    int margin = 10;
                    int spacing = l.pb.Width + 5;
                    int x = WindaX + margin + (slot - 1) * spacing;
                    int y = WindaY + WindaHeight - l.pb.Height - margin;
                    l.pb.Location = new Point(x, y);
                    l.pb.BringToFront();
                }
                if (winda.waga >= 530)
                {
                    winda.zalegle.Enqueue(l);
                    pietra[p].ludzie.Remove(l);

                }
            }
        }

        private void ludziewyjsc(int p)
        {
            var toExit = new List<Ludzik>(winda.ludzie);
            foreach (var l in toExit)
            {
                if (l.cel == p)
                {
                    winda.ludzie.Remove(l);
                    winda.waga = winda.ludzie.Count() * 70;
                    Controls.Remove(l.pb);
                    l.pb.Dispose();
                }
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            using var pen = new Pen(Color.Blue, 2);
            e.Graphics.DrawRectangle(pen, WindaX, WindaY, WindaWidth, WindaHeight);
        }
        public PictureBox DodajLudzikaGraficznie(int actual, List<Pietro> pietra)
        {
            PictureBox pb = new PictureBox();
            string imagePath = Path.Combine(Application.StartupPath, "../../../ludzik.jpg");
            pb.Image = Image.FromFile(imagePath);
            pb.SizeMode = PictureBoxSizeMode.Zoom;
            pb.Size = new Size(40, 40);

            // pozycja ludzika na pietrze
            int y = pietra[actual].pietroY - pb.Height;
            int x = (actual % 2 == 0) ? WindaX - pb.Size.Width - ((pietra[actual].ludzie.Count)) * pb.Size.Width : WindaX + WindaWidth + ((pietra[actual].ludzie.Count)) * pb.Size.Width;
            pb.Location = new Point(x, y - pb.Height / 2);

            this.Controls.Add(pb);
            pb.BringToFront();
            return pb;
        }

        public void dodajdokolejki(int a, int c,bool czyzalegle=false)
        {
            if (a < c)
            {

                if (!(pietra[a].pietroY > (WindaY + WindaHeight)) || !winda.dogory||czyzalegle)
                {
                    if (czyzalegle)// ustawienie kierunku windy
                    {
                    winda.dogory = true;
                    }
                    
                    if (!kolejkazawiera(queues[0], a))
                        queues[0].Enqueue(a, a);
                }
                else if (!kolejkazawiera(queues[2], a))
                    queues[2].Enqueue(a, a); // Dodaj do kolejki w gore
            }
            else if (a > c)
            {
                if (!(pietra[a].pietroY < (WindaY + WindaHeight)) || winda.dogory||czyzalegle)
                {
                    if (czyzalegle)// ustawienie kierunku windy
                    {
                        winda.dogory = false;
                    }
                    if (!kolejkazawiera(queues[1], a))
                        queues[1].Enqueue(a, -a);
                }
                else if (!kolejkazawiera(queues[3], a))
                    queues[3].Enqueue(a, -a); // Dodaj do kolejki w dol
            }
        }
    }
}
