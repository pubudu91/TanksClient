using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core
{
    public class NetworkListener
    {
        public delegate void MessageReceivedEventHandler(string message);

        public event MessageReceivedEventHandler MessegeReceived = delegate { };

        public static NetworkListener instance;
        private Thread listenerThread;
        private bool startedListening;

        private NetworkListener()
        {
            startedListening = false;
            listenerThread = null;
        }

        public static NetworkListener GetInstance()
        {
            if (instance == null)
                instance = new NetworkListener();
            return instance;
        }

        public void sendMessage(string msg, string serverip)
        {
            try
            {
                TcpClient client = new TcpClient();
                Console.WriteLine("Connecting to " + serverip);

                client.Connect(serverip, 6000);

                Console.WriteLine("Connected to " + serverip);
                //Console.Write("Enter the string to be transmitted : ");
                Console.WriteLine("Message to be transmitted: " + msg);


                //String str = Console.ReadLine();
                Stream stm = client.GetStream();

                ASCIIEncoding encode = new ASCIIEncoding();
                byte[] ba = encode.GetBytes(msg);

                Console.WriteLine("Transmitting.....");

                stm.Write(ba, 0, ba.Length);

                client.Close();

            }

            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }
            Console.WriteLine("Connection closed\n");
        }

        public void receiveMessage()
        {
            while (true)
            {
                string reply = null;
                try
                {
                    IPAddress ipAd = IPAddress.Parse("127.0.0.1");
                    TcpListener listener = new TcpListener(ipAd, 7000);

                    /* Start Listeneting at the specified port */
                    listener.Start();

                    //Console.WriteLine("The server is running at port 7000...");
                    //Console.WriteLine("The local End point is  :" + listener.LocalEndpoint);
                    //Console.WriteLine("Waiting for a connection.....");

                    Socket socket = listener.AcceptSocket();
                    //Console.WriteLine("Connection accepted from " + socket.RemoteEndPoint);

                    byte[] data = new byte[300];
                    int k = socket.Receive(data);

                    //Console.WriteLine("Recieved...");

                    reply = Encoding.UTF8.GetString(data);
                    //Console.WriteLine(reply);

                    MessegeReceived(reply); // send the message taken from the server, to the parser

                    //ASCIIEncoding asen = new ASCIIEncoding();
                    //s.Send(asen.GetBytes("The string was recieved by the server."));
                    //Console.WriteLine("\nSent Acknowledgement");
                    /* clean up */
                    socket.Close();
                    listener.Stop();
                    Thread.Sleep(10);

                }
                catch (Exception e)
                {
                    Console.WriteLine(reply);
                    Console.WriteLine("Error..... " + e.StackTrace);
                    return;
                }

                Console.WriteLine("Done receiving");
            }
        }
        /*public void connect(string serverip)
        {
            try
            {
                Console.WriteLine(serverip);
                TcpClient client = new TcpClient();
                Console.WriteLine("Connecting.....");

                client.Connect(serverip, 6000);
                // use the ipaddress as in the server program

                Console.WriteLine("Connected");
                Console.Write("Enter the string to be transmitted : ");

                String str = Console.ReadLine();
                Stream stm = client.GetStream();

                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(str);
                Console.WriteLine("Transmitting.....");

                stm.Write(ba, 0, ba.Length);

                client.Close();

            }

            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e);
            }
            Console.WriteLine("Connection closed\n");

            //readData();
        }*/

        public void startListening()
        {
            //if the Network Listener isn't already running, start the listener in a separate thread
            if (!startedListening)
            {
                listenerThread = new Thread(new ThreadStart(this.receiveMessage));
                listenerThread.Start();
                startedListening = true;
            }
        }

        public void stopListening()
        {
            if (startedListening)
            {
                listenerThread.Abort();
                listenerThread.Join();
                startedListening = false;
            }
        }
    }
}
