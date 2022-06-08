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
            Open01.Filter = "(*.txt)|*.txt";
            //如果用户没有点击“确定”按钮
            if (Open01.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            dataGridView1.Rows.Clear();
            var errInfo = "";
            var Ok01 = InputData(dataGridView1, Open01.FileName, ref errInfo);
            if (!Ok01)
            {
                MessageBox.Show(errInfo, "温馨提示");
            } //end if
        }
        //导入CGCS2000坐标
        private void 导入CGCS2000坐标ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog Open01 = new OpenFileDialog();
            //设置标题
            Open01.Title = "请选择文件";
            Open01.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //文件格式过滤
            Open01.Filter = "(*.txt)|*.txt";
            //如果用户没有点击“确定”按钮
            if (Open01.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            dataGridView2.Rows.Clear();
            var errInfo = "";
            var Ok01 = InputData(dataGridView2, Open01.FileName, ref errInfo);
            if (!Ok01)
            {
                MessageBox.Show(errInfo, "温馨提示");
            } //end if
        }
        //导入坐标方法
        /// <summary>
        /// 导入坐标
        /// </summary>
        /// <param name="gridview"></param>
        /// <param name="fileName"></param>
        /// <param name="errInfo"></param>
        /// <returns></returns>
        private bool InputData(DataGridView gridview, string fileName, ref string errInfo)
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
                    gridview.Rows.Add(infos);
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

            var Ok02 = DataTableToTxt(dataGridView4, Sf01.FileName, ',');
            MessageBox.Show("保存成功", "温馨提示");
            if (!Ok02)
            {
                MessageBox.Show("保存出错,请检查表格中数据是否为空或有误", "温馨提示");
            }


        }
        //导出转换后坐标方法
        //strFileName为文件名，strSplit为数据间的分隔符
        /// <summary>
        /// 导出转换后坐标
        /// </summary>
        /// <param name="gridview"></param>
        /// <param name="strFileName"></param>
        /// <param name="strSplit"></param>
        /// <returns></returns>
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
                    strBuilder01.Remove(strBuilder01.Length - 1, 1); //将最后添加的一个分隔符删除
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
            int num = 36;
            //Matrix Text = new Matrix(20, 3);
            //double[,] text = new double[20, 3];
            //////////////////////////////////////
            //定义所需的矩阵
            Matrix X = new Matrix(7, 1);            
            Matrix B = new Matrix(num, 7);            
            Matrix V = new Matrix(num, 1);
            Matrix L = new Matrix(num, 1);
            Matrix Place = new Matrix(num, 1);
            Matrix CGCS2000 = new Matrix(num, 1);

            double[,] x = new double[7, 1];
            double[,] b = new double[num,7];           
            double[,] v = new double[num, 1];
            double[,] l = new double[num, 1];
            double[,] place = new double[num, 1];
            double[,] cgcs = new double[num, 1];
            X.Detail = x; B.Detail = b; V.Detail = x; L.Detail = l;
            Place.Detail = place; CGCS2000.Detail = cgcs;
            //导入数据到矩阵Place,CGCS2000
            GetDataFromDGV2(dataGridView1, place);
            GetDataFromDGV2(dataGridView2, cgcs);
            //导入数据到矩阵B
            for (int i = 0; i < num/3; i++)
            {
                //B[0,0],[1,1],[2,2] = 0
                b[3 * i, 0] = 1; b[3 * i + 1, 1] = 1; b[3 * i + 2, 2] = 1;
                //B矩阵中每三行循环赋值
                b[3 * i, 4] = -place[3*i+2, 0]; b[3 * i, 5] = place[3*i+1, 0]; b[3 * i, 6] = place[3*i, 0];//Row0
                b[3 * i + 1, 3] = place[3*i+2, 0]; b[3 * i + 1, 5] = -place[3*i, 0]; b[3 * i + 1, 6] = place[3*i+1, 0];//Row1
                b[3 * i + 2, 3] = -place[3*i+1, 0]; b[3 * i + 2, 4] = place[3*i, 0]; b[3 * i + 2, 6] = place[3*i+2, 0];//Row2
            }
            //计算矩阵L
            L = MatrixOperations.MatrixSub(Place, CGCS2000);
            //计算转换7参数矩阵X
            X = MatrixOperations.MatrixMulti(MatrixOperations.MatrixInvByCom(MatrixOperations.MatrixMulti(MatrixOperations.MatrixTrans(B), B)), MatrixOperations.MatrixMulti(MatrixOperations.MatrixTrans(B), L));
            textBox1.Text = Convert.ToString(X.Detail[3, 0]);
            //////////////////////////////////////////
            //GetDataFromDGV1(dataGridView1, text);
            //GetDataFromDGV2(dataGridView1, v);
            //MessageBox.Show(Convert.ToString(text[4, 1]));
            //MessageBox.Show(Convert.ToString(v[4, 0]));


            //Matrix B = new Matrix(3, 3);
            //Matrix E = new Matrix(3, 3);
            //Matrix X = new Matrix(3, 3);
            //double[,] F;
            //double[,] C = new double[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
            //double[,] D = new double[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
            //B.Detail = C;
            //E.Detail = D;
            // X = MatrixOperations.MatrixAdd(B, E);

            //textBox1.Text = ((Convert.ToString(X.Detail[2,2])));
            //GetDataFromDGV(dataGridView1, B);
        }
        
        

        //从DataGridView中获取数据方法1（X,Y,Z）循环
        /// <summary>
        /// 从DataGridView中获取数据
        /// </summary>
        /// <param name="DGV"></param>
        /// <param name="errInfo"></param>
        /// <returns></returns>
        private bool GetDataFromDGV1(DataGridView DGV, double[,] array)
        {
            for (int i = 0; i < DGV.RowCount; i++)
            {
                for (int j = 1; j < DGV.ColumnCount; j++)
                {
                    array[i,j-1] = Convert.ToDouble(DGV[j, i].Value);                                   
                }
            }
            return true;
        }
        //从DataGridView获取数据方法3（X,Y,Z）T循环(列)
        /// <summary>
        /// 获取数据3
        /// </summary>
        /// <param name="DGV"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        private bool GetDataFromDGV3(DataGridView DGV, double[,] array)
        {
            int a = 0;
            for (int i = 0; i < DGV.RowCount; i++)
            {
                for (int j = 1; j < DGV.ColumnCount; j++)
                {                   
                    array[a, 0] = Convert.ToDouble(DGV[j, i].Value);
                    a = a + 1;                    
                }
            }
            return true;
        }
        //从DataGridView获取数据2（T读取12行）
        /// <summary>
        /// 获取数据2
        /// </summary>
        /// <param name="DGV"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        private bool GetDataFromDGV2(DataGridView DGV, double[,] array)
        {
            int a = 0;
            for (int i = 0; i < 12; i++)
            {
                for (int j = 1; j < DGV.ColumnCount; j++)
                {
                    array[a, 0] = Convert.ToDouble(DGV[j, i].Value);
                    a = a + 1;                    
                }
            }
            return true;
        }
        //获取数据，读取12行（XYZ循环）
        /// <summary>
        /// 从DataGridView获取数据4
        /// </summary>
        /// <param name="DGV"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        private bool GetDataFromDGV4(DataGridView DGV, double[,] array)
        {
            for (int i = 0; i < 12; i++)
            {
                for (int j = 1; j < DGV.ColumnCount; j++)
                {
                    array[i, j - 1] = Convert.ToDouble(DGV[j, i].Value);
                }
            }
            return true;
        }
        /// <summary>
        /// 退出提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //退出程序提示
        private void FormClosing01(object sender, FormClosingEventArgs e)
        {

            var dlgR = MessageBox.Show("确实退出?", "温馨提示", MessageBoxButtons.OKCancel);
            //取消退出
            if (dlgR == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }        
    }
}

    
