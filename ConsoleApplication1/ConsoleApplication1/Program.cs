using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    class Program
    {
        static void Main(string[] args)
        {
            MessageParser parser = new MessageParser("localhost");

            parser.sendMessage("JOIN#");
            parser.getMessage();
        }
    }
}
