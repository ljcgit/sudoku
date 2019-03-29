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
    public partial class MyView : Form
    {
        public MyView()
        {
            InitializeComponent();
        }

        private void MyView_Load(object sender, EventArgs e)
        {
            string mes = new SqlHelper().getUserMessage();
            string[] data = mes.Split(',');
            username.Text = data[0].ToString();
            birthday.Text = data[1].Substring(0,10);
            if(data[2]=="男")
            {
                sex.BackgroundImage=new Bitmap("E:\\SHUDU\\shudu\\shudu\\icon\\man.png");
            }
            else
            {
                sex.BackgroundImage=new Bitmap("E:\\SHUDU\\shudu\\shudu\\icon\\woman.png");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SignView sv = new SignView(1);
            sv.Show();
            this.Hide();
        }

        private void MyView_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 f = new Form1();
            f.Show();
        }

    }
}
