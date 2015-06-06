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
    public class StoneCell : Cell
    {
        public StoneCell()
        {
            texture = Util.Content.Load<Texture2D>("stone");
            isPassable = false;
        }

        public override void Draw()
        {
            
        }
    }
}
