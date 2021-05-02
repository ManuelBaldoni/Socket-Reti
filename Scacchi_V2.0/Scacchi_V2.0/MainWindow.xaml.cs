using Scacchi_V2._0.Classi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Scacchi_V2._0
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public static List<Pezzo> pezzi;
        public static List<Pezzo> pezziMangiati;
        public static List<string> mosse;
        public static Grid scacchiera;
        public static Grid sMosse;
        public static Pezzo pezzoSel;

        public static DispatcherTimer timerNero;
        public static DispatcherTimer timerBianco;

        public const string sourceIP = "192.168.1.4";
        public const string destinationIP = "192.168.1.8";
        public const int porta = 56000;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            pezzi = new List<Pezzo>();
            pezziMangiati = new List<Pezzo>();
            scacchiera = GScacchiera;
            sMosse = GMosse;
            creaPezzi();
            bottoniOnOff(false, "n");

            timerNero = new DispatcherTimer();
            timerNero.Tick += TimerNero_Tick;
            timerNero.Interval = new TimeSpan(0, 0, 1);

            timerBianco = new DispatcherTimer();
            timerBianco.Tick += TimerBianco_Tick;
            timerBianco.Interval = new TimeSpan(0, 0, 1);

            IPEndPoint sourceSocket = new IPEndPoint(IPAddress.Parse(sourceIP), porta);

            Thread ricezione = new Thread(new ParameterizedThreadStart(SocketReceive));
            ricezione.Start(sourceSocket);
        }

        public async void SocketReceive(object socketSource)
        {
            IPEndPoint ipEndP = (IPEndPoint)socketSource;
            Socket t = new Socket(ipEndP.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
            t.Bind(ipEndP);

            Byte[] byteRicevuti = new Byte[256];
            string message;
            int contaCaratteri = 0;

            await Task.Run(() =>
            {
                while (true)
                {
                    if (t.Available > 0)
                    {
                        message = "";
                        contaCaratteri = t.Receive(byteRicevuti, byteRicevuti.Length, 0);
                        message += Encoding.ASCII.GetString(byteRicevuti, 0, contaCaratteri);

                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {

                            int r = int.Parse(message[0].ToString()), c = int.Parse(message[1].ToString());
                            Pezzo p = Pezzo.GetChildren(r, c);

                            //Suono del pezzo
                            SoundPlayer sp = new SoundPlayer("mossaPezzo.wav");
                            sp.Play();

                            //Pezzo mangia l'altro
                            if (message[4].ToString() == "x")
                            {
                                Pezzo.GetChildren(int.Parse(message[2].ToString()), int.Parse(message[3].ToString())).mangiato();
                            }
                            p.Mosso = true;
                            p.R = int.Parse(message[2].ToString());
                            p.C = int.Parse(message[3].ToString());

                            //Arrocco
                            if (message[4].ToString() == "o")
                            {
                                if (p.C > 4)
                                {
                                    Pezzo.GetChildren(p.R, p.C + 1).Mosso = true;
                                    Pezzo.GetChildren(p.R, p.C + 1).C = p.C - 1;
                                }
                                else
                                {
                                    Pezzo.GetChildren(p.R, p.C - 2).Mosso = true;
                                    Pezzo.GetChildren(p.R, p.C - 2).C = p.C + 1;
                                }
                            }

                            Pezzo.pulisci();
                            foreach (Pezzo P in MainWindow.pezzi)
                            {
                                P.aggiornaMossePossibili();
                            }

                            MainWindow.bottoniOnOff(false, p.Colore);
                            if (p.Colore == "b")
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
                        }));
                    }
                }
            });
        }

        int timerBM = 5, timerNM = 5, timerBS = 0, timerNS = 0;

        private void TimerBianco_Tick(object sender, EventArgs e)
        {
            if (timerBS == 0)
            {
                timerBS = 60;
                timerBM--;
            }
            timerBS--;
            TXTTimerBianco.Text = string.Format("{0}:{1:00}", timerBM, timerBS);
            if (timerBM == 0 && timerBS == 0)
            {
                //Quando Nero vince
            }
        }

        private void TimerNero_Tick(object sender, EventArgs e)
        {
            if (timerNS == 0)
            {
                timerNS = 60;
                timerNM--;
            }
            timerNS--;
            TXTTimerNero.Text = string.Format("{0}:{1:00}", timerNM, timerNS);
            if (timerNM == 0 && timerNS == 0)
            {
                //Quando Bianco vince
            }
        }

        private void CBXWB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Scacchiere e Timers
            RotateTransform rt = new RotateTransform(0);
            if (CBXWB.SelectedIndex == 0)
            {
                //Timer Bianco
                TXTTimerBianco.Margin = new Thickness(0, 0, 50, 100);
                TXTTimerBianco.VerticalAlignment = VerticalAlignment.Bottom;

                //Timer Nero
                TXTTimerNero.Margin = new Thickness(0, 100, 50, 0);
                TXTTimerNero.VerticalAlignment = VerticalAlignment.Top;

                //Scacchiere
                rt = new RotateTransform(0);
                GScacchiera.LayoutTransform = rt;
                GMosse.LayoutTransform = rt;

                //Numeri e Lettere
                TXTNumeri.Text = "8       7       6       5       4       3       2      1";
                TXTLettere.Text = "a                b                c                d                  e                f                g                h";
            }
            else if(CBXWB.SelectedIndex == 1)
            {
                //Timer Bianco
                TXTTimerBianco.Margin = new Thickness(0, 100, 50, 0);
                TXTTimerBianco.VerticalAlignment = VerticalAlignment.Top;

                //Timer Nero
                TXTTimerNero.Margin = new Thickness(0, 0, 50, 100);
                TXTTimerNero.VerticalAlignment = VerticalAlignment.Bottom;

                //Scacchiere
                rt = new RotateTransform(180);
                GScacchiera.LayoutTransform = rt;
                GMosse.LayoutTransform = rt;

                //Numeri e Lettere
                TXTNumeri.Text = "1       2       3       4       5       6       7      8";
                TXTLettere.Text = "h                g                f                e                  d                c                b                a";
            }

            //Pezzi
            foreach (Pezzo p in pezzi)
            {
                ((Image)p.B.Content).LayoutTransform = rt;
            }

        }

        void creaPezzi()
        {
            //Pedoni bianchi
            for (int i = 0; i < 8; i++)
            {
                Image img = new Image();
                img.Source = new BitmapImage(new Uri("/Immagini/pedoneBianco.png", UriKind.Relative));
                Pezzo pedone = new Pedone(6, i, "p", "b", img, GScacchiera);
                pezzi.Add(pedone);
            }

            //Pedoni neri
            for (int i = 0; i < 8; i++)
            {
                Image img = new Image();
                img.Source = new BitmapImage(new Uri("/Immagini/pedoneNero.png", UriKind.Relative));
                Pezzo pedone = new Pedone(1, i, "p", "n", img, GScacchiera);
                pezzi.Add(pedone);
            }

            //Pezzi bianchi
            Image immagine = new Image();
            immagine.Source = new BitmapImage(new Uri("/Immagini/torreBianca.png", UriKind.Relative));
            pezzi.Add(new Torre(7, 0, "t", "b", immagine, GScacchiera));
            immagine = new Image();
            immagine.Source = new BitmapImage(new Uri("/Immagini/torreBianca.png", UriKind.Relative));
            pezzi.Add(new Torre(7, 7, "t", "b", immagine, GScacchiera));

            immagine = new Image();
            immagine.Source = new BitmapImage(new Uri("/Immagini/cavalloBianco.png", UriKind.Relative));
            pezzi.Add(new Cavallo(7, 1, "c", "b", immagine, GScacchiera));
            immagine = new Image();
            immagine.Source = new BitmapImage(new Uri("/Immagini/cavalloBianco.png", UriKind.Relative));
            pezzi.Add(new Cavallo(7, 6, "c", "b", immagine, GScacchiera));

            immagine = new Image();
            immagine.Source = new BitmapImage(new Uri("/Immagini/alfiereBianco.png", UriKind.Relative));
            pezzi.Add(new Alfiere(7, 2, "a", "b", immagine, GScacchiera));
            immagine = new Image();
            immagine.Source = new BitmapImage(new Uri("/Immagini/alfiereBianco.png", UriKind.Relative));
            pezzi.Add(new Alfiere(7, 5, "a", "b", immagine, GScacchiera));

            immagine = new Image();
            immagine.Source = new BitmapImage(new Uri("/Immagini/reBianco.png", UriKind.Relative));
            pezzi.Add(new Re(7, 4, "R", "b", immagine, GScacchiera));

            immagine = new Image();
            immagine.Source = new BitmapImage(new Uri("/Immagini/reginaBianca.png", UriKind.Relative));
            pezzi.Add(new Regina(7, 3, "r", "b", immagine, GScacchiera));

            //Pezzi neri
            immagine = new Image();
            immagine.Source = new BitmapImage(new Uri("/Immagini/torreNera.png", UriKind.Relative));
            pezzi.Add(new Torre(0, 0, "t", "n", immagine, GScacchiera));
            immagine = new Image();
            immagine.Source = new BitmapImage(new Uri("/Immagini/torreNera.png", UriKind.Relative));
            pezzi.Add(new Torre(0, 7, "t", "n", immagine, GScacchiera));

            immagine = new Image();
            immagine.Source = new BitmapImage(new Uri("/Immagini/cavalloNero.png", UriKind.Relative));
            pezzi.Add(new Cavallo(0, 1, "c", "n", immagine, GScacchiera));
            immagine = new Image();
            immagine.Source = new BitmapImage(new Uri("/Immagini/cavalloNero.png", UriKind.Relative));
            pezzi.Add(new Cavallo(0, 6, "c", "n", immagine, GScacchiera));

            immagine = new Image();
            immagine.Source = new BitmapImage(new Uri("/Immagini/alfiereNero.png", UriKind.Relative));
            pezzi.Add(new Alfiere(0, 2, "a", "n", immagine, GScacchiera));
            immagine = new Image();
            immagine.Source = new BitmapImage(new Uri("/Immagini/alfiereNero.png", UriKind.Relative));
            pezzi.Add(new Alfiere(0, 5, "a", "n", immagine, GScacchiera));

            immagine = new Image();
            immagine.Source = new BitmapImage(new Uri("/Immagini/reNero.png", UriKind.Relative));
            pezzi.Add(new Re(0, 4, "R", "n", immagine, GScacchiera));

            immagine = new Image();
            immagine.Source = new BitmapImage(new Uri("/Immagini/reginaNera.png", UriKind.Relative));
            pezzi.Add(new Regina(0, 3, "r", "n", immagine, GScacchiera));

            foreach (Pezzo p in pezzi)
            {
                p.aggiornaMossePossibili();
            }
        }
        public static void bottoniOnOff(bool ok, string colore)
        {
            foreach (Pezzo p in pezzi)
            {
                if(p.Colore == colore)
                {
                    p.B.IsHitTestVisible = ok;
                }
            }
        }
    }
}