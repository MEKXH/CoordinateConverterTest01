using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PingChaText0
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //导入区域坐标数据
        private void 导入区域坐标ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog Open01 = new OpenFileDialog();
            //设置标题
            Open01.Title = "请选择文件";
            Open01.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //文件格式过滤
            Open01.Filter = "(*.txt)|*.txt|(*.dat)|*.dat";
            //如果用户没有点击“确定”按钮
            if (Open01.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            dataGridView1.Rows.Clear();
            var errInfo = "";
            var Ok01 = InputData(Open01.FileName, ref errInfo);
            if (!Ok01)
            {
                MessageBox.Show(errInfo, "温馨提示");
            } //end if
        }

        //导入区域坐标方法
        private bool InputData(string fileName, ref string errInfo)
        {
            var Ok01 = true;
            var datas = File.ReadAllLines(fileName);
            //行计数器
            var i = 0;
            //遍历数据行，显示到表格中
            foreach (var data in datas)
            {
                i++;
                var infos = data.Split(',');
                //判断数据格式是否正确：数据必须有4列
                if (infos.Length != 4)
                {
                    errInfo += "文件第 " + i + "行数据格式有误！已过滤";
                    Ok01 = false;
                }
                else
                {
                    //将数据显示在表格中
                    dataGridView1.Rows.Add(infos);
                }
            }
            return Ok01;
        }


        //导出转换后坐标
        private void 导出转换后坐标ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog Sf01 = new SaveFileDialog();
            //设置文件保存类型
            Sf01.Filter = "(*.txt)|*.txt|(*.dat)|*.dat";
            Sf01.RestoreDirectory = true;
            Sf01.AddExtension = true;
            //设置对话框标题
            Sf01.Title = "保存文件";
            if (Sf01.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            
            var Ok02 = DataTableToTxt(dataGridView3, Sf01.FileName,',' );
            MessageBox.Show("保存成功", "温馨提示");
            if (!Ok02)
            {
                MessageBox.Show("保存出错,请检查表格中数据是否为空或有误", "温馨提示");
            } 

            
        }
        //导出转换后坐标方法
        //strFileName为文件名，strSplit为文件中数据间的分隔符
        private static bool DataTableToTxt(DataGridView gridview, string strFileName, char strSplit)
        {
            if (gridview == null || gridview.Rows.Count == 0)
                return false;

            FileStream FS01 = new FileStream(strFileName, FileMode.OpenOrCreate);
            StreamWriter SW01 = new StreamWriter(FS01, Encoding.UTF8);
            StringBuilder strBuilder01 = new StringBuilder();

            try
            {
                for (int i = 0; i < gridview.Rows.Count; i++)
                {
                    strBuilder01 = new StringBuilder();
                    for (int j = 0; j < gridview.Columns.Count; j++)
                    {
                        strBuilder01.Append(gridview.Rows[i].Cells[j].Value.ToString() + strSplit);
                    }
                    strBuilder01.Remove(strBuilder01.Length - 1, 1); // 将最后添加的一个分隔符删除
                    SW01.WriteLine(strBuilder01.ToString());
                }
            }
            catch (Exception ex)
            {
                string strErrorMessage = ex.Message;
                return false;
            }
            finally
            {
                SW01.Close();
                FS01.Close();
            }
            return true;
        }
        //计算七参数
        private void 计算转换7参数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }
       //private void button3_Click(object sender, EventArgs e)
       // {
       //     /*******************数据录入对应矩阵开始**************/
       //     //计算第一点的B，L
       //     Matrix sumB = GetItsB(_f1[0, 0], _f1[0, 1], _f1[0, 2]);
       //     Matrix sumL = new Matrix(3, 1, new double[3] { _f2[0, 0], _f2[0, 1], _f2[0, 2] });
       //     //计算后续点的B，L 并将之按行添加到第一点末尾   
       //     for (int i = 1; i < 6; i++)
       //     {
       //         Matrix _B = GetItsB(_f1[i, 0], _f1[i, 1], _f1[i, 2]);
       //         sumB = sumB.Addinend(_B);
       //         Matrix _L = new Matrix(3, 1, new double[3] { _f2[i, 0], _f2[i, 1], _f2[i, 2] });
       //         sumL = sumL.Addinend(_L);
       //     }
       //     /*******************数据录入对应矩阵结束**************/

       //     /*******************计算开始*****************/
       //     //X阵求解,V阵求解
       //     Matrix N = new Matrix(sumB.Transpose() * sumB);
       //     N.InvertGaussJordan();
       //     Matrix X = new Matrix(N.Multiply(-1) * sumB.Transpose() * sumL);
       //     Matrix V = new Matrix(sumB * X + sumL);
       //     /*******************计算结束*****************/


       //     /*******************结果输出开始*****************/
       //     double m0 = Math.Sqrt((V.Transpose() * V).elements[0] / 11);
       //     textBox3.Text = "平移X0:" + X.elements[0] + "m" + "\r\n" +
       //                   "平移Y0:" + X.elements[1] + "m   " + "\r\n" +
       //                   "平移Z0:" + X.elements[2] + "m  " + "\r\n" +
       //                   "尺度 K:" + Math.Pow(10, 6) * (X.elements[3] - 1) + "ppm   " + "\r\n" +
       //                   "旋转 X:" + 180 * 60 * 60 * (X.elements[4] / X.elements[3]) / Math.PI + "s   " + "\r\n" +
       //                   "旋转 Y:" + 180 * 60 * 60 * (X.elements[5] / X.elements[3]) / Math.PI + "s   " + "\r\n" +
       //                   "旋转 Z:" + 180 * 60 * 60 * (X.elements[6] / X.elements[3]) / Math.PI + "s   " + "\r\n" +
       //                   "单位权中误差m0:" + m0 * Math.Pow(10, 2) + "cm   ";
       //     /*******************结果输出结束*****************/


       // }

       // /**
       //  * 输入一个点的x,y,z输出其B矩阵
       //  */
       // Matrix GetItsB(double X, double Y, double Z)
       // {
       //     double[] toB = new double[21]{1,0,0,X,0,-Z,Y,
       //                                   0,1,0,Y,Z,0,-X,
       //                                   0,0,1,Z,-Y,X,0};
       //     Matrix B = new Matrix(3, 7, toB);
       //     return B.Multiply(-1);

       // }


    }
}
    
