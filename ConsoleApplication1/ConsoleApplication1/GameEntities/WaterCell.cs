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
    public class WaterCell : Cell
    {
        public WaterCell()
        {
            texture = Util.Content.Load<Texture2D>("water");
            isPassable = false;
        }

        public override void Draw()
        {
            
        }
    }
}
