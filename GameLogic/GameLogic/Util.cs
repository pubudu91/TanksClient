using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Core
{
    public class Util
    {
        public static string SERVERIP = "localhost";
        public static string MYIP = "127.0.0.1";
        public static ContentManager Content;
        public static SpriteBatch sprite;
        public static float scale = 0.75f;
    }
}
