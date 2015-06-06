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
    public class LifePack : Cell
    {
        public int lifetime { get; set; }
        public long timestamp { get; set; }

        public LifePack()
        {
            texture = Util.Content.Load<Texture2D>("lifepack");
            isPassable = true;
        }

        public override void Draw()
        {
            
        }
    }
}
