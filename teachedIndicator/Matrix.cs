using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace teachedIndicator
{
    public class Matrix
    {
        public double[][] Table { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }

        public Matrix(int row, int column)
        {
            Row = row;
            Column = column;
            Table = new double[Row][];
            for (int i = 0; i < Row; i++)
            {
                Table[i] = new double[Column];
            } 
        }

        public Matrix(string fileName)
        {
            bool isFirst = true;
            var path = $"{Environment.CurrentDirectory}\\{fileName}";
            using (var streamReader = new StreamReader(path, System.Text.Encoding.Default))
            {
                string fileText = streamReader.ReadToEnd();
                string[] lines = fileText.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                Row = lines.Length;
               
                for (var i = 0; i < lines.Length; i++)
                {
                    string[] numbers = lines[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (isFirst)
                    {
                        Column = numbers.Length;
                        Table = new double[Row][];
                        for (int k = 0; k < Row; k++)
                        {
                            Table[k] = new double[Column];
                        }
                        isFirst = false;
                    }

                    for (int j = 0; j < numbers.Length; j++)
                    {
                       if(!double.TryParse(numbers[j], out Table[i][j]))
                            throw new ArgumentException();
                    }
                }
            }
        }
        public Matrix(Matrix xMatrix, Matrix yMatrix, int yColumn)
        {
            Row = xMatrix.Row;
            Column = xMatrix.Column + 1;
            Table = new double[Row][];
            for (int i = 0; i < Row; i++)
            {
                Table[i] = new double[Column];
            }

            for (int r = 0; r < Row; r++)
            {
                for (int c = 0; c < xMatrix.Column; c++)
                {
                    Table[r][c] = xMatrix.Table[r][c];
                }
                Table[r][xMatrix.Column] = yMatrix.Table[r][yColumn];
            }
        }

        public void PrintMatrix()
        {
            Console.WriteLine();

            for (var i = 0; i < Row; i++)
            {
                for (var j = 0; j < Column; j++)
                    Console.Write($"{Math.Round(Table[i][j], 2),-7}");
                Console.WriteLine();
                Console.WriteLine();
            }
        }
    }
}

