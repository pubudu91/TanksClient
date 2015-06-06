using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogic.Managers;

namespace Core
{
    class Program
    {
        static void Main(string[] args)
        {
            GameManager manager = GameManager.getInstance();
            MessageParser parser = MessageParser.getInstance();

            parser.sendMessage("JOIN#");
            parser.getMessage();
        }
    }
}
