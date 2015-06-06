using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Core;

namespace GameEntity
{
    public class CoinPile : Cell
    {
        public int lifetime { get; set; }
        public int value { get; set; }
        public long timestamp { get; set; }

        public CoinPile()
        {
            F = -1;
            G = 0;
            H = 0;
            priority = 5;
            texture = Util.Content.Load<Texture2D>("coin");
            isPassable = true;
        }

        public override void Draw()
        {
            
        }
    }
}
