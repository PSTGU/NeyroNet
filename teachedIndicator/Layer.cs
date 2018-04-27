using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teachedIndicator
{
    class Layer
    {
        int Size, Inputs;
        Neyron[] neyronArray;
        
        public Layer(int size, int ins)
        {
            Random r = new Random();
            Size = size;
            Inputs = ins;
            neyronArray = new Neyron[Size];
            for (int neyronNum = 0; neyronNum < Size; neyronNum++)
            {
                neyronArray[neyronNum] = new Neyron(Inputs, neyronNum, r);
            }
        } 
        public double[] Exe(double [] input)
        {
            double[] output = new double[Size];
            for (int i = 0; i < Size; i++)
            {
                output[i] = neyronArray[i].Execute(input);
            }
            return output;
        }
        public void CorrectCoeffs (double[] amendments, out Matrix oldWeights)
        {
            oldWeights = new Matrix(Size, Inputs);
            for (int outNeyronNum = 0; outNeyronNum < Size; outNeyronNum++)
            {
                neyronArray[outNeyronNum].CorrectCoeffs(amendments[outNeyronNum], out oldWeights.Table[outNeyronNum]);
            }  
        }
        public void CorrectCoeffs(double[] amendments)
        {
            for (int outNeyronNum = 0; outNeyronNum < Size; outNeyronNum++)
            {
                neyronArray[outNeyronNum].CorrectCoeffs(amendments[outNeyronNum]);
            }
        }
        public void Print(int round)
        {
            for (int neyronNum = 0; neyronNum < Size; neyronNum++)
            {
                neyronArray[neyronNum].PrintCoeffs(round);
            }
        }
    }
}
