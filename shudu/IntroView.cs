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
    public partial class IntroView : Form
    {
        public IntroView()
        {
            InitializeComponent();
        }

        private void IntroView_Load(object sender, EventArgs e)
        {
            string intro = new SqlHelper().getIntroduction();
            label2.Text = intro;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new SqlHelper().deleteCBoard();
            GameView gv = new GameView();
            gv.Show();
            this.Hide();
        }
    }
}
