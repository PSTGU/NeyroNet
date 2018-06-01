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
        [STAThread]
        static void Main(string[] args)
        {
            var TheNet = new NeyroNet(3500, 4, 10);
            //var TheNet = new NeyroNet(7, 4, 10);
            TheNet.Study("x.txt", "y.txt", 0.5, 15000);
            TheNet.Print();
            Console.ReadKey();
            TheNet.Work();
        }
    }
}
 