using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace teachedIndicator
{
    class Program
    {
        static void Main(string[] args)
        {
            var TheNet = new NeyroNet(7, 4, 10);
            TheNet.Study("x.txt", "y.txt", 0.5, 100000);
            TheNet.Print();
            Console.ReadKey();
            // dsadsa
        }
    }
}
 