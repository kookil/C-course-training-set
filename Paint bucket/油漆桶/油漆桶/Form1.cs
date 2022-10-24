using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 油漆桶
{
    public partial class Form1 : Form
    {

        char SPLIT_SEPARATOR = ' ';
        public Form1()
        {
            InitializeComponent();
            this.SizeChanged += new Resize(this).Form1_Resize;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int line_max_count = 0;//计算这个二维表的列最多去到多少
            List<List<String>> T = new List<List<String>>();//用于存二维表的容器，就是一个存List的List
            OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();//一个打开文件的对话框    
            openFileDialog1.Filter = "文本文件(*.csv)|*.csv"; 
            if (openFileDialog1.ShowDialog() == DialogResult.OK)//判断是否选择了文件      
            {
                StreamReader streamReader = new StreamReader(openFileDialog1.FileName, Encoding.Default);//读取文件的流
                while (!streamReader.EndOfStream)//如果没读到最后
                {
                    List<String> line = new List<String>(streamReader.ReadLine().Split(SPLIT_SEPARATOR));//每一行根据分隔符形成数组，同时形成List
                    T.Add(line);//T增加这一行
                    if (line_max_count < line.Count)//此乃求列的最大值算法
                    {
                        line_max_count = line.Count;
                    }
                }
                streamReader.Close();
            }
            //设置listview的表头  
            for (int i = 0; i < line_max_count; i++)
            {
                listView1.Columns.Add("列" + (i + 1), listView1.Width / line_max_count - 1, HorizontalAlignment.Left);
            }
            listView1.BeginUpdate();//数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度  
            for (int i = 0; i < T.Count; i++)
            {
                List<String> line = T[i];
                ListViewItem listViewItem = new ListViewItem();
                for (int j = 0; j < line.Count; j++)
                {
                    if (j == 0)
                    {
                        listViewItem.Text = line[j];//listview的每一行的第一项  
                    }
                    else
                    {
                        listViewItem.SubItems.Add(line[j]);//其余子项
                    }
                }
                listView1.Items.Add(listViewItem);//将这行添加到listview中
                //Console.WriteLine(listViewItem);
            }
            listView1.EndUpdate();//结束数据处理，UI界面一次性绘制。  
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        class Resize
        {
            private Form _form;

            public Resize(Form form)
            {
                int count = form.Controls.Count * 2 + 2;
                float[] factor = new float[count];
                int i = 0;
                factor[i++] = form.Size.Width;
                factor[i++] = form.Size.Height;
                foreach (Control ctrl in form.Controls)
                {
                    factor[i++] = ctrl.Location.X / (float)form.Size.Width;
                    factor[i++] = ctrl.Location.Y / (float)form.Size.Height;
                    ctrl.Tag = ctrl.Size;
                }
                form.Tag = factor;
                this._form = form;
            }

            public void Form1_Resize(object sender, EventArgs e)
            {
                float[] scale = (float[])this._form.Tag;
                int i = 2;
                foreach (Control ctrl in this._form.Controls)
                {
                    ctrl.Left = (int)(this._form.Size.Width * scale[i++]);
                    ctrl.Top = (int)(this._form.Size.Height * scale[i++]);
                    ctrl.Width = (int)(this._form.Size.Width / (float)scale[0] * ((Size)ctrl.Tag).Width);
                    ctrl.Height = (int)(this._form.Size.Height / (float)scale[1] * ((Size)ctrl.Tag).Height);
                }
            }
        }
       
        private void button2_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = new Bitmap(this.Height, this.Width); 
            Graphics g = Graphics.FromImage(bitmap);
            g.DrawString("GDI+ 技术", new Font("宋体", 12), new SolidBrush(Color.Red), this.Width / 3, 0);

            g.DrawLine(new Pen(Color.Yellow, 12), 0, 40, this.Width, 40);

            g.DrawLine(new Pen(Color.Red, 0.1F), 0, 80, this.Width, 80);
            this.BackgroundImage = bitmap;
            g.Dispose();//*/

            //数据初始化   
            int[] X = new int[5] { 1, 2, 3, 4, 5 };
            int[] Y = new int[5] {1, 2, 3, 4, 5};
            //画图初始化   
            Bitmap bMap = new Bitmap(500, 500);
            Graphics gph = Graphics.FromImage(bMap);
            gph.Clear(Color.White);

            PointF cPt = new PointF(40, 420);//中心点(坐标轴原点)   
            PointF[] xPt = new PointF[3]{
             new   PointF(cPt.Y+5,cPt.Y),
             new   PointF(cPt.Y,cPt.Y-5),
             new   PointF(cPt.Y,cPt.Y+5)};//(X轴右边三角形的三个点坐标）   
            PointF[] yPt = new PointF[3]{
             new   PointF(cPt.X,cPt.X-5),
             new   PointF(cPt.X+5,cPt.X),
             new   PointF(cPt.X-5,cPt.X)};//Y轴三角形   
            gph.DrawString("作图", new Font("宋体", 14),
             Brushes.Black, new PointF(cPt.X + 12, cPt.X));//图表标题   
            //画X轴   
            gph.DrawLine(Pens.Black, cPt.X, cPt.Y, cPt.Y, cPt.Y);//画x轴
            gph.DrawPolygon(Pens.Black, xPt);//画X轴箭头三角形
            gph.FillPolygon(new SolidBrush(Color.Black), xPt);//X轴箭头填充黑色
            gph.DrawString("x轴", new Font("宋体", 12),
             Brushes.Black, new PointF(cPt.Y + 12, cPt.Y - 10));
            //画Y轴   
            gph.DrawLine(Pens.Black, cPt.X, cPt.Y, cPt.X, cPt.X);
            gph.DrawPolygon(Pens.Black, yPt);
            gph.FillPolygon(new SolidBrush(Color.Black), yPt);
            gph.DrawString("y轴", new Font("宋体", 12), Brushes.Black, new PointF(6, 7));


            }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }


    }
}

