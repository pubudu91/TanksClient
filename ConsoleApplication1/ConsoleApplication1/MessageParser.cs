using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;
using GameEntity;

namespace Core
{
    
    public class MessageParser
    {
        NetworkListener listener;
        private string serverip;

        public const string JOIN = "JOIN#";
        public const string PLAYERS_FULL = "PLAYERS_FULL#";
        public const string ALREADY_ADDED = "ALREADY_ADDED#";
        public const string GAME_ALREADY_STARTED = "GAME_ALREADY_STARTED#";

        public const string UP = "UP#";
        public const string DOWN = "DOWN#";
        public const string LEFT = "LEFT#";
        public const string RIGHT = "RIGHT#";
        public const string SHOOT = "SHOOT#";

        /* ERROR CODES */
        public const string OBSTACLE = "OBSTACLE#";
        public const string CELL_OCCUPIED = "CELL_OCCUPIED#";
        public const string DEAD = "DEAD#";
        public const string TOO_QUICK = "TOO_QUICK#";
        public const string INVALID_CELL = "INVALID_CELL#";
        public const string GAME_HAS_FINISHED = "GAME_HAS_FINISHED#";
        public const string GAME_NOT_STARTED_YET = "GAME_NOT_STARTED_YET#";
        public const string NOT_A_VALID_CONTESTANT = "NOT_A_VALID_CONTESTANT#";

        private Char[] delim = new Char[] { ',', ';' };

        private Player me;
        private Player[] opponents;
        private ArrayList bricks;
        private ArrayList stone;
        private ArrayList water;
        private ArrayList coinpiles;
        private ArrayList lifepacks;
       

        
        public MessageParser(String serverip)
        {
            this.serverip = serverip;
            listener = NetworkListener.GetInstance();
            //listener.register(this);
            //listener.connect(serverip);
            //Thread t = new Thread(new ThreadStart(this.listenToKeyboard));
            //t.Start();
            listener.MessegeReceived += decode;
        }


        public void sendMessage(string msg)
        {
            listener.sendMessage(msg, serverip);
            Thread.Sleep(1000);
        }

        public void listenToKeyboard()
        {
            while (true)
            {
                ConsoleKeyInfo c = Console.ReadKey();
                //Console.WriteLine(c.Key == ConsoleKey.LeftArrow);

                switch (c.Key)
                {
                    case ConsoleKey.LeftArrow: sendMessage(LEFT); break;
                    case ConsoleKey.RightArrow: sendMessage(RIGHT); break;
                    case ConsoleKey.UpArrow: sendMessage(UP); break;
                    case ConsoleKey.DownArrow: sendMessage(DOWN); break;
                    case ConsoleKey.Spacebar: sendMessage(SHOOT); break;
                }
            }
        }

        public string[] getMessage()
        {
            //Thread t = new Thread(new ThreadStart(listener.receiveMessage));
            //t.Start();
            listener.startListening();
            return null;
        }

        public void decode(string msg)
        {
            int len = msg.LastIndexOf('#');
            msg = msg.Trim().Substring(0, len);
            string []s = msg.Split(new Char[]{':'});

            switch (s[0])
            {
                case "I": decodeGameInitiation(s); break;
                case "S": decodeAcceptMessage(s); break;
                case "G": decodeGlobalBroadcast(s); break;
                case "C": decodeCoinPile(s); break;
                case "L": decodeLifePack(s); break;
                default: Console.WriteLine("ERROR"); break;
            }
        }

        private void decodeAcceptMessage(string[] str)
        {
            if(opponents == null)
                opponents = new Player[4];

            for (int i = 1, k=0; i < str.Length; i++)
            {
                string[] temp = str[i].Split(delim);
                opponents[k] = new Player();

                if (temp[0].Equals(me.id))
                {
                    //Console.WriteLine("OWN PLAYER: " + me.id);
                    continue;
                }

                //Console.WriteLine("DAM: " + i);
                opponents[k].id = temp[0];
                opponents[k].x = Int32.Parse(temp[1]);
                opponents[k].y = Int32.Parse(temp[2]);

                switch (temp[3])
                {
                    case "0": opponents[k].direction = Direction.NORTH; break;
                    case "1": opponents[k].direction = Direction.EAST; break;
                    case "2": opponents[k].direction = Direction.SOUTH; break;
                    case "3": opponents[k].direction = Direction.WEST; break;
                    default: Console.WriteLine("Invalid direction: -{0}-", temp[3]); break;
                }
                
                k++;
                //Console.WriteLine("PLAYER: "+str[i]);
            }
            Console.WriteLine("ACCEPTANCE MSG DECODED " + opponents[0].direction);
        }

