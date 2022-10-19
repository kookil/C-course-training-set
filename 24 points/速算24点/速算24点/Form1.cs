using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 速算24点
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Timer time = new Timer();
        Stopwatch sw; //秒表对象
        TimeSpan ts;
        static int count = 1;

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();

            sw.Stop();
            time.Stop();
            label5.Text = string.Format("{0}:{1}:{2}:{3}", 0, 0, 0, 0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Random rd = new Random();
            int sj1 = rd.Next(1, 14);
            textBox2.AppendText(sj1.ToString());

            int sj2 = rd.Next(1, 14);
            textBox3.AppendText(sj2.ToString());

            int sj3 = rd.Next(1, 14);
            textBox4.AppendText(sj3.ToString());

            int sj4 = rd.Next(1, 14);
            textBox5.AppendText(sj4.ToString());

            button3.Enabled = true;
            sw = new Stopwatch();
            time.Tick += new EventHandler(timer1_Tick);  //时钟触发信号
            time.Interval = 1;
            sw.Start();
            time.Start();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ts = sw.Elapsed;
            label5.Text = string.Format("{0}:{1}:{2}:{3}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string s = "";
            bool try24;
            int a = Convert.ToInt32(textBox2.Text);
            int b = Convert.ToInt32(textBox3.Text);
            int c = Convert.ToInt32(textBox4.Text);
            int d = Convert.ToInt32(textBox5.Text);
            try24 = Try24(a, b, c, d, ref s);
            MessageBox.Show(s,"答案");
            sw.Stop();
            time.Stop();
            label5.Text = string.Format("{0}:{1}:{2}:{3}", 0, 0, 0, 0);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string s = textBox1.Text;
            //CalculateExpression exp = new CalculateExpression();
            string o = CalculateExpression.Calculate(s).ToString();
            int q = Convert.ToInt32(o);
            if (q == 24)
            {
                MessageBox.Show("正确，您的用时为"+ts.Hours+":"+ts.Minutes + ":"+ts.Seconds + ":"+ts.Milliseconds / 10, "答案");
            }
            else
            { 
                MessageBox.Show("错误", "答案");
            }
            sw.Stop();
            time.Stop();
            label5.Text = string.Format("{0}:{1}:{2}:{3}", 0, 0, 0, 0);
        }
        /// <summary>
        /// 判断abcd四个数进行任意四则运算后能不能得出24，每个数字只能用一次
        /// </summary>
        /// <param name="a">数字a</param>
        /// <param name="b">数字b</param>
        /// <param name="c">数字c</param>
        /// <param name="d">数字d</param>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static bool Try24(int a, int b, int c, int d, ref string expression)
        {
            //a字头
            if (TryEach(a, b, c, d, ref expression)) return true;
            if (TryEach(a, b, d, c, ref expression)) return true;
            if (TryEach(a, c, b, d, ref expression)) return true;
            if (TryEach(a, c, d, b, ref expression)) return true;
            if (TryEach(a, d, b, c, ref expression)) return true;
            if (TryEach(a, d, c, b, ref expression)) return true;
            //b字头
            if (TryEach(b, a, c, d, ref expression)) return true;
            if (TryEach(b, a, d, c, ref expression)) return true;
            if (TryEach(b, c, a, d, ref expression)) return true;
            if (TryEach(b, c, d, a, ref expression)) return true;
            if (TryEach(b, d, a, c, ref expression)) return true;
            if (TryEach(b, d, c, a, ref expression)) return true;
            //c字头
            if (TryEach(c, a, b, d, ref expression)) return true;
            if (TryEach(c, a, d, b, ref expression)) return true;
            if (TryEach(c, b, a, c, ref expression)) return true;
            if (TryEach(c, b, c, a, ref expression)) return true;
            if (TryEach(c, d, a, b, ref expression)) return true;
            if (TryEach(c, d, b, a, ref expression)) return true;
            //d字头
            if (TryEach(d, a, b, c, ref expression)) return true;
            if (TryEach(d, a, c, b, ref expression)) return true;
            if (TryEach(d, b, a, c, ref expression)) return true;
            if (TryEach(d, b, c, a, ref expression)) return true;
            if (TryEach(d, c, a, b, ref expression)) return true;
            if (TryEach(d, c, b, a, ref expression)) return true;
            return false;
        }
        /// <summary>
        /// 判断指定顺序的四个数abcd进行任意四则运算后能不能得出24，每个数字只能用一次
        /// </summary>
        /// <param name="a">数字1</param>
        /// <param name="b">数字2</param>
        /// <param name="c">数字3</param>
        /// <param name="d">数字4</param>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static bool TryEach(int a, int b, int c, int d, ref string expression)
        {
            expression = "";
            //两个数可以做6种运算：加、减、被减、乘以、除以、除
            //四个数共可以进行6*6*6=216种不同次序的四则运算
            //初始化数组
            for (int i = 0; i < 6 * 6 * 6; i++)
            {
                //a与b间的运算符：i / 36
                //b与c间的运算符：i % 36 / 6
                //c与d间的运算符：i % 6
                //1.运算顺序：a和b，再和c，再和d
                {
                    string expression1 = "", expression2 = "";
                    int temp1 = ResultOf(a, b, i / 36, ref expression1);
                    int temp2 = ResultOf(temp1, c, i % 36 / 6, ref expression2, expression1);
                    int result = ResultOf(temp2, d, i % 6, ref expression, expression2);
                    if (result == 24) return true;
                }
                //2.运算顺序：a和b，c和d，前面部分和后面部分
                {
                    string expression1 = "", expression2 = "";
                    int temp1 = ResultOf(a, b, i / 36, ref expression1);
                    int temp2 = ResultOf(c, d, i % 6, ref expression2);
                    int result = ResultOf(temp1, temp2, i % 36 / 6,
                     ref expression, expression1, expression2);
                    if (result == 24) return true;
                }
                //3.运算顺序：b和c运算，再与a运算，再与d运算
                {
                    string expression1 = "", expression2 = "";
                    int temp1 = ResultOf(b, c, i % 36 / 6, ref expression1);
                    int temp2 = ResultOf(a, temp1, i / 36, ref expression2, "", expression1);
                    int result = ResultOf(temp2, d, i % 6, ref expression, expression2);
                    if (result == 24) return true;
                }
                //4.运算顺序：b和c运算，再与d运算，再与a运算
                {
                    string expression1 = "", expression2 = "";
                    int temp1 = ResultOf(b, c, i % 36 / 6, ref expression1);
                    int temp2 = ResultOf(temp1, d, i % 6, ref expression2, expression1);
                    int result = ResultOf(a, temp2, i / 36, ref expression, "", expression2);
                    if (result == 24) return true;
                }
                //5.运算顺序：c和d运算，再和b运算，再和a运算
                {
                    string expression1 = "", expression2 = "";
                    int temp1 = ResultOf(c, d, i % 6, ref expression1);
                    int temp2 = ResultOf(b, temp1, i % 36 / 6, ref expression2, "", expression1);
                    int result = ResultOf(a, temp2, i / 36, ref expression, "", expression2);
                    if (result == 24) return true;
                }
            }
            expression = "Abandoned";
            return false;
        }
        /// <summary>
        /// 求两数进行某一四则运算后的结果
        /// </summary>
        /// <param name="x">数字1</param>
        /// <param name="y">数字2</param>
        /// <param name="method">（0-5分别代表：加、减、被减、乘以、除以、除）</param>
        /// <param name="expression">返回的表达式</param>
        /// <param name="expressionLeft">数字1表达式</param>
        /// <param name="expressionRight">数字2表达式</param>
        /// <returns></returns>
        private static int ResultOf(int x, int y, int method,
         ref string expression, string expressionLeft = "", string expressionRight = "")
        {
            //左右表达式之前被判定为无效则不计算，除数为0时不计算
            if (expressionLeft == "Abandoned" || expressionRight == "Abandoned" ||
             (x == 0 && method == 5) || (y == 0 && method == 4))
            {
                expression = "Abandoned";
                return -1;
            }
            int result = 0;
            switch (method)
            {
                case 0:
                    {
                        //加
                        result = x + y;
                        expression = string.Format("{0}+{1}",
                         expressionLeft == "" ? x.ToString() : expressionLeft,
                         expressionRight == "" ? y.ToString() : expressionRight);
                    }
                    break;
                case 1:
                    {
                        //减
                        result = x - y;
                        expression = string.Format("{0}-{1}",
                         expressionLeft == "" ? x.ToString() : expressionLeft,
                         expressionRight == "" ? y.ToString() : expressionRight);
                    }
                    break;
                case 2:
                    {
                        //被减
                        result = y - x;
                        expression = string.Format("{1}-{0}",
                         expressionLeft == "" ? x.ToString() : expressionLeft,
                         expressionRight == "" ? y.ToString() : expressionRight);
                    }
                    break;
                case 3:
                    {
                        //乘以
                        result = x * y;
                        expression = string.Format("({0})*({1})",
                         expressionLeft == "" ? x.ToString() : expressionLeft,
                         expressionRight == "" ? y.ToString() : expressionRight);
                    }
                    break;
                case 4:
                    {
                        //除以
                        if (x % y == 0)
                        {
                            result = x / y;
                            expression = string.Format("({0})/({1})",
                             expressionLeft == "" ? x.ToString() : expressionLeft,
                             expressionRight == "" ? y.ToString() : expressionRight);
                        }
                        else
                        {
                            expression = "Abandoned";
                        }
                    }
                    break;
                case 5:
                    {
                        //除
                        if (y % x == 0)
                        {
                            result = y / x;
                            expression = string.Format("({1})/({0})",
                             expressionLeft == "" ? x.ToString() : expressionLeft,
                             expressionRight == "" ? y.ToString() : expressionRight);
                        }
                        else
                        {
                            expression = "Abandoned";
                        }
                    }
                    break;
            }
            //运算不合法，则返回-1，表达式为Abandoned，
            if (expression == "Abandoned")
            {
                return -1;
            }
            return result;
        }
        /// <summary>
        /// 用于字符串公式得出数值如string s="2+2" 返回object o =4;
        /// </summary>
        public class CalculateExpression
        {
            /// <summary>
            /// 接受一个string类型的表达式并计算结果,返回一个object对象,静态方法
            /// </summary>
            public static object Calculate(string expression)
            {
                try
                {
                    string className = "Calc";
                    string methodName = "Run";
                    expression = expression.Replace("/", "*1.0/");

                    // 创建编译器实例。
                    CodeDomProvider complier = (new Microsoft.CSharp.CSharpCodeProvider());
                    // 设置编译参数。
                    CompilerParameters paras = new CompilerParameters();
                    paras.GenerateExecutable = false;
                    paras.GenerateInMemory = true;

                    // 创建动态代码。
                    StringBuilder classSource = new StringBuilder();
                    classSource.Append("public class " + className + "\n");
                    classSource.Append("{\n");
                    classSource.Append(" public object " + methodName + "()\n");
                    classSource.Append(" {\n");
                    classSource.Append(" return " + expression + ";\n");
                    classSource.Append(" }\n");
                    classSource.Append("}");

                    // 编译代码。
                    CompilerResults result = complier.CompileAssemblyFromSource(paras, classSource.ToString());

                    // 获取编译后的程序集。
                    Assembly assembly = result.CompiledAssembly;

                    // 动态调用方法。
                    object eval = assembly.CreateInstance(className);
                    MethodInfo method = eval.GetType().GetMethod(methodName);
                    object reobj = method.Invoke(eval, null);
                    GC.Collect();
                    return reobj;
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                    return null;
                }
            }
        }

        class ClassScale
        {
            //X、Y是保存初始窗体的 Width Height
            public float X;
            public float Y;
            //flag是初始化X、Y的标识
            public static bool flag = true;
            public Size size { get; set; }
            public Control cons { get; set; }
            public void InitXY(Size s)
            {
                X = s.Width;
                Y = s.Height;
            }
            //setTag方法是将窗体上所有子控件的宽度、高度、左边距、顶部距离及字体大小暂存起来
            //con.Width   con.Height   con.Left   con.Top  con.Font.Size
            public void setTag(Control cons)
            {
                foreach (Control con in cons.Controls)
                {
                    con.Tag = con.Width + ":" + con.Height + ":" + con.Left + ":" + con.Top + ":" + con.Font.Size;
                    if (con.Controls.Count > 0)
                        setTag(con);
                }
            }
            //通过缩放比例，改变窗体上所有子控件的宽度、高度、左边距、顶部距离及字体大小的信息
            public void setControls(float newx, float newy, Control cons)
            {
                foreach (Control con in cons.Controls)
                {
                    string[] mytag = con.Tag.ToString().Split(new char[] { ':' });
                    float a = Convert.ToSingle(mytag[0]) * newx;
                    con.Width = (int)a;
                    a = Convert.ToSingle(mytag[1]) * newy;
                    con.Height = (int)(a);
                    a = Convert.ToSingle(mytag[2]) * newx;
                    con.Left = (int)(a);
                    a = Convert.ToSingle(mytag[3]) * newy;
                    con.Top = (int)(a);
                    Single currentSize = Convert.ToSingle(mytag[4]) * Math.Min(newx, newy);
                    con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                    if (con.Controls.Count > 0)
                    {
                        setControls(newx, newy, con);
                    }
                }
            }
            //计算缩放的比例
            public void Scale_Resize(object sender, EventArgs e)
            {
                float newx = size.Width / X;
                float newy = size.Height / Y;
                setControls(newx, newy, cons);
            }
            //初始化X、Y
            //调用setTag() Scale_Resize()方法 
            public void Scale_Change()
            {
                if (flag) InitXY(size);
                flag = false;
                setTag(cons);
                Scale_Resize(new object(), new EventArgs());
            }
        }
        ClassScale c; //全局变量 c
                      //窗体的Load事件中添加
        private void Form1_Load(object sender, EventArgs e)
        {
            //获取当前窗体的size 和所有控件
            c = new ClassScale() { size = this.Size, cons = this.Controls.Owner };
            this.Resize += new EventHandler(c.Scale_Resize);  //调整控件大小
            c.Scale_Change();
        }
        //当窗体大小发生变化时，更新c的size
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            c.size = this.Size;
        }
    }
}
