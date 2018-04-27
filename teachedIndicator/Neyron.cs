using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace teachedIndicator
{
    public class Neyron
    {
        private int Size;
        private double[] InputValues;
        private double[] Weights;
        private double OutputValue;
        private int Num;


        public Neyron(int size, int num, Random r)
        {
            Size = size;
            Num = num;
            InputValues = new double[Size];
            Weights = new double[Size + 1];
            //Random r = new Random();
            for (var i = 0; i < Weights.Length; i++)
            {
                Weights[i] = Convert.ToDouble(r.Next(50, 200)) * Math.Sign(0.5 - r.Next(10) % 2) / 100;
            }
        }

        public double Execute(double[] input)
        {
            InputValues = input;
            double totalsum = 0;

            //Parallel.For<double>(0, Size, () => 0, (i, loop, sum) =>
            //{
            //    sum += InputValues[i] * Weights[i];
            //    return sum;
            //},
            //    (sum) => { totalsum += sum; }
            //);

            for (int i = 0; i < Size; i++)
            {
                totalsum += InputValues[i] * Weights[i];
            }
            totalsum -= Weights[Size];//порог активации

            OutputValue = 1 / (1 + Math.Exp(-totalsum));//экспоненциальная сигмоида

            return OutputValue;
        }

        public void CorrectCoeffs(double amendments)
        {
            for (int i = 0; i < Size; i++)
            {
                Weights[i] += amendments * InputValues[i];
            }
            Weights[Size] -= amendments;
        }
        public void CorrectCoeffs(double amendments, out double[] outOldWeights)
        {
            outOldWeights = new double[Size + 1];
            for (int i = 0; i < Size; i++)
            {
                outOldWeights[i] = Weights[i];
                Weights[i] += amendments * InputValues[i];
            }
            outOldWeights[Size] = Weights[Size];
            Weights[Size] -= amendments;
        }

        public void PrintCoeffs(int round)//Диагностика
        {
            StreamWriter sw = new StreamWriter(@"Rez.csv", true, System.Text.Encoding.Default);
            Console.Write("Koeffs for neyron: " + Num + " - ");
            foreach (double w in Weights)
            {
                Console.Write(Math.Round(w, round) + "  ");
                sw.Write(Math.Round(w, round).ToString() + ";");
            }
            sw.WriteLine();
            Console.WriteLine();        
            sw.Close(); ///sdasdas
        }
    }
}