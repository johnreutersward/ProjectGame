using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectGame
{
    static class Debug
    {
        public static void Print(String msg)
        {
            //We could use this to save debug onto a text file here aswell
            DateTime currentTime = DateTime.Now.ToUniversalTime();
            Console.WriteLine(msg + " [" + currentTime + "]");
        }
    }
}
