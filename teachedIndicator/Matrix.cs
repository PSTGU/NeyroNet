using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;

using System.Windows;
//using System.Windows.Controls;
using System.Windows.Forms;


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
                string[] lines = fileText.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

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
                        if (!double.TryParse(numbers[j], out Table[i][j]))
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
        public Matrix(double[] arr)
        {
            Row = 1;
            Column = arr.Length + 1;
            Table = new double[Row][];
            Table[0] = new double[Column];
            for (int i = 0; i < arr.Length; i++)
            {
                Table[0][i] = arr[i];
            }
            Table[0][arr.Length] = -1;
        }

        public Matrix(int size)
        {
            var dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            var files = Directory.GetFiles(dialog.SelectedPath);
            bool isfirst = true;
            int count = 0;
            foreach (var f in files)
            {
                Bitmap image1 = new Bitmap(f, true);
                double[] vec = new double[image1.Width * image1.Height];
                if (isfirst)
                {
                    Row = size;
                    Column = image1.Width * image1.Height;
                    Table = new double[Row][];
                    isfirst = false;
                }
                for (int x = 0; x < image1.Width; x++)
                {
                    for (int y = 0; y < image1.Height; y++)
                    {
                        Color from = image1.GetPixel(x, y);
                        if (Math.Max(from.G, Math.Max(from.B, from.R)) < 124)
                            vec[x * image1.Height + y] = 1;
                        else
                            vec[x * image1.Height + y] = 0;
                    }
                }
                Table[count] = vec;
                count++;
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

        //1x8 on 10x8
        public Matrix MatrixMultuplyParallel(Matrix left)
        {
            if (left.Column != Column)
            {
                throw new ArgumentException("Uncorrect lenght of matrix");
            }
            Matrix result = new Matrix(left.Row, Row);
            for (int i = 0, len = result.Row; i < len; i++)
            {
                result.Table[i] = new double[Row];
            }
            Parallel.For(0, Row, (i) =>
            {
                for (int j = 0; j < left.Row; j++)
                {
                    double summ = 0;
                    for (int n = 0; n < left.Column; n++)
                    {
                        summ += left.Table[j][n] * Table[i][n];
                    }
                    result.Table[j][i] = summ;
                }
            });
            return result;
        }

    }

    /* examples */
    // vec to matrix
    /*Matrix m = new Matrix();
    double[] left_vec = new double[3] { 3, 2, 1 };
    double[][] left = new double[1][] { left_vec }; //new double[3][] { new double[] { 3, 2, 1 }, new double[] { 1, 2, 3 }, new double[] { 1, 1, 1 } };
    double[][] result = m.MatrixMultuplyParallel(left);
    double[] res2 = result[0];*/
    // matr to matr
    /*Matrix m = new Matrix();
    double[][] left = new double[3][] { new double[] { 3, 2, 1 }, new double[] { 1, 2, 3 }, new double[] { 1, 1, 1 } };
    double[][] result = m.MatrixMultuplyParallel(left);*/

    //Console.WriteLine(Show_dbg(result));




}

