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
    public partial class ChooseView : Form
    {   
        private string bname;
        public ChooseView(string bname,Point t)
        {
            InitializeComponent();
            this.bname = bname;
            if (GameInfo.m == 3)
            {
                this.Left = t.X + 36;
                this.Top = t.Y + 36;
            }
            else
            {
                this.Left = t.X + 36 * (3 - GameInfo.m);
                this.Top = t.Y + 36 * (3 - GameInfo.m);
            }
        }

        private void ChooseView_Load(object sender, EventArgs e)
        {

        }
        private void ack(object sender, EventArgs e)
        {
            //获取父窗体指定按钮的数字
            int i;
            if (bname.Length > 6)
                i = int.Parse(bname.Remove(0, 6));
            else
                i = int.Parse(bname);
            Button bt = (Button)sender;
            //获取父窗口对象
            if (GameInfo.m == 3)
            {
                GameView gk = (GameView)this.Owner;
                gk.getButton(i).Text = bt.Text;
            }
            else
            {
                GamePanel gv = (GamePanel)this.Owner;
                //将选择的数填入父窗口中
                gv.getButton(i).Text = bt.Text;
            }
            this.Close();
        }
    }
}
