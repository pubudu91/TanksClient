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

        public void sendMessage(string msg)
        {
            try
            {
                TcpClient client = new TcpClient();
                Console.WriteLine("Connecting to " + Util.SERVERIP);

                client.Connect(Util.SERVERIP, 6000);

                Console.WriteLine("Connected to " + Util.SERVERIP);
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
            NetworkStream stream = null;
            IPAddress ipAd = IPAddress.Parse(Util.MYIP);
            TcpListener listener = new TcpListener(ipAd, 7000);
            Socket socket = null;

            /* Start Listeneting at the specified port */
            listener.Start();

            string reply = "";

            while (true)
            {
                try
                {
                    socket = listener.AcceptSocket();

                    if (socket.Connected)
                    {
                        stream = new NetworkStream(socket);
                        List<byte> data = new List<byte>();

                        do
                        {
                            data.Add((byte)stream.ReadByte());
                        }while (stream.DataAvailable);

                        reply = Encoding.UTF8.GetString(data.ToArray());
                        //Console.WriteLine(reply);

                        stream.Close();
                        socket.Close();

                        if(reply.Length > 2)
                            MessegeReceived(reply); // send the message taken from the server, to the parser
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(reply);
                    Console.WriteLine("Error..... " + e.StackTrace);
                    Console.WriteLine("END OF ERROR");
                }
            }
        }

        public void startListening()
        {
            //if the Network Listener isn't already running, start the listener in a separate thread
            if (!startedListening)
            {
                Console.WriteLine("Starting to Listen");
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
