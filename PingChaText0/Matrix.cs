using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingChaText0
{
    class Matrix
    {
        //矩阵打包成类，矩阵为m * n
        double[,] A;
        int m, n;
        string name;
        public Matrix(int am, int an)
        {
            m = am;
            n = an;
            A = new double[m, n];
            name = "Result";
        }
        public Matrix(int am, int an, string aName)
        {
            m = am;
            n = an;
            A = new double[m, n];
            name = aName;
        }

        public int getM
        {
            get { return m; }
        }
        public int getN
        {
            get { return n; }
        }
        public double[,] Detail
        {
            get { return A; }
            set { A = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}



