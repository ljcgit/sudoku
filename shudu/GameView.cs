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
    /*
     * 挑战赛比赛规则每道题根据难度有对应的分值，每道题所填数字与答案完全一致，则得到该题全部分值。
     * 题目未完成或所填数字与答案不一致，该题目不得分。 
     * 若在规定时限内提前完成所有的题目，并且全对，每提前半分钟加5分。 
     */
    public partial class GameView : Form
    {
        private int starttime=60*10;   //游戏开始时间
        private Button[,] bts = new Button[9, 9];    //按钮组
        public static int[,] maps = new int[9, 9];   //数独
        public int pass = 1;                         //当前关卡
        public Random random = new Random();         //随机数
        public List<int[,]> untiemaps = new List<int[,]>();   //保存数独的所有解
        private int sum = 0;    //游戏得分
        private int props = 999;
        public GameView()
        {
            GameInfo.p = pass;
            int i ;
            int j ;
            InitializeComponent();
            for (int k = 1; k <=81; k++)
            {
                i = (k-1) / 9;
                j = (k - 1) % 9;
                bts[i, j] = getButton(k);
            }
            CreateMap map = new CreateMap();
            maps = map.getmap();
            for (int m = 0; m < 9; m++)
            {
                for (int n = 0; n < 9; n++)
                {
                    if (passRole())
                    {
                        int z = maps[m, n];
                        bts[m, n].Text = z.ToString();
                        bts[m, n].BackColor = getColor(z);
                        bts[m, n].Enabled = false;
                    }
                    else
                    {
                        bts[m, n].Text = "";
                        bts[m, n].BackColor = Color.FromArgb(255, 128, 0);
                        bts[m, n].ForeColor = Color.Red;
                        bts[m, n].Font = new Font("宋体",13);
                    }
                }
            }
            if (!new SqlHelper().isHaveBoard(GameInfo.p))
                create_mapstring();   //保存记录
            unlock();
            checkFinish();
        }
        /**
         * 判断数独是否完成
         */
        private bool checkmap(int[,] m)
        {
            for (int i = 0; i < GameInfo.gnum; i++)
            {
                for (int j = 0; j < GameInfo.gnum; j++)
                {
                    if (bts[i, j].Text.Length != 1)
                        return false;
                    else
                    {
                        int dt = int.Parse(bts[i, j].Text);
                        if (dt != m[i, j])
                        {
                            return false; ;
                        }
                    }
                }
            }
            return true;
        }
        private void checkFinish()
        {
            bool flag = false;
            foreach (int[,] m in untiemaps)
            {
                if (checkmap(m))
                    flag = true;
            }
            if (flag)
            {
                sum = sum + 100;
                score.Text = sum.ToString();
                if (pass < 8)
                {
                    MessageBox.Show("恭喜你!过关了!马上进入下一关！", "消息提示", MessageBoxButtons.OK);
                    forward_Click(new object(),new EventArgs());
                }
                else
                {
                    timer1.Enabled = false;    //暂停计时
                    int etime = starttime / 30;   //计算剩余时间
                    sum = sum + etime * 5;
                    score.Text = sum.ToString();
                    new SqlHelper().saveGrades(sum);
                    showTop();
                    MessageBox.Show("恭喜你！完成比赛！", "消息提示", MessageBoxButtons.OK);
                }
            }
        }
        /**
         * 重新加载数独
         */
        private void reload()
        {
            CreateMap map = new CreateMap();
            maps = map.getmap();
            for (int m = 0; m < 9; m++)
            {
                for (int n = 0; n < 9; n++)
                {
                    if (passRole())
                    {
                        int z = maps[m, n];
                        bts[m, n].Text = z.ToString();
                        bts[m, n].BackColor = getColor(z);
                        bts[m, n].Enabled = false;
                    }
                    else
                    {
                        bts[m, n].Text = "";
                        bts[m, n].BackColor = Color.FromArgb(255, 128, 0);
                        bts[m, n].ForeColor = Color.Red;
                        bts[m, n].Font = new Font("宋体", 13);
                        bts[m, n].Enabled = true;   //必须,避免第一次设置按钮时设置为false
                    }
                }
                
            }
            pass += 1;
            GameInfo.p = pass;
            label3.Text = pass.ToString();
            if (!new SqlHelper().isHaveBoard(GameInfo.p))
                create_mapstring();   //保存记录
            unlock();
            checkFinish();
        }
        /**
         * 根据保存到数据库中数据加载
         */
        private void preload()
        {
            for (int m = 0; m < 9; m++)
            {
                for (int n = 0; n < 9; n++)
                {
                    int z = maps[m, n];
                    if (z!=-1)
                    {
                        bts[m, n].Text = z.ToString();
                        bts[m, n].BackColor = getColor(z);
                        bts[m, n].Enabled = false;
                    }
                    else
                    {
                        bts[m, n].Text = "";
                        bts[m, n].BackColor = Color.FromArgb(255, 128, 0);
                        bts[m, n].ForeColor = Color.Red;
                        bts[m, n].Font = new Font("宋体", 13);
                        bts[m, n].Enabled = true;   //必须,避免第一次设置按钮时设置为false
                    }
                }

            }
            unlock();
            checkFinish();
        }
        /**
         * 检查对于坐标的数字是否正确
         */
        private bool check(int i, int j)
        {
            if (bts[i,j].Text=="")
            {    //未填入数字
                return false;
            }

            for (int k = 0; k < 9; k++)
            {
                
                if (bts[i, j].Text.CompareTo(bts[i, k].Text) == 0 && j != k)
                {   //检查行上是否有相同数字
                    return false;
                }
                if (bts[i,j].Text.CompareTo(bts[k,j].Text)==0 && i != k)
                {  //检查列上是否有相同数字
                    return false;
                }
                int ii = (i / 3) * 3 + k / 3;   //小宫格内的行
                int jj = (j / 3) * 3 + k % 3;   //小宫格内的列
                if (bts[i, j].Text.CompareTo(bts[ii, jj].Text) == 0 && !(i == ii && j == jj))
                {   //检查宫格内是否有相同数字
                    return false;
                }

            }
            return true;
        }
        /*
         * 初始化游戏界面
         */
        private void GameView_Load(object sender, EventArgs e)
        {
            systemtime.Text = DateTime.Now.ToString();
            timer1.Enabled = true;
            timer1.Interval = 1000;
            spenttime.Text = starttime.ToString();
            label3.Text = pass.ToString();
            score.Text = sum.ToString();
            showTop();
        }
        /*
         * 显示排名区
         */
        public void showTop()
        {
            string[] top = new SqlHelper().getCTop5();

            for (int i = 0; i < top.Length; i++)
            {
                if (top[i] != "" && top[i] != null)
                {
                    string[] t = top[i].Split(',');
                    string username = t[0];
                    string grades = t[1];
                    if (t[1] != "0")
                    {
                        //姓名
                        Label b1 = new Label();
                        b1.Width = 70;
                        b1.Text = username;
                        b1.Visible = true;
                        b1.BackColor = Color.Transparent;
                        b1.Font = new Font("宋体", 16);
                        b1.Location = new Point(26, 30 + i * 70);
                        //成绩
                        Label b2 = new Label();
                        b2.Text = grades;
                        b2.Font = new Font("宋体", 16);
                        b2.BackColor = Color.Transparent;
                        b2.Visible = true;
                        b2.Location = new Point(100, 30 + i * 70);
                        panel1.Controls.Add(b1);
                        panel1.Controls.Add(b2);
                    }
                }
                //若总名次人数小于3则第四第五名不显示
                else
                {
                    if (i >= 3)
                    {
                        string pname = "pictureBox" + (i + 1).ToString();
                        getPictureBox(pname).Visible = false;
                    }
                }
            }
        }
        /**
         * 获取对应的pictureBox对象
         */
        public PictureBox getPictureBox(string name)
        {
            return (PictureBox)this.GetType().GetField(name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase).GetValue(this);
        }
        /**
         * 鼠标右击事件，用来清空输入的内容
         */
        private void mouse_right(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                Button bt = (Button)sender;
                bt.Text = "";
            }
        }
        /**
         * 设置时钟事件
         */
        private void timer1_Tick(object sender, EventArgs e)
        {
            systemtime.Text = DateTime.Now.ToString();
            spenttime.Text = (starttime--).ToString()+"s";
            if (starttime == 0)
            {
                timer1.Enabled = false;
                new SqlHelper().saveGrades(sum);
                MessageBox.Show("时间到,游戏结束!", "消息提示", MessageBoxButtons.OK);
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        bts[i, j].Enabled = false;
                    }
                }
                back.Enabled = false;
                forward.Enabled = false;
            }
        }
        /**
         * 解数独
         */
        private void unlock()
        {
            int length = GameInfo.gnum;
            for (int i = 0; i < GameInfo.gnum; i++)
            {
                for (int j = 0; j < GameInfo.gnum; j++)
                {
                    if (maps[i, j] == -1)
                        maps[i, j] = 0;
                }
            }

            algorithm shudu = new algorithm(maps, length);

            //返回结果  
            List<int[,]> t = shudu.Computing(false);
            untiemaps = t;
        }
        /**
         * 获取对应的按钮组件
         */
        public Button getButton(int i)
        {
            string name = "button" + i.ToString();
            return (Button)this.GetType().GetField(name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase).GetValue(this);
        }
        /**
         * 显示对应的选择数字窗体
         */
        public void choosemath(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            Point p = new Point(0, 0); // 0,0 是左上角 
            p = bt.PointToScreen(p); // p.X, p.Y 是Button1左上角在屏幕上的坐标 
            ChooseView cv = new ChooseView(bt.Name, p);
            cv.ShowDialog(this);
            checkFinish();
        }
        /**
         * 用来随机确认该按钮是为该填入
         */
        public bool passRole()
        {            
            return random.Next(1, 1000) >(pass*50+150);
        }
        /**
         * 获取相应数字按钮的颜色
         */
        public Color getColor(int x)
        {
            Color color=Color.Pink;
            switch (x)
            {
                case 1: color = Color.FromArgb(255, 255, 204); break;
                case 2: color = Color.FromArgb(204, 255, 255); break;
                case 3: color = Color.FromArgb(255, 204, 204); break;
                case 4: color = Color.FromArgb(255, 204, 153); break;
                case 5: color = Color.FromArgb(204, 255, 153); break;
                case 6: color = Color.FromArgb(204, 204, 204); break;
                case 7: color = Color.FromArgb(255, 204, 204); break;
                case 8: color = Color.FromArgb(255, 255, 255); break;
                case 9: color = Color.FromArgb(153, 255, 153); break;
                default:break;
            }
            return color;
        }
        /*
         * 提示按钮事件
         */
        private void prompt_Click(object sender, EventArgs e)
        {
            if (props <= 0)
            {
                MessageBox.Show("道具已使用完！！！", "消息提示", MessageBoxButtons.OK);
            }
            else
            {
                props--;
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (bts[i, j].Text == "")
                        {
                            bts[i, j].Text = maps[i, j].ToString();
                            checkFinish();
                            return;
                        }
                    }
                }
            }
        }
        /*
         *生成并保存数独字符串
         */
        private void create_mapstring()
        {
            string cmap = "";
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (bts[i, j].Text == "")
                        cmap += "-1,";
                    else
                        cmap += bts[i, j].Text + ",";
                }
            }
            SqlHelper sh = new SqlHelper();
            sh.saveMap(cmap,GameInfo.p);
        }
        /*
         * 请进
         */
        private void forward_Click(object sender, EventArgs e)
        {
            if (!new SqlHelper().isHaveCBoard(GameInfo.p+1))
            {
                reload();
            }
            else
            {
                pass += 1;
                GameInfo.p = pass;
                label3.Text = pass.ToString();
                maps = new CreateMap().getmap( GameInfo.p);
                preload();
            }
            if (pass == 8)
            {
                forward.Enabled = false;
            }
            if (pass > 1)
                back.Enabled = true;
        }
        /*
         * 后退
         */
        private void back_Click(object sender, EventArgs e)
        {
            pass -=1;
            if (pass == 1)
                back.Enabled = false;
            if (pass < 8)
            {
                forward.Enabled = true;
            }
            GameInfo.p = pass;
            label3.Text = pass.ToString();
            maps = new CreateMap().getmap(GameInfo.p);
            preload();
        }
        //退出事件
        private void quit_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.Show();
            this.Close();
        }

        
    }
}
