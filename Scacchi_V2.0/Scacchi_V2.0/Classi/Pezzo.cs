using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Scacchi_V2._0.Classi
{
    public abstract class Pezzo : IEnumerable<Pezzo>
    {
        private int r;
        public int R
        {
            get
            {
                return r;
            }
            set
            {
                Grid.SetRow(this.B, value);
                r = value;
            }
        }
        private int c;
        public int C
        {
            get
            {
                return c;
            }
            set
            {
                Grid.SetColumn(this.B, value);
                c = value;
            }
        }
        public string Tipo { get; set; }
        public string Colore { get; set; }
        public Button B { get; set; }
        public bool Mangiato { get; set; }
        public bool Mosso { get; set; }

        public List<string> mossePossibili;

        public Pezzo(int r, int c, string tipo, string colore, Image I, Grid g)
        {
            this.B = new Button();
            B.Style = App.Current.FindResource("PezziBottoni") as Style;
            this.B.Content = I;
            B.Click += B_Click;
            g.Children.Add(this.B);

            this.R = r;
            this.C = c;
            this.Tipo = tipo;
            this.Colore = colore;
            this.Mangiato = false;
            this.Mosso = false;

            this.mossePossibili = new List<string>();
        }

        public abstract void aggiornaMossePossibili();
        public void torreMove()
        {
            int i = this.C - 1;
            while (i >= 0)
            {
                if (GetChildren(this.R, i) == null)
                {
                    mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R}{i},c");
                }
                else
                {
                    if (GetChildren(this.R, i).Colore != this.Colore)
                    {
                        mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R}{i},x");
                    }
                    break;
                }
                i--;
            }
            i = this.C + 1;
            while (i <= 7)
            {
                if (GetChildren(this.R, i) == null)
                {
                    mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R}{i},c");
                }
                else
                {
                    if (GetChildren(this.R, i).Colore != this.Colore)
                    {
                        mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{this.R}{i},x");
                    }
                    break;
                }
                i++;
            }
            i = this.R - 1;
            while (i >= 0)
            {
                if (GetChildren(i, this.C) == null)
                {
                    mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{i}{this.C},c");
                }
                else
                {
                    if (GetChildren(i, this.C).Colore != this.Colore)
                    {
                        mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{i}{this.C},x");
                    }
                    break;
                }
                i--;
            }
            i = this.R + 1;
            while (i <= 7)
            {
                if (GetChildren(i, this.C) == null)
                {
                    mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{i}{this.C},c");
                }
                else
                {
                    if (GetChildren(i, this.C).Colore != this.Colore)
                    {
                        mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{i}{this.C},x");
                    }
                    break;
                }
                i++;
            }
        }
        public void alfiereMove()
        {
            //Alto a sinistra
            int i = this.C - 1;
            int j = this.R - 1;
            while (i >= 0 && j >= 0)
            {
                if (GetChildren(j, i) == null)
                {
                    mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{j}{i},c");
                }
                else
                {
                    if (GetChildren(j, i).Colore != this.Colore)
                    {
                        mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{j}{i},x");
                    }
                    break;
                }
                i--;
                j--;
            }
            i = this.C + 1;
            j = this.R - 1;
            while (i <= 7 && j >= 0)
            {
                if (GetChildren(j, i) == null)
                {
                    mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{j}{i},c");
                }
                else
                {
                    if (GetChildren(j, i).Colore != this.Colore)
                    {
                        mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{j}{i},x");
                    }
                    break;
                }
                i++;
                j--;
            }
            i = this.C + 1;
            j = this.R + 1;
            while (i <= 7 && j <= 7)
            {
                if (GetChildren(j, i) == null)
                {
                    mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{j}{i},c");
                }
                else
                {
                    if (GetChildren(j, i).Colore != this.Colore)
                    {
                        mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{j}{i},x");
                    }
                    break;
                }
                i++;
                j++;
            }
            i = this.C - 1;
            j = this.R + 1;
            while (i >= 0 && j <= 7)
            {
                if (GetChildren(j, i) == null)
                {
                    mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{j}{i},c");
                }
                else
                {
                    if (GetChildren(j, i).Colore != this.Colore)
                    {
                        mossePossibili.Add($"{this.Tipo},{this.R}{this.C},{j}{i},x");
                    }
                    break;
                }
                i--;
                j++;
            }
        }

        private void B_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            pulisci();
            if(MainWindow.pezzoSel != GetChildren(Grid.GetRow((Button)sender), Grid.GetColumn((Button)sender)))
            {
                MainWindow.pezzoSel = GetChildren(Grid.GetRow((Button)sender), Grid.GetColumn((Button)sender));
                foreach (string s in mossePossibili)
                {
                    string[] tmp = s.Split(',');
                    Button mossa = new Button();
                    mossa.Name = tmp[3];
                    if (tmp[3] == "x")
                    {
                        mossa.BorderThickness = new Thickness(4);
                        mossa.BorderBrush = Brushes.Red;
                    }
                    else
                    {
                        mossa.Style = App.Current.FindResource("PezziTmp") as Style;
                        Ellipse cerchio = new Ellipse();
                        cerchio.Height = 20;
                        cerchio.Width = 20;
                        cerchio.Fill = Brushes.LightSeaGreen;
                        mossa.Content = cerchio;
                    }
                    mossa.Click += Mossa_Click;
                    MainWindow.sMosse.Children.Add(mossa);
                    Grid.SetRow(mossa, int.Parse(tmp[2][0].ToString()));
                    Grid.SetColumn(mossa, int.Parse(tmp[2][1].ToString()));
                }
            }
            else
            {
                MainWindow.pezzoSel = null;
            }
        }

        private void Mossa_Click(object sender, RoutedEventArgs e)
        {
            string ipAdress = "192.168.1.4";
            int port = 56000;

            SocketSend(IPAddress.Parse(ipAdress), port, $"{this.R}{this.C}{Grid.GetRow((Button)sender)}{Grid.GetColumn((Button)sender)}{((Button)sender).Name}");

            //Suono del pezzo
            SoundPlayer sp = new SoundPlayer("mossaPezzo.wav");
            sp.Play();

            //Pezzo mangia l'altro
            if(((Button)sender).Name == "x")
            {
                GetChildren(Grid.GetRow((Button)sender), Grid.GetColumn((Button)sender)).mangiato();
            }
            this.Mosso = true;
            this.R = Grid.GetRow((Button)sender);
            this.C = Grid.GetColumn((Button)sender);

            //Arrocco
            if(((Button)sender).Name == "o")
            {
                if(this.C > 4)
                {
                    GetChildren(this.R, this.C + 1).Mosso = true;
                    GetChildren(this.R, this.C + 1).C = this.C - 1;
                }
                else
                {
                    GetChildren(this.R, this.C - 2).Mosso = true;
                    GetChildren(this.R, this.C - 2).C = this.C + 1;
                }
            }

            pulisci();
            foreach (Pezzo p in MainWindow.pezzi)
            {   
                p.aggiornaMossePossibili();
            }

            MainWindow.bottoniOnOff(false, this.Colore);
            if(this.Colore == "b")
            {
                MainWindow.bottoniOnOff(true, "n");
                MainWindow.timerBianco.Stop();
                MainWindow.timerNero.Start();
            }
            else
            {
                MainWindow.bottoniOnOff(true, "b");
                MainWindow.timerBianco.Start();
                MainWindow.timerNero.Stop();
            }
        }

        public static void pulisci()
        {
            MainWindow.sMosse.Children.Clear();
        }
        public void mangiato()
        {
            MainWindow.pezzi.Remove(this);
            MainWindow.pezziMangiati.Add(this);
            MainWindow.scacchiera.Children.Remove(this.B);
            this.Mangiato = true;
        }
        public static Pezzo GetChildren(int row, int column)
        {
            foreach (Pezzo p in MainWindow.pezzi)
            {   
                if(p.R == row && p.C == column)
                {
                    if (!p.Mangiato)
                    {
                        return p;
                    }
                }
            }
            return null;
        }

        public void SocketSend(IPAddress dest, int destPort, string message)
        {
            Byte[] byteInviati = Encoding.ASCII.GetBytes(message);

            Socket s = new Socket(dest.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

            IPEndPoint remoteEndPoint = new IPEndPoint(dest, destPort);

            s.SendTo(byteInviati, remoteEndPoint);
        }

        public IEnumerator<Pezzo> GetEnumerator()
        {
            for (int i = 0; i < MainWindow.pezzi.Count; i++)
            {
                yield return MainWindow.pezzi[i];
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public static bool operator ==(Pezzo p1, Pezzo p2)
        {
            if(p1 is null && p2 is null)
            {
                return true;
            }
            if(p1 is null || p2 is null)
            {
                return false;
            }
            return p1.R == p2.R && p1.C == p2.C;
        }
        public static bool operator !=(Pezzo p1, Pezzo p2)
        {
            return !(p1 == p2);
        }

    }
}
