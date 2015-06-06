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
    public class EmptyCell : Cell
    {
        public EmptyCell()
        {
            texture = Util.Content.Load<Texture2D>("sand");
            isPassable = false;
        }

        public override void Draw()
        {
            
        }
    }
}
