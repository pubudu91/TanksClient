using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameEntity;

namespace GameLogic.Managers
{
    public class PlayerManager
    {
        private static PlayerManager playermanager;

        //private List<Player> players;
        private Dictionary<string, Player> players;
        private Player me;
        
        private PlayerManager()
        {
            players = new Dictionary<string, Player>();
        }

        public static PlayerManager getInstance()
        {
            if (playermanager == null)
                playermanager = new PlayerManager();

            return playermanager;
        }

        public void init(Player me)
        {
            this.me = me;
            //players.Insert(0, me);
            players.Add(me.id, me);
        }

        public void setupPlayers(List<string[]> decodedPlayers)
        {
            foreach (string[] record in decodedPlayers)
            {
                if (record[0].Equals(me.id))
                {
                    me.x = Int32.Parse(record[1]);
                    me.y = Int32.Parse(record[2]);
                    me.direction = getDirection(record[3]);

                    /*switch (record[3])
                    {
                        case "0": me.direction = Direction.NORTH; break;
                        case "1": me.direction = Direction.EAST; break;
                        case "2": me.direction = Direction.SOUTH; break;
                        case "3": me.direction = Direction.WEST; break;
                        default: Console.WriteLine("Invalid direction: -{0}-", record[3]); break;
                    }*/
                }
                else
                {
                    Player p = new Player();
                    p.id = record[0];
                    p.x = Int32.Parse(record[1]);
                    p.y = Int32.Parse(record[2]);
                    p.direction = getDirection(record[3]);

                    /*switch (record[3])
                    {
                        case "0": p.direction = Direction.NORTH; break;
                        case "1": p.direction = Direction.EAST; break;
                        case "2": p.direction = Direction.SOUTH; break;
                        case "3": p.direction = Direction.WEST; break;
                        default: Console.WriteLine("Invalid direction: -{0}-", record[3]); break;
                    }*/

                    players.Add(p.id, p);
                }
            }
        }

        public void updatePlayers(List<string[]> plyrs)
        {
            for (int i = 0; i < plyrs.Count; i++)
            {
                Player temp = players[plyrs[i][0]];
                temp.x = Int32.Parse(plyrs[i][1]);
                temp.y = Int32.Parse(plyrs[i][2]);
                temp.direction = getDirection(plyrs[i][3]);
                temp.wasShot = Int32.Parse(plyrs[i][4]) == 0 ? false : true;
                temp.health = Int32.Parse(plyrs[i][5]);
                temp.coins = Int32.Parse(plyrs[i][6]); // Coins total or coins gathered within that second?
                temp.points = Int32.Parse(plyrs[i][7]);
            }
        }

        public void printPlayers()
        {
            foreach (Player p in players.Values)
            {
                Console.Write("id: " + p.id);
                Console.Write(", x: " + p.x);
                Console.Write(", y: " + p.y);
                Console.Write(", points: " + p.points);
                Console.Write(", health: " + p.health);
                Console.Write(", direction: " + p.direction);
                Console.WriteLine();
                Console.WriteLine();
            }
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
    }
}
