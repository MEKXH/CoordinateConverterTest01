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

            
        }



        //从DataGridView中获取数据
        /// <summary>
        /// 从DataGridView中获取数据
        /// </summary>
        /// <param name="DGV"></param>
        /// <param name="errInfo"></param>
        /// <returns></returns>
        private bool GetDataFromDGV(DataGridView DGV, Matrix intoMatrix)
        {
            for (int i = 0; i < DGV.RowCount; i++)
            {
                for (int j = 0; j < DGV.ColumnCount; j++)
                {
                    //打印第i行第j列数据
                    MessageBox.Show(Convert.ToString(dataGridView1[j, i].Value));

                }
            }
            return true;
        }

        //退出程序提示
        private void FormClosing01(object sender, FormClosingEventArgs e)
        {

            var dlgR = MessageBox.Show("确实退出?", "温馨提示", MessageBoxButtons.OKCancel);
            //取消退出
            if (dlgR == DialogResult.Cancel)
            {
                e.Cancel = true;
            }//end if
        }
    }
}

    