        private void decodeGameInitiation(string []str)
        {
            bricks = new ArrayList();
            stone = new ArrayList();
            water = new ArrayList();
            me = new Player();

            me.id = str[1];

            string[] temp = str[2].Split(delim);

            for (int i = 0; i < temp.Length; i+=2)
            {
                int[] xy = new int[] { Int32.Parse(temp[i]), Int32.Parse(temp[i + 1]) };
                bricks.Add(xy);
            }

            temp = str[3].Split(delim);

            for (int i = 0; i < temp.Length; i += 2)
            {
                int[] xy = new int[] { Int32.Parse(temp[i]), Int32.Parse(temp[i + 1]) };
                stone.Add(xy);
            }

            temp = str[4].Split(delim);

            for (int i = 0; i < temp.Length; i += 2)
            {
                int[] xy = new int[] { Int32.Parse(temp[i]), Int32.Parse(temp[i + 1]) };
                water.Add(xy);
            }
            Console.WriteLine("INITIATION MSG DECODED");
        }

        private void decodeGlobalBroadcast(string[] str)
        {
            for (int i = 1; i < str.Length-1; i++)
            {
                string[] temp = str[i].Split(delim);

                if (me.id.Equals(temp[0]))
                {
                    me.x = Int32.Parse(temp[1]);
                    me.y = Int32.Parse(temp[2]);
                    me.direction = getDirection(temp[3]);
                    me.wasShot = Int32.Parse(temp[4]) == 0 ? false : true;
                    me.health = Int32.Parse(temp[5]);
                    me.coins = Int32.Parse(temp[6]); // Coins total or coins gathered within that second?
                    me.points = Int32.Parse(temp[7]);
                }
                else
                {
                    Console.WriteLine("-" + temp[1] + "-");
                    opponents[i].x = Int32.Parse(temp[1]);
                    opponents[i].y = Int32.Parse(temp[2]);
                    opponents[i].direction = getDirection(temp[3]);
                    opponents[i].wasShot = Int32.Parse(temp[4]) == 0 ? false : true;
                    opponents[i].health = Int32.Parse(temp[5]);
                    opponents[i].coins = Int32.Parse(temp[6]); // Coins total or coins gathered within that second?
                    opponents[i].points = Int32.Parse(temp[7]);
                }
            }
        }

        private void decodeCoinPile(string[] str)
        {
            if(coinpiles == null)
                coinpiles = new ArrayList();

            //long timestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long timestamp = (long)(DateTime.UtcNow - epoch).TotalMilliseconds;

            CoinPile coins = new CoinPile();

            string[] temp = str[1].Split(delim);

            coins.x = Int32.Parse(temp[0]);
            coins.y = Int32.Parse(temp[1]);
            coins.lifetime = Int32.Parse(str[2]);
            coins.value = Int32.Parse(str[3]);
            coins.timestamp = timestamp;
            // consider the possibility of using a stopwatch here

            coinpiles.Add(coins);
        }

        private void decodeLifePack(string[] str)
        {
            if (lifepacks == null)
                lifepacks = new ArrayList();

            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long timestamp = (long)(DateTime.UtcNow - epoch).TotalMilliseconds;

            LifePack lifepack = new LifePack();

            string[] temp = str[1].Split(delim);

            lifepack.x = Int32.Parse(temp[0]);
            lifepack.y = Int32.Parse(temp[1]);
            lifepack.lifetime = Int32.Parse(str[2]);
            lifepack.timestamp = timestamp;

            lifepacks.Add(lifepack);
        }

        private Direction getDirection(string str)
        {
            switch (str)
            {
                case "0": return Direction.NORTH;
                case "1": return Direction.EAST;
                case "2": return Direction.SOUTH;
                case "3": return Direction.WEST;
                default: Console.WriteLine("Invalid direction: -{0}-", str); return Direction.INVALID;
            }
        }

        public Player getMyself()
        {
            return me;
        }

        public Player[] getOpponents()
        {
            return opponents;
        }

        public ArrayList getCoinPiles()
        {
            return coinpiles;
        }

        public ArrayList getLifePacks()
        {
            return lifepacks;
        }

        public ArrayList getBricks()
        {
            return bricks;
        }

        public ArrayList getStones()
        {
            return stone;
        }

        public ArrayList getWater()
        {
            return water;
        }
    }
}
