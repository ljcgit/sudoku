using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace shudu
{
    public partial class mission : Form
    {
        private int m_num;   //游戏种类下标
        private Button[] bts = new Button[8];
        public mission()
        {
            InitializeComponent();
            this.m_num = GameInfo.m;
        }
        private void mission_Load(object sender, EventArgs e)
        {
            SqlHelper sh = new SqlHelper();
            string introduction = sh.getIntroduction();
            textBox1.Text = introduction;
            String[] missions={"四宫格","六宫格","九宫格","自定义的九宫格"};
            label1.Text = missions[m_num];
            //将标签背景设置成透明
            label1.BackColor = Color.Transparent;
            //获取关卡按钮组
            for (int i = 0; i < 8; i++)
            {
                string name = "mission" + (i+1).ToString();
                bts[i] = getButton(name);    //获取相应按钮

            }
            int max = sh.getMaxPoint();
            //将未完成关卡后面的关都给关闭
            for (int i = max; i < 8; i++)
            {
                bts[i].Enabled = false;
            }
            int x = bts[max - 1].Location.X;
            int y = bts[max - 1].Location.Y;
            pictureBox1.Location = new Point(x, y-90);

        }
        //打开游戏主界面
        public void choose_misssion(object sender, EventArgs e)
        {
            Button bt=(Button)sender;
            GameInfo.p = int.Parse(bt.Text);
            GamePanel gb = new GamePanel();
            new SqlHelper().saveRecord();
            gb.Show(this);
            this.Hide();
        }
         /**
         * 获取对应的按钮组件
         */
        public Button getButton(string name)
        {
            return (Button)this.GetType().GetField(name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase).GetValue(this);
        }
        //关闭窗体事件
        private void mission_FormClosed(object sender, FormClosedEventArgs e)
        {
            new Form1().Show();
        }



    }
}
