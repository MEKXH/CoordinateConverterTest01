using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace PingChaText0
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //定义所需的矩阵
        int num = 36;
        Matrix X = new Matrix(7, 1);
        Matrix B = new Matrix(36, 7);
        Matrix V = new Matrix(36, 1);
        Matrix L = new Matrix(36, 1);
        Matrix Place = new Matrix(36, 1);
        Matrix CGCS2000 = new Matrix(36, 1);
        Matrix VTPV = new Matrix(2, 2);
        Matrix XYZCon = new Matrix(18, 1);
        Matrix XYZKnown = new Matrix(18, 1);
        Matrix B1 = new Matrix(18, 7);
        Matrix NBB_1 = new Matrix(7, 7);

        double[,] x = new double[7, 1];
        double[,] b = new double[36, 7];
        double[,] v = new double[36, 1];
        double[,] l = new double[36, 1];
        double[,] place = new double[36, 1];
        double[,] cgcs = new double[36, 1];
        double[,] xyzcon = new double[18, 1];
        double[,] xyzknown = new double[18, 1];
        double[,] b1 = new double[18, 7];
        double sigma;
        double[] nbb_1 = new double[7];


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
            var Ok02 = DataGridToTxt(dataGridView4, Sf01.FileName, ',');
            if (!Ok02)
            {
                MessageBox.Show("保存出错,请检查表格中数据是否为空或有误", "温馨提示");
            }
            else
                MessageBox.Show("       保存成功", "温馨提示");


        }
        //导出数据方法
        //strFileName为文件名，strSplit为数据间的分隔符
        /// <summary>
        /// 导出表格中数据
        /// </summary>
        /// <param name="gridview"></param>
        /// <param name="strFileName"></param>
        /// <param name="strSplit"></param>
        /// <returns></returns>
        private static bool DataGridToTxt(DataGridView gridview, string strFileName, char strSplit)
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

            //导入数据到矩阵Place,CGCS2000
            GetDataFromDGV1(dataGridView1, place, 12);
            GetDataFromDGV1(dataGridView2, cgcs, 12);
            //导入数据到矩阵B
            for (int i = 0; i < 12; i++)
            {
                //B[0,0],[1,1],[2,2] = 0
                b[3 * i, 0] = 1; b[3 * i + 1, 1] = 1; b[3 * i + 2, 2] = 1;
                //B矩阵中每三行循环赋值
                b[3 * i, 4] = -place[3 * i + 2, 0]; b[3 * i, 5] = place[3 * i + 1, 0]; b[3 * i, 6] = place[3 * i, 0];//Row0
                b[3 * i + 1, 3] = place[3 * i + 2, 0]; b[3 * i + 1, 5] = -place[3 * i, 0]; b[3 * i + 1, 6] = place[3 * i + 1, 0];//Row1
                b[3 * i + 2, 3] = -place[3 * i + 1, 0]; b[3 * i + 2, 4] = place[3 * i, 0]; b[3 * i + 2, 6] = place[3 * i + 2, 0];//Row2
            }
            //输出B矩阵
            //for (int i = 0; i < 36; i++)
            //{
            //    dataGridView5.Rows.Add(B.Detail[i, 0], B.Detail[i, 1], B.Detail[i, 2], B.Detail[i, 3], B.Detail[i, 4], B.Detail[i, 5], B.Detail[i, 6]);
            //}
            X.Detail = x; B.Detail = b; V.Detail = v; L.Detail = l;
            Place.Detail = place; CGCS2000.Detail = cgcs;
            //计算矩阵L
            L = MatrixOperations.MatrixSub(CGCS2000, Place);
            //计算NBB的逆
            NBB_1 = MatrixOperations.MatrixInvByCom(MatrixOperations.MatrixMulti(MatrixOperations.MatrixTrans(B), B));
            //显示NBB的逆
            //for (int i = 0; i < 7; i++)
            //{
            //    dataGridView5.Rows.Add(NBB_1.Detail[i, 0], NBB_1.Detail[i, 1], NBB_1.Detail[i, 2], NBB_1.Detail[i, 3], NBB_1.Detail[i, 4], NBB_1.Detail[i, 5], NBB_1.Detail[i, 6]);
            //}
            //显示中误差
            //计算转换7参数矩阵X
            X = MatrixOperations.MatrixMulti(NBB_1, MatrixOperations.MatrixMulti(MatrixOperations.MatrixTrans(B), L));
            //计算改正数
            V = MatrixOperations.MatrixSub(MatrixOperations.MatrixMulti(B, X), L);
            //计算验后单位权中误差并显示
            sigma = Math.Sqrt(Convert.ToDouble((MatrixOperations.MatrixMulti(MatrixOperations.MatrixTrans(V), V)).Detail[0, 0]) / (num - 7));
            textBox1.Text = (Convert.ToString(sigma));
            //导入计算得到的X矩阵(7参数)到DataGridView
            double[] x1 = new double[7];
            for (int i = 0; i < 7; i++)
            {
                x1[i] = X.Detail[i, 0];
            }
            dataGridView3.Rows.Add(x1[0], x1[1], x1[2], x1[3], x1[4], x1[5], x1[6]);
            //计算中误差
            double n = 0;
            for (int i = 0; i < 7; i++)
            {
                n =Math.Sqrt(NBB_1.Detail[i, i] )* sigma;
                nbb_1[i] = n; 
                
            }
            
            dataGridView3.Rows.Add(nbb_1[0], nbb_1[1], nbb_1[2], nbb_1[3], nbb_1[4], nbb_1[5], nbb_1[6]);
        }
        

    //转换坐标
    private void 计算转换坐标ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //导入数据到已知点矩阵
            GetDataFromDGV2(dataGridView1, xyzknown);
            //导入数据到矩阵Place,CGCS2000
            GetDataFromDGV1(dataGridView1, place, 12);
            GetDataFromDGV1(dataGridView2, cgcs, 12);
            //导入数据到矩阵B
            for (int i = 0; i < 12; i++)
            {
                //B[0,0],[1,1],[2,2] = 0
                b[3 * i, 0] = 1; b[3 * i + 1, 1] = 1; b[3 * i + 2, 2] = 1;
                //B矩阵中每三行循环赋值
                b[3 * i, 4] = -place[3 * i + 2, 0]; b[3 * i, 5] = place[3 * i + 1, 0]; b[3 * i, 6] = place[3 * i, 0];//Row0
                b[3 * i + 1, 3] = place[3 * i + 2, 0]; b[3 * i + 1, 5] = -place[3 * i, 0]; b[3 * i + 1, 6] = place[3 * i + 1, 0];//Row1
                b[3 * i + 2, 3] = -place[3 * i + 1, 0]; b[3 * i + 2, 4] = place[3 * i, 0]; b[3 * i + 2, 6] = place[3 * i + 2, 0];//Row2
            }
            //计算矩阵L
            L = MatrixOperations.MatrixSub(CGCS2000, Place);
            //计算NBB的逆
            NBB_1 = MatrixOperations.MatrixInvByCom(MatrixOperations.MatrixMulti(MatrixOperations.MatrixTrans(B), B));
            //计算转换7参数矩阵X
            X = MatrixOperations.MatrixMulti(NBB_1, MatrixOperations.MatrixMulti(MatrixOperations.MatrixTrans(B), L));
            //导入数据到B1矩阵
            for (int i = 0; i < 6; i++)
            {
                //B1[0,0],[1,1],[2,2] = 0
                b1[3 * i, 0] = 1; b1[3 * i + 1, 1] = 1; b1[3 * i + 2, 2] = 1;
                //B1矩阵中每三行循环赋值
                b1[3 * i, 4] = -xyzknown[3 * i + 2, 0]; b1[3 * i, 5] = xyzknown[3 * i + 1, 0]; b1[3 * i, 6] = xyzknown[3 * i, 0];//Row0
                b1[3 * i + 1, 3] = xyzknown[3 * i + 2, 0]; b1[3 * i + 1, 5] = -xyzknown[3 * i, 0]; b1[3 * i + 1, 6] = xyzknown[3 * i + 1, 0];//Row1
                b1[3 * i + 2, 3] = -xyzknown[3 * i + 1, 0]; b1[3 * i + 2, 4] = xyzknown[3 * i, 0]; b1[3 * i + 2, 6] = xyzknown[3 * i + 2, 0];//Row2
            }
            B1.Detail = b1;
            XYZCon.Detail = xyzcon;
            XYZKnown.Detail = xyzknown;
            //计算转换后坐标
            XYZCon = MatrixOperations.MatrixAdd(XYZKnown, MatrixOperations.MatrixMulti(B1, X));
            //导入计算得到的XYZCon矩阵到DataGridView            
            for (int i = 0; i < 6; i++)
            {
                dataGridView4.Rows.Add(i+13,Convert.ToDouble(XYZCon.Detail[3*i, 0]).ToString("0.000"), Convert.ToDouble(XYZCon.Detail[3*i+1, 0]).ToString("0.000"), Convert.ToDouble(XYZCon.Detail[3*i+2, 0]).ToString("0.000"));
            }
        }

        
        //从DataGridView获取数据1（T读取b行）
        /// <summary>
        /// 获取数据1
        /// </summary>
        /// <param name="DGV"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        private bool GetDataFromDGV1(DataGridView DGV, double[,] array, int b)
        {
            int a = 0;
            for (int i = 0; i < b ; i++)
            {
                for (int j = 1; j < DGV.ColumnCount; j++)
                {
                    array[a, 0] = Convert.ToDouble(DGV[j, i].Value);
                    a = a + 1;                    
                }
            }
            return true;
        }
        //从DataGridView获取数据方法2（X,Y,Z）T循环(后6列)
        /// <summary>
        /// 获取数据2
        /// </summary>
        /// <param name="DGV"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        private bool GetDataFromDGV2(DataGridView DGV, double[,] array)
        {
            int a = 0;
            for (int i = 12; i < 18; i++)
            {
                for (int j = 1; j < DGV.ColumnCount; j++)
                {
                    array[a, 0] = Convert.ToDouble(DGV[j, i].Value);
                    a = a + 1;
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

    
