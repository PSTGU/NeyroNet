using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teachedIndicator
{
    public class NeyroNet
    {
        private int NumOfInputs, Size, NumOfOutputs;
        private double[] inputs, middle, outputs;
        //private Neyron[] workLoyer, outLoyer;
        private Layer workLoyer, outLoyer;
        private Matrix workLoyerCoeffs, outLoyerCoeffs;

        public NeyroNet(int ins, int outs, int size)
        {
            NumOfInputs = ins;
            Size = size;
            NumOfOutputs = outs;
            
            inputs = new double[ins];
            middle = new double[size];
            outputs = new double[outs];

            //workLoyer = new Neyron[size];
            //outLoyer = new Neyron[outs];
            workLoyer = new Layer(Size, NumOfInputs);
            outLoyer = new Layer(NumOfOutputs, Size);

            workLoyerCoeffs = new Matrix("wl.txt");
            outLoyerCoeffs = new Matrix("ol.txt");

            //for (int outNeyronNum = 0; outNeyronNum < outLoyer.Length; outNeyronNum++)
            //{
            //    outLoyer[outNeyronNum] = new Neyron(size, outNeyronNum, r);
            //}
            //for (int workNeyronNum = 0; workNeyronNum < workLoyer.Length; workNeyronNum++)
            //{
            //    workLoyer[workNeyronNum] = new Neyron(ins, workNeyronNum, r);
            //}
        }
        public double[] Exe(double[] question)
        {
            inputs = question;
            middle = workLoyer.Exe(inputs);
            outputs = outLoyer.Exe(middle);
            return outputs;            
            
        }
        public double[] QuickExe(double[] question)
        {
            Matrix workMatr = new Matrix(question);
            Matrix inter = workLoyerCoeffs.MatrixMultuplyParallel(workMatr);
            for (int i = 0; i < inter.Column; i++)
            {
                inter.Table[0][i] = 1 / (1 + Math.Exp(-inter.Table[0][i]));
            }
            workMatr = new Matrix(inter.Table[0]);
            inter = outLoyerCoeffs.MatrixMultuplyParallel(workMatr);
            for (int i = 0; i < inter.Column; i++)
            {
                inter.Table[0][i] = 1 / (1 + Math.Exp(-inter.Table[0][i]));
            }
            return inter.Table[0];
        }
        public void Study(string examplesAddress, string correctAnswersAddress, double step, long iter)
        {
            Matrix examples = new Matrix(examplesAddress);
            Matrix correctAnswers = new Matrix(correctAnswersAddress);
            System.Diagnostics.Stopwatch swatch = new System.Diagnostics.Stopwatch(); // создаем объект
            swatch.Start(); // старт
            for (long i = 0; i < iter; i++)
            {
                for (int j = 0; j < examples.Row; j++)
                {
                    Exe(examples.Table[j]);
                    double[] outGadients = new double[NumOfOutputs];
                    Matrix oldOutLoyerWeights = new Matrix(NumOfOutputs, Size);
                   
                    for (int outNeyronNum = 0; outNeyronNum < NumOfOutputs; outNeyronNum++)
                    {
                        outGadients[outNeyronNum] = (correctAnswers.Table[j][outNeyronNum] - outputs[outNeyronNum]) * outputs[outNeyronNum] * (1 - outputs[outNeyronNum]) * step;
                    }
                    outLoyer.CorrectCoeffs(outGadients, out oldOutLoyerWeights);
                   
                    double[] workGadients = new double[Size];
                    for (int workNeyronNum = 0; workNeyronNum < Size; workNeyronNum++)
                    {
                        double sumMistake = 0;
                        for (int L = 0; L < NumOfOutputs; L++)
                        {
                            sumMistake += oldOutLoyerWeights.Table[L][workNeyronNum] * outGadients[L];
                        }
                        workGadients[workNeyronNum] = sumMistake * middle[workNeyronNum] * (1 - middle[workNeyronNum]);
                    }
                    workLoyer.CorrectCoeffs(workGadients);
                       // workLoyer[workNeyronNum].CorrectCoeffs(gradient);
                }
            }
            swatch.Stop(); // стоп
            Console.WriteLine(swatch.Elapsed);
        }

        public void Print()
        {
            outLoyerCoeffs = outLoyer.Print(5, "ol.txt");
            workLoyerCoeffs = workLoyer.Print(5, "wl.txt");
        }

    }
}
