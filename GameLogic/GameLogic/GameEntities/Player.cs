using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GameEntity
{
    public class Player
    {
        public string id { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public Direction direction { get; set; }
        public bool wasShot { get; set; }
        public int health { get; set; }
        public int coins { get; set; }
        public int points { get; set; }
        public bool isOpponent { get; set; }
    }
}
