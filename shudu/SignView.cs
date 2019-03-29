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
    public partial class SignView : Form
    {
        private string sex = "";     //性别
        private int x;
        public SignView(int x=0)
        {
            this.x = x;
            InitializeComponent();
        }
        /**
         * 注册
         */
        private void Sign_Click(object sender, EventArgs e)
        {
            string uname = username.Text;
            if (uname.CompareTo("") == 0)
            {
                MessageBox.Show("用户名未输入！", "提示信息", MessageBoxButtons.OK);
                return;
            }
            string pword = password.Text;
            string apword = ackpassword.Text;
            if (pword.CompareTo("") == 0 || apword.CompareTo("") == 0)
            {
                MessageBox.Show("密码未输入！", "提示信息", MessageBoxButtons.OK);
                return;
            }
            else
            {
                if (pword.CompareTo(apword) != 0)
                {
                    MessageBox.Show("两次密码输入不一致，请重新输入！", "提示信息", MessageBoxButtons.OK);
                    return;
                }
            }
            if (sex == "")
            {
                MessageBox.Show("未选择性别！", "提示信息", MessageBoxButtons.OK);
            }
            SqlHelper sh = new SqlHelper();
            if (sh.checkUser(uname, "%%"))
            {
                MessageBox.Show("用户已存在！", "提示信息", MessageBoxButtons.OK);
                return;
            }
            if (sh.saveUser(uname, pword,birthday.Value.ToString(),sex))
            {
                GameInfo.username = uname;
                new Form1().Show(this);
                this.Hide();   //隐藏本窗口
            }
            else
                MessageBox.Show("注册失败！", "提示信息", MessageBoxButtons.OK);
        }
        private void Change_Click(object sender, EventArgs e)
        {
            string uname = username.Text;
            string oldpword = oldpassword.Text;
            string pword = password.Text;
            string apword = ackpassword.Text;
            if (pword.CompareTo("") == 0 || apword.CompareTo("") == 0)
            {
                MessageBox.Show("密码未输入！", "提示信息", MessageBoxButtons.OK);
                return;
            }
            else
            {
                if (pword.CompareTo(apword) != 0)
                {
                    MessageBox.Show("两次密码输入不一致，请重新输入！", "提示信息", MessageBoxButtons.OK);
                    return;
                }
            }
            SqlHelper sh = new SqlHelper();
            if (sh.checkUser(uname, "%%") && GameInfo.username != uname)
            {
                MessageBox.Show("用户已存在！", "提示信息", MessageBoxButtons.OK);
                return;
            }
            if (!sh.checkUser(uname,oldpword))
            {
                MessageBox.Show("旧密码错误！！", "提示信息", MessageBoxButtons.OK);
                return;
            }
            if (sh.updateUser(uname, pword, birthday.Value.ToString(), sex))
            {
                GameInfo.username = uname;
                new Form1().Show(this);
                this.Hide();   //隐藏本窗口
            }
            else
                MessageBox.Show("修改失败！", "提示信息", MessageBoxButtons.OK);
        }
        private void SignView_Load(object sender, EventArgs e)
        {
            if (x == 1)
            {
                label1.Text = "Change";
                string mes = new SqlHelper().getUserMessage();
                string[] data = mes.Split(',');
                username.Text = data[0].ToString();
                birthday.Value = DateTime.Parse(data[1]);
                sex = data[2];
                submit.Click -= new EventHandler(Sign_Click);
                submit.Click += new EventHandler(Change_Click);
                label4.Visible = false;
                label5.Visible = false;
                label6.Visible = false;
            }
            else
            {
                label10.Visible = false;
                oldpassword.Visible = false;
                panel1.Location = new Point(panel1.Location.X, panel1.Location.Y - 55);
                this.Height = this.Height - 50;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            pictureBox3.Visible = false;
            sex = "男";
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            pictureBox2.Visible = false;
            sex = "女";
        }

        private void label5_Click(object sender, EventArgs e)
        {
            LoginView lv = new LoginView();
            lv.Show();
            this.Hide();
        }

    }
}
