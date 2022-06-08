using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingChaText0
{
    class MatrixOperations
    {
        /***********矩阵运算***********/
        //矩阵加法
        public static Matrix MatrixAdd(Matrix Ma, Matrix Mb)
        {
            int m = Ma.getM;
            int n = Ma.getN;
            int m2 = Mb.getM;
            int n2 = Mb.getN;

            if ((m != m2) || (n != n2))
            {
                Exception myException = new Exception("数组维数不匹配");
                throw myException;
            }

            Matrix Mc = new Matrix(m, n);
            double[,] c = Mc.Detail;
            double[,] a = Ma.Detail;
            double[,] b = Mb.Detail;

            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    c[i, j] = a[i, j] + b[i, j];
            return Mc;
        }

        //矩阵减法
        public static Matrix MatrixSub(Matrix Ma, Matrix Mb)
        {
            int m = Ma.getM;
            int n = Ma.getN;
            int m2 = Mb.getM;
            int n2 = Mb.getN;
            if ((m != m2) || (n != n2))
            {
                Exception myException = new Exception("数组维数不匹配");
                throw myException;
            }
            Matrix Mc = new Matrix(m, n);
            double[,] c = Mc.Detail;
            double[,] a = Ma.Detail;
            double[,] b = Mb.Detail;

            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    c[i, j] = a[i, j] - b[i, j];
            return Mc;
        }

        //矩阵乘法
        public static Matrix MatrixMulti(Matrix Ma, Matrix Mb)
        {
            int m = Ma.getM;
            int n = Ma.getN;
            int m2 = Mb.getM;
            int n2 = Mb.getN;

            if (n != m2)
            {
                Exception myException = new Exception("数组维数不匹配");
                throw myException;
            }

            Matrix Mc = new Matrix(m, n2);
            double[,] c = Mc.Detail;
            double[,] a = Ma.Detail;
            double[,] b = Mb.Detail;

            for (int i = 0; i < m; i++)
                for (int j = 0; j < n2; j++)
                {
                    c[i, j] = 0;
                    for (int k = 0; k < n; k++)
                        c[i, j] += a[i, k] * b[k, j];
                }
            return Mc;

        }

        //矩阵数乘
        public static Matrix MatrixSimpleMulti(double k, Matrix Ma)
        {
            int m = Ma.getM;
            int n = Ma.getN;
            Matrix Mc = new Matrix(m, n);
            double[,] c = Mc.Detail;
            double[,] a = Ma.Detail;

            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    c[i, j] = a[i, j] * k;
            return Mc;
        }

        //矩阵转置
        public static Matrix MatrixTrans(Matrix MatrixOrigin)
        {
            int m = MatrixOrigin.getM;
            int n = MatrixOrigin.getN;
            Matrix MatrixNew = new Matrix(n, m);
            double[,] c = MatrixNew.Detail;
            double[,] a = MatrixOrigin.Detail;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    c[i, j] = a[j, i];
            return MatrixNew;
        }

        //矩阵求逆（伴随矩阵法）
        public static Matrix MatrixInvByCom(Matrix Ma)
        {
            double d = MatrixDet(Ma);
            if (d == 0)
            {
                Exception myException = new Exception("没有逆矩阵");
                throw myException;
            }
            Matrix Ax = MatrixCom(Ma);
            Matrix An = MatrixSimpleMulti((1.0 / d), Ax);
            return An;
        }
        //对应行列式的代数余子式矩阵
        public static Matrix MatrixSpa(Matrix Ma, int ai, int aj)
        {
            int m = Ma.getM;
            int n = Ma.getN;
            if (m != n)
            {
                Exception myException = new Exception("矩阵不是方阵");
                throw myException;
            }
            int n2 = n - 1;
            Matrix Mc = new Matrix(n2, n2);
            double[,] a = Ma.Detail;
            double[,] b = Mc.Detail;

            //左上
            for (int i = 0; i < ai; i++)
                for (int j = 0; j < aj; j++)
                {
                    b[i, j] = a[i, j];
                }
            //右下
            for (int i = ai; i < n2; i++)
                for (int j = aj; j < n2; j++)
                {
                    b[i, j] = a[i + 1, j + 1];
                }
            //右上
            for (int i = 0; i < ai; i++)
                for (int j = aj; j < n2; j++)
                {
                    b[i, j] = a[i, j + 1];
                }
            //左下
            for (int i = ai; i < n2; i++)
                for (int j = 0; j < aj; j++)
                {
                    b[i, j] = a[i + 1, j];
                }
            //符号位
            if ((ai + aj) % 2 != 0)
            {
                for (int i = 0; i < n2; i++)
                    b[i, 0] = -b[i, 0];

            }
            return Mc;

        }

        //矩阵的行列式,矩阵必须是方阵
        public static double MatrixDet(Matrix Ma)
        {
            int m = Ma.getM;
            int n = Ma.getN;
            if (m != n)
            {
                Exception myException = new Exception("数组维数不匹配");
                throw myException;
            }
            double[,] a = Ma.Detail;
            if (n == 1) return a[0, 0];

            double D = 0;
            for (int i = 0; i < n; i++)
            {
                D += a[1, i] * MatrixDet(MatrixSpa(Ma, 1, i));
            }
            return D;
        }

        //矩阵的伴随矩阵
        public static Matrix MatrixCom(Matrix Ma)
        {
            int m = Ma.getM;
            int n = Ma.getN;
            Matrix Mc = new Matrix(m, n);
            double[,] c = Mc.Detail;
            double[,] a = Ma.Detail;

            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    c[i, j] = MatrixDet(MatrixSpa(Ma, j, i));

            return Mc;
        }

    }
}

