using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

//Aggiunta delle seguenti librerie
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace Progetto_Socket
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

        private void BTNSocket_Click(object sender, RoutedEventArgs e)
        {
            IPEndPoint sourceSocket = new IPEndPoint(IPAddress.Parse("192.168.1.4"), 56000);

            BTNInvia.IsEnabled = true;

            Thread ricezione = new Thread(new ParameterizedThreadStart(SocketReceive));
            ricezione.Start(sourceSocket);

            TXTInfo.Text = "Socket creato correttamente";
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
                            LBLRicevi.Content += message + "\n";
                        }));
                    }
                }
            });

        }

        private void TXTIp_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TXTPorta_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void BTNInvia_Click(object sender, RoutedEventArgs e)
        {
            //ToDo controllo ip e porta
            string ipAdress = TXTIp.Text;
            int port = int.Parse(TXTPorta.Text);

            SocketSend(IPAddress.Parse(ipAdress), port, TXTMessage.Text);
        }
        public void SocketSend(IPAddress dest, int destPort, string message)
        {
            Byte[] byteInviati = Encoding.ASCII.GetBytes(message);

            Socket s = new Socket(dest.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

            IPEndPoint remoteEndPoint = new IPEndPoint(dest, destPort);

            s.SendTo(byteInviati, remoteEndPoint);

            TXTInfo.Text = "Messaggio inviato";
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter && BTNInvia.IsEnabled)
            {
                //ToDo controllo ip e porta
                string ipAdress = TXTIp.Text;
                int port = int.Parse(TXTPorta.Text);

                SocketSend(IPAddress.Parse(ipAdress), port, TXTMessage.Text);
            }
        }
    }
}
