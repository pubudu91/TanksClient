using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace GameEntity
{
    public abstract class Cell
    {
        public int x { get; set; }
        public int y { get; set; }
        protected Texture2D texture { get; set; }
        public bool isPassable { get; set; }
        public abstract void Draw();
    }
}
