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
    public partial class GamePanel : Form
    {
         private long starttime;   //游戏开始时间
         private Button[,] bts = new Button[GameInfo.gnum, GameInfo.gnum];    //按钮组
         public  int[,] maps = new int[GameInfo.gnum, GameInfo.gnum];   //数独
         public List<int[,]> untiemaps = new List<int[,]>();   //保存数独的所有解
        public int pass = 1;                         //当前关卡
        public Random random = new Random();         //随机数
        public GamePanel()
        {
            pass = GameInfo.p;     
            int i ;
            int j ;
            InitializeComponent();
            int width=40* 9 / GameInfo.gnum;
            int height=40 * 9 / GameInfo.gnum;
            int paddingx=0,paddingy=0;
            int q;
            if(GameInfo.gnum==4||GameInfo.gnum==6)
                q=2;
            else
                q=3;
            int[] w = { 0, 8, 16, 24 };    //宫格间隙
            for (int k = 1; k <= GameInfo.gnum * GameInfo.gnum; k++)
            {
                i = (k - 1) / GameInfo.gnum;   //按钮横坐标
                j = (k - 1) % GameInfo.gnum;   //按钮竖坐标
                bts[i, j] = new Button();
                bts[i, j].Text = "";
                bts[i, j].Margin = new Padding(0,0,0,0);   //按钮间距离
                bts[i, j].Name = "button" + k.ToString();  //按钮name属性
                if (i % q == 0)      
                    paddingx = w[i/q];
                if (j % (GameInfo.gnum / q) == 0)
                    paddingy = w[j / (GameInfo.gnum / q)];
                bts[i, j].Location = new Point(13 + j * height + paddingy, 36 + i * width + paddingx);   //按钮在窗体中的位置
                bts[i, j].Size = new Size(width, height);   //按钮大小
                bts[i, j].Font = new System.Drawing.Font("宋体",9*(3-GameInfo.m));
                bts[i, j].Click += new EventHandler(choosemath);   //点击事件
                bts[i, j].MouseDown += new MouseEventHandler(mouse_right);   //右击事件
                groupBox3.Controls.Add(bts[i, j]);  

            }
            CreateMap map = new CreateMap();
            maps = map.getmap(GameInfo.m, GameInfo.p);  //获取数独内容
                for (int m = 0; m < GameInfo.gnum; m++)
                {
                    for (int n = 0; n < GameInfo.gnum; n++)
                    {
                        if (maps[m, n] != -1)     //固定区
                        {
                            int z = maps[m, n];
                            bts[m, n].Text = z.ToString();
                            bts[m, n].BackColor = getColor(z);
                            bts[m, n].Enabled = false;
                        }
                        else                      //填入区
                        {
                            bts[m, n].Text = "";
                            bts[m, n].BackColor = Color.FromArgb(255, 128, 0);
                            bts[m, n].ForeColor = Color.Red;
                            bts[m, n].Font = new Font("宋体", 13 * (3 - GameInfo.m));
                        }
                    }
                }
                if (GameInfo.m == 1)
                    unlock1();
                else
                    unlock();
                showTop();   //显示排名区
            checkFinish();   //验证是否完成
        }
        /*
         * 六宫格解密
         */
        public void unlock1()
        {
            
            ShuduHelper sh = new ShuduHelper(maps);
            MessageBox.Show(sh.getCount().ToString());
            untiemaps = sh.getMap();  //获得结果集
            maps = untiemaps[0];      
        }
        /*
         * 显示排名区
         */
        public void showTop()
        {
            panel1.Controls.Clear();
            string[] top = new SqlHelper().getTop5();

            for (int i = 0; i < top.Length; i++)
            {
                if (top[i] != "" && top[i] != null)
                {
                    string[] t = top[i].Split(',');
                    string username = t[0];
                    string grades = t[1];
                    if (t[1] != "9999")
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
        /**
         * 判断数独是否完成
         */
        private void checkFinish()
        {
            bool flag=false;
            foreach (int[,] m in untiemaps)
            {
                if (checkmap(m))
                    flag = true;
            }
            if (flag)
            {
                int time = int.Parse(spenttime.Text.Substring(0, spenttime.Text.Length - 1));
                new SqlHelper().savefastest_record(time);
                if (pass == 8)
                {
                    MessageBox.Show("恭喜你!通关了！", "消息提示", MessageBoxButtons.OK);
                }
                else
                {

                    MessageBox.Show("恭喜你!过关了!马上进入下一关！", "消息提示", MessageBoxButtons.OK);
                    reload();
                }
            }
        }
        /**
         * 
         */
        private void reload()
        {
            GameInfo.p = GameInfo.p + 1;
            new SqlHelper().saveRecord();
            CreateMap map = new CreateMap();
            maps = map.getmap(GameInfo.m, GameInfo.p);
            for (int m = 0; m < GameInfo.gnum; m++)
            {
                for (int n = 0; n < GameInfo.gnum; n++)
                {
                    if (maps[m, n] != -1)
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
                        bts[m, n].Font = new Font("宋体", 13 * (3 - GameInfo.m));
                        bts[m, n].Enabled = true;
                    }
                }
                
            }
            starttime = DateTime.Now.Ticks;
            pass += 1;
            label3.Text = pass.ToString();
            showTop();   //显示排名区
            if (GameInfo.m == 1)
                unlock1();
            else
                unlock();
            checkFinish();   //验证是否完成
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

            for (int k = 0; k < GameInfo.gnum; k++)
            {
                
                if (bts[i, j].Text.CompareTo(bts[i, k].Text) == 0 && j != k)
                {   //检查行上是否有相同数字
                    return false;
                }
                if (bts[i,j].Text.CompareTo(bts[k,j].Text)==0 && i != k)
                {  //检查列上是否有相同数字
                    return false;
                }
                int ii = (i / GameInfo.i) * GameInfo.i + k / GameInfo.i;   //小宫格内的行
                int jj = (j / GameInfo.j) * GameInfo.j + k % GameInfo.j;   //小宫格内的列
                if (bts[i, j].Text.CompareTo(bts[ii, jj].Text) == 0 && !(i == ii && j == jj))
                {   //检查宫格内是否有相同数字
                    return false;
                }

            }
            return true;
        }
        private void GamePanel_Load(object sender, EventArgs e)
        {
            systemtime.Text = DateTime.Now.ToString();
            starttime = DateTime.Now.Ticks;
            timer1.Enabled = true;
            timer1.Interval = 1000;
            label3.Text = pass.ToString();
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
            long endtime = DateTime.Now.Ticks;
            long min = (endtime - starttime) / 10000000;
            spenttime.Text = min.ToString()+"s";
        }
        /**
         * 获取对应的按钮组件
         */
        public Button getButton(string name)
        {
           
            return (Button)this.GetType().GetField(name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase).GetValue(this);
        }
        /**
         * 获取对应的pictureBox对象
         */
        public PictureBox getPictureBox(string name)
        {
            return (PictureBox)this.GetType().GetField(name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase).GetValue(this);
        }
        public Button getButton(int k)
        {
            int i,j;
            i = (k - 1) / GameInfo.gnum;
            j = (k - 1) % GameInfo.gnum;
            return bts[i, j];
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
            checkFinish();   //检查是否完成
        }
        /**
         * 用来随机确认该按钮是为该填入
         */
        public bool passRole()
        {
            
            return random.Next(1, 1000) >(pass*50+100);
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
            maps = t[0];
        }
        /**
         * 提示按钮事件
         */
        private void prompt_Click(object sender, EventArgs e)
        {
            SqlHelper sh=new SqlHelper();
            int uid = sh.getUid(GameInfo.username);
            int count = sh.getProps();
            if(count>0)
            {
                for (int i = 0; i < GameInfo.gnum; i++)
                {
                    for (int j = 0; j < GameInfo.gnum; j++)
                    {
                        if (bts[i, j].Text == "")
                        {
                            bts[i, j].Text = maps[i, j].ToString();
                            checkFinish();
                            sh.changeProps();
                            count--;
                            MessageBox.Show("你还有" + count + "次查看答案的机会!", "提示信息", MessageBoxButtons.OK);
                            return;
                        }
                    }
                }
            }
            else
                MessageBox.Show("你三次机会已经用完了!", "提示信息", MessageBoxButtons.OK);
        }

        private void quit_Click(object sender, EventArgs e)
        {
            mission m = (mission)this.Owner;
            m.Show();
            this.Close();
        }

    }
}
