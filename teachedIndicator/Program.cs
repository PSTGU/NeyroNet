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
            TheNet.Study("x.txt", "y.txt", 0.5, 5000);
            TheNet.Print();
            Console.ReadKey();
            string[] question = new string[7];
            double[] vect = new double[7];
            while (true)
            {
                Console.WriteLine("Input vector:");
                question = Console.ReadLine().Split(' ');
                int i = 0;
                foreach (string ch in question)
                {
                    vect[i] = Double.Parse(question[i]);
                    i++;
                }
                double[] ans = new double[4];
                ans = TheNet.Exe(vect);
                foreach (double d in ans)
                {
                    Console.Write(Math.Round(d, 1));
                }
            }
            
        }
    }
}
 