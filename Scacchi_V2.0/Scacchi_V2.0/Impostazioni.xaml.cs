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
using System.Windows.Shapes;

namespace Scacchi_V2._0
{
    /// <summary>
    /// Logica di interazione per Impostazioni.xaml
    /// </summary>
    public partial class Impostazioni : Window
    {
        public Impostazioni()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            App.windowImp.Clear();
        }

        private void BTNOk_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.porta = int.Parse(TXTSourcePort.Text);
            MainWindow.sourceIP = TXTSourceIP.Text;
            MainWindow.destinationIP = TXTDestinationIP.Text;

            //Per comunicazione
            IPEndPoint sourceSocket = new IPEndPoint(IPAddress.Parse(MainWindow.sourceIP), MainWindow.porta);

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
    }
}
