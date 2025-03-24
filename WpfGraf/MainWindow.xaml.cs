using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace WpfGraf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {        
        Person jeg = new Person() { Id = 111, Name = "Jakup", X = 20, Y = 20, Width = 100, Height = 100 };

        public MainWindow()
        {
            InitializeComponent();
            clearCanvas();
            tegnPerson();
  
        }
        private void clearCanvas()
        {
            mitCanvas.Children.Clear();
        }
        private void tegnPerson()
        {            
            tegnFikant(jeg.X, jeg.Y, jeg.Width, jeg.Height);
            tegnText(jeg.Name, jeg.X + 5, jeg.Y + 5);
        }
        private void tegnFikant(double xStart, double yStart, double width, double height)
        {
            Rectangle rect1 = new Rectangle();
            rect1.Stroke = new SolidColorBrush(Colors.Black);

            rect1.StrokeThickness = 2;
            rect1.Width = width;
            rect1.Height = height;
            Canvas.SetLeft(rect1, xStart);
            Canvas.SetTop(rect1, yStart);
            mitCanvas.Children.Add(rect1);
        }
 
        private void tegnText(String txt, double xStart, double yStart)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = txt;
            textBlock.FontSize = 24;
            textBlock.Foreground = Brushes.Black;
            Canvas.SetLeft(textBlock, xStart);
            Canvas.SetTop(textBlock, yStart);
            mitCanvas.Children.Add(textBlock);
        }

        private void mitCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            clearCanvas();
            Point p = e.GetPosition((Canvas)sender);
            Console.WriteLine(p.X +" "+ p.Y);
            jeg.X = p.X;
            jeg.Y = p.Y;
            tegnPerson();
        }

        private void buttonServer_Click(object sender, RoutedEventArgs e)
        {
            InitializeComponent();

            var ipAddresses = NetworkInterface.GetAllNetworkInterfaces()
                .Where(ni => ni.OperationalStatus == OperationalStatus.Up)
                .SelectMany(ni => ni.GetIPProperties().UnicastAddresses)
                .Where(unicast => unicast.Address.AddressFamily == AddressFamily.InterNetwork)
                .Select(unicast => unicast.Address.ToString())
                .ToList();

            // Eksempel: vis IP-adresserne i en MessageBox
            if (ipAddresses.Any())
            {
                MessageBox.Show("Lokal IP-adresse(r):\n" + string.Join("\n", ipAddresses));
            }
            else
            {
                MessageBox.Show("Ingen aktive IPv4-adresser fundet.");
            }
            Thread serverThread = new Thread(ServerVenterPaaKlient);
            serverThread.Start();
           
        }

        private void ServerVenterPaaKlient()
        {
            // Opret en TcpListener, der lytter på alle netværksinterfaces (IPAddress.Any) på port 12345
            TcpListener listener = new TcpListener(IPAddress.Any, 12345);
            listener.Start();
           
            this.Dispatcher.Invoke(() =>
            {
                listBoxBesked.Items.Add("Server started. Waiting for a connection...");
            });
            // Accepter første klient, der forbinder
            TcpClient client = listener.AcceptTcpClient();
            
            this.Dispatcher.Invoke(() =>
            {
                listBoxBesked.Items.Add("Client connected.");
            });
            // Send "Hello" til klienten
            NetworkStream stream = client.GetStream();
            byte[] message = Encoding.ASCII.GetBytes("Hello");
            stream.Write(message, 0, message.Length);

            
            this.Dispatcher.Invoke(() =>
            {
                listBoxBesked.Items.Add("Server: Har sendt Hello til Client...");
            });
            // Luk klient og server
            client.Close();
            listener.Stop();
        }

        private void ClientSenderTilServer()
        {
            string ip = "";
            // Opret forbindelse til serverens IP og port (her 127.0.0.1:12345)
            this.Dispatcher.Invoke(() =>
            {
                ip = textBoxIp.Text;
            });
            using TcpClient client = new TcpClient(ip, 12345);
           
            this.Dispatcher.Invoke(() =>
            {
                listBoxBesked.Items.Add("Connected to server.");
            });
            // Læs besked fra serveren
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[256];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);

            // Konverter byte-array til streng og skriv den ud
            string receivedMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);

            this.Dispatcher.Invoke(() =>
            {
                listBoxBesked.Items.Add("Client: Modtager fra server - " + receivedMessage);
            });
        }

        private void buttonClient_Click(object sender, RoutedEventArgs e)
        {
            Thread clientThread = new Thread(ClientSenderTilServer);
            clientThread.Start();
        }
    }
    class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
    }
}
