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
    public delegate void AcceptMessageHandler(List<string[]> players);
    public delegate void GameInitiationHandler(List<Position> bricks, List<Position> stone, List<Position> water, Player me);
    public delegate void GlobalBroadcastHandler(List<string[]> players, int[,] brickdamage);
    
    public class MessageParser
    {
        public event GameInitiationHandler GameInitiation = delegate { };
        public event AcceptMessageHandler AcceptMessage = delegate { };
        public event GlobalBroadcastHandler GlobalBroadcast = delegate { };

        private NetworkListener listener;

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

        //private Player me;
        //private Player[] opponents;
        private List<string[]> players;
        private List<Position> bricks;
        private List<Position> stone;
        private List<Position> water;
        private List<Position> coinpiles;
        private List<Position> lifepacks;
        private int noOfPlayers;

        private static MessageParser parser;
        
        private MessageParser()
        {
            //this.serverip = serverip;
            listener = NetworkListener.GetInstance();
            listener.MessegeReceived += decode;
        }

        public static MessageParser getInstance()
        {
            if (parser == null)
                parser = new MessageParser();

            return parser;
        }


        public void sendMessage(string msg)
        {
            listener.sendMessage(msg);
            //Thread.Sleep(1000);
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

        public void getMessage()
        {
            listener.startListening();
        }

        public void decode(string msg)
        {
            int len = msg.Length - 1; //msg.LastIndexOf('#');
            msg = msg.Trim().Substring(0, len);
            //Console.WriteLine(msg);
            string []s = msg.Split(new Char[]{':'});

            switch (s[0])
            {
                case "I": decodeGameInitiation(s); break;
                case "S": decodeAcceptMessage(s); break;
                case "G": decodeGlobalBroadcast(s); break;
                //case "C": decodeCoinPile(s); break;
                //case "L": decodeLifePack(s); break;
                //default: Console.WriteLine("ERROR: "+msg); break;
            }
        }

        private void decodeAcceptMessage(string[] str)
        {
            if (players == null)
                players = new List<string[]>();

            //noOfPlayers = str.Length - 1;

            for (int i = 1; i < str.Length; i++)
            {
                string[] temp = str[i].Split(delim);
                players.Add(temp);
            }

            noOfPlayers = players.Count;

            Console.WriteLine("ACCEPTANCE MSG DECODED " + players.Count);
            AcceptMessage(players);
        }

        private void decodeGameInitiation(string []str)
        {
            bricks = new List<Position>();
            stone = new List<Position>();
            water = new List<Position>();
            //players = new List<Player>();
            Player me = new Player();

            me.id = str[1];
            me.isOpponent = false;
            //players.Add(me);

            string[] temp = str[2].Split(delim);

            for (int i = 0; i < temp.Length; i+=2)
            {
                int[] xy = new int[] { Int32.Parse(temp[i]), Int32.Parse(temp[i + 1]) };
                bricks.Add(new Position(xy[0], xy[1]));
            }

            temp = str[3].Split(delim);

            for (int i = 0; i < temp.Length; i += 2)
            {
                int[] xy = new int[] { Int32.Parse(temp[i]), Int32.Parse(temp[i + 1]) };
                stone.Add(new Position(xy[0], xy[1]));
            }

            temp = str[4].Split(delim);

            for (int i = 0; i < temp.Length; i += 2)
            {
                int[] xy = new int[] { Int32.Parse(temp[i]), Int32.Parse(temp[i + 1]) };
                water.Add(new Position(xy[0], xy[1]));
            }
            Console.WriteLine("INITIATION MSG DECODED");
            GameInitiation(bricks, stone, water, me);
        }

        private void decodeGlobalBroadcast(string[] str)
        {
            players = new List<string[]>();

            /* Breaking down the details/updates about the players in the global broadcast message */
            for (int i = 1; i <= noOfPlayers; i++)
            {
                string[] temp = str[i].Split(delim);
                players.Add(temp);
            }

            /* Breaking down the updates about the brick damages in the global broadcast message */
            string[] s = str[noOfPlayers+1].Split(delim); 
            int[,] brickdamage = new int[bricks.Count, 3];

            for (int i = 0, k = 0; i < s.Length; i+=3, k++)
            {
                brickdamage[k, 0] = Int32.Parse(s[i]);
                brickdamage[k, 1] = Int32.Parse(s[i+1]);
                brickdamage[k, 2] = Int32.Parse(s[i+2]);
            }
                
            GlobalBroadcast(players, brickdamage);
        }

        /*private void decodeCoinPile(string[] str)
        {
            if(coinpiles == null)
                coinpiles = new List<Position>();

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
                lifepacks = new List();

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
            return players[0];
        }

        public Player[] getOpponents()
        {
            return opponents;
        }

        public List getCoinPiles()
        {
            return coinpiles;
        }

        public List getLifePacks()
        {
            return lifepacks;
        }

        public List getBricks()
        {
            return bricks;
        }

        public List getStones()
        {
            return stone;
        }

        public List getWater()
        {
            return water;
        }*/
    }
}
