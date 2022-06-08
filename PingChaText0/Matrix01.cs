using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingChaText0
{
    class Matrix01
    {
        ///   <summary> 
        ///   矩阵的转置 
        ///   </summary> 
        ///   <param   name= "Matrix0 "> </param> 
        public static double[,] Transpose(double[,] Matrix0)
        {
            int row = Matrix0.GetLength(0);
            int column = Matrix0.GetLength(1);
            //double[,] Matrix0 = new double[column, row];
            double[,] TempMatrix = new double[row, column];
            double[,] MatrixT = new double[column, row];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    TempMatrix[i, j] = Matrix0[i, j];
                }
            }
            for (int i = 0; i < column; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    MatrixT[i, j] = TempMatrix[j, i];
                }
            }
            return MatrixT;

        }

        ///   <summary> 
        ///   矩阵的逆矩阵 
        ///   </summary> 
        ///   <param   name= "Matrix0 "> </param> 
        public static double[,] Athwart(double[,] Matrix0)
        {
            int i = 0;
            int row = Matrix0.GetLength(0);
            double[,] Matrix2 = new double[row, row * 2];
            double[,] MatrixInv = new double[row, row];
            for (i = 0; i < row; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    Matrix2[i, j] = Matrix0[i, j];
                }
            }
            for (i = 0; i < row; i++)
            {
                for (int j = row; j < row * 2; j++)
                {
                    Matrix2[i, j] = 0;
                    if (i + row == j)
                        Matrix2[i, j] = 1;
                }
            }

            for (i = 0; i < row; i++)
            {
                if (Matrix2[i, i] != 0)
                {
                    double intTemp = Matrix2[i, i];
                    for (int j = 0; j < row * 2; j++)
                    {
                        Matrix2[i, j] = Matrix2[i, j] / intTemp;
                    }
                }
                for (int j = 0; j < row; j++)
                {
                    if (j == i)
                        continue;
                    double intTemp = Matrix2[j, i];
                    for (int k = 0; k < row * 2; k++)
                    {
                        Matrix2[j, k] = Matrix2[j, k] - Matrix2[i, k] * intTemp;
                    }
                }
            }

            for (i = 0; i < row; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    MatrixInv[i, j] = Matrix2[i, j + row];
                }
            }
            return MatrixInv;
        }

        ///   <summary> 
        ///   矩阵加法 
        ///   </summary> 
        ///   <param   name= "Matrix1 "> </param> 
        ///   <param   name= "Matrix2 "> </param> 
        public static double[,] AddMatrix(double[,] Matrix1, double[,] Matrix2)
        {
            double[,] MatrixResult = new double[Matrix1.GetLength(0), Matrix2.GetLength(1)];
            for (int i = 0; i < Matrix1.GetLength(0); i++)
                for (int j = 0; j < Matrix2.GetLength(1); j++)
                    MatrixResult[i, j] = Matrix1[i, j] + Matrix2[i, j];
            return MatrixResult;
        }

        ///   <summary> 
        ///   矩阵减法 
        ///   </summary> 
        ///   <param   name= "Matrix1 "> </param> 
        ///   <param   name= "Matrix2 "> </param> 
        public static double[,] SubMatrix(double[,] Matrix1, double[,] Matrix2)
        {
            double[,] MatrixResult = new double[Matrix1.GetLength(0), Matrix2.GetLength(1)];
            for (int i = 0; i < Matrix1.GetLength(0); i++)
                for (int j = 0; j < Matrix2.GetLength(1); j++)
                    MatrixResult[i, j] = Matrix1[i, j] - Matrix2[i, j];
            return MatrixResult;
        }

        ///   <summary> 
        ///   矩阵乘法 
        ///   </summary> 
        ///   <param   name= "Matrix1 "> </param> 
        ///   <param   name= "Matrix2 "> </param> 
        public static double[,] MultiplyMatrix(double[,] Matrix1, double[,] Matrix2)
        {
            double[,] MatrixResult = new double[Matrix1.GetLength(0), Matrix2.GetLength(1)];
            for (int i = 0; i < Matrix1.GetLength(0); i++)
            {
                for (int j = 0; j < Matrix2.GetLength(1); j++)
                {
                    for (int k = 0; k < Matrix1.GetLength(1); k++)
                    {
                        MatrixResult[i, j] += Matrix1[i, k] * Matrix2[k, j];
                    }
                }
            }
            return MatrixResult;
        }

        ///   <summary> 
        ///   矩阵对应行列式的值 
        ///   </summary> 
        ///   <param   name= "Matrix1 "> </param> 
        ///   <returns> </returns> 
        public static double ResultDeterminant(double[,] Matrix1)
        {
            return Matrix1[0, 0] * Matrix1[1, 1] * Matrix1[2, 2] + Matrix1[0, 1] * Matrix1[1, 2] * Matrix1[2, 0] + Matrix1[0, 2] * Matrix1[1, 0] * Matrix1[2, 1]
            - Matrix1[0, 2] * Matrix1[1, 1] * Matrix1[2, 0] - Matrix1[0, 1] * Matrix1[1, 0] * Matrix1[2, 2] - Matrix1[0, 0] * Matrix1[1, 2] * Matrix1[2, 1];

        }
    }
}
