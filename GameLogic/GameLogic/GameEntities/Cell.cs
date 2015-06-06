using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace GameEntity
{
    public abstract class Cell : IEquatable<Cell>
    {
        public int x { get; set; }
        public int y { get; set; }
        protected Texture2D texture { get; set; }
        public bool isPassable { get; set; }
        public int F { get; set; }
        public int G { get; set; }
        public int H { get; set; }
        public int priority { get; set; }
        public abstract void Draw();

        public bool Equals(Cell other)
        {
            if (this.x == other.x && this.y == other.y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int Min(List<Cell> list)
        {
            return list.Min(node => node.F);
        }
    }
}
