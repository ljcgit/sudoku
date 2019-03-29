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
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            new SqlHelper().savelog();
        }
        public void direct(object sender,System.EventArgs e)
        {
            Button btn=(Button)sender;
            String[] missions={"四宫格","六宫格","九宫格","挑战赛"};
            int[] gnum = { 4, 6, 9, 9 };
            int k;
            for (k = 0; k < 4; k++)
            {
                if(btn.Text.CompareTo(missions[k])==0)
                {
                    break;
                }
            }
            GameInfo.m = k;
            GameInfo.gnum = gnum[k];
            if (gnum[k] == 4)
            {
                GameInfo.i = GameInfo.j = 2;
            }
            else if (gnum[k] == 6)
            {
                GameInfo.i = 2;
                GameInfo.j = 3;
            }
            else
            {
                GameInfo.i = GameInfo.j = 3;
            }
            if (GameInfo.m == 3)
            {
                IntroView t = new IntroView();
                t.Show();
                this.Hide();
            }
            else
            {
                mission mis = new mission();
                mis.Show();
                this.Hide();
            }
        }
        /**
         * 窗口关闭返回主窗体
         */
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            LoginView lv = new LoginView();
           // LoginView lv = (LoginView)this.Owner;
            lv.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            LogView lv = new LogView();
            lv.ShowDialog();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            MyView mv = new MyView();
            mv.Show();
            this.Hide();
        }

    }
}
