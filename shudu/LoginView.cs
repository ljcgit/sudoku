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
    public partial class LoginView : Form
    {
        public LoginView()
        {
            InitializeComponent();
        }
        /**
         * 登录
         */
        private void submit_Click(object sender, EventArgs e)
        {
            string uname = username.Text;
            string pword = password.Text;
            SqlHelper sh = new SqlHelper();
            if (sh.checkUser(uname, pword))
            {
                GameInfo.username = uname;
                new Form1().Show(this);
                this.Hide();   //隐藏本窗口
            }
            else
                MessageBox.Show("登录失败！", "提示信息", MessageBoxButtons.OK);
        }
        /**
         * 切换提交按钮事件
         */
        private void label5_Click(object sender, EventArgs e)
        {
            SignView sv = new SignView();
            sv.Show();
            this.Hide();
        }

        private void LoginView_Load(object sender, EventArgs e)
        {
           
        }

        private void LoginView_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(0); 
        }


    }
}
