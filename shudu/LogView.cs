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
    public partial class LogView : Form
    {
        public LogView()
        {
            InitializeComponent();
        }

        private void LogView_Load(object sender, EventArgs e)
        {
            List<string []> logs=new SqlHelper().getLog();
            int i = 0;
            foreach (string[] t in logs)
            {
                PictureBox pb = new PictureBox();   //记录最前面的图片
                pb.BackgroundImage = Image.FromFile("E:\\SHUDU\\shudu\\shudu\\icon\\日志.png");
                pb.BackgroundImageLayout=System.Windows.Forms.ImageLayout.Stretch;
                pb.BackColor = Color.Transparent;
                pb.Width = 60;
                pb.Height = 60;
                pb.Location = new Point(10, 37 + 70 * i);
                Label label = new Label();    //记录信息
                label.BackColor = Color.Transparent;
                label.Width = 500;
                string s = t[0] + " " + t[1] + "  " + t[2];
                label.Text = s;
                label.Font = new Font("宋体", 16);
                label.Location = new Point(88,50+70*i++);
                this.Controls.Add(label);
                this.Controls.Add(pb);
            }
        }
    }
}
