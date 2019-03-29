using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
namespace shudu
{

    class SqlHelper
    {
        private MySqlConnection myConnnect;
        public SqlHelper()
        {
            string constructorString = "server=localhost;User Id=root;password=;Database=shudu;Charset=utf8";
            myConnnect = new MySqlConnection(constructorString);
            try
            {
                myConnnect.Open();
            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);
            }
        }
        /*
         * 获取用户信息
         */
        public string getUserMessage()
        {
            string sql = "SELECT * from user where username LIKE'" + GameInfo.username+"'";
            MySqlCommand mycmd = new MySqlCommand(sql, myConnnect);
            MySqlDataReader result = mycmd.ExecuteReader();
            result.Read();
            return result.GetString("username") + "," + result.GetString("birthday") + "," + result.GetString("sex");
        }
        /**
         * 检查用户名和密码是否正确
         */
        public bool checkUser(string username,string password)
        {
            string sql = "SELECT count(*) from user where username LIKE'" + username + "' and password LIKE'" + password + "'";
            MySqlCommand mycmd = new MySqlCommand(sql, myConnnect);
            Object result = mycmd.ExecuteScalar();
            if (result != null&&int.Parse(result.ToString())>0)
                return true;
            else
                return false;
        }
        /**
         * 注册用户
         */
        public bool saveUser(string username, string password,string birthday,string sex)
        {
            string sql = "INSERT INTO user(username,password,birthday,sex) VALUES('" + username + "','" + password +"','"+birthday+ "','" + sex + "')";
            MySqlCommand mycmd = new MySqlCommand(sql, myConnnect);
            int result = mycmd.ExecuteNonQuery();
            if (result > 0)
                return true;
            else
                return false;
        }
        /*
         * 修改用户资料
         */
        public bool updateUser(string username,string password,string birthday,string sex)
        {
            string sql = "UPDATE user set username='"+username+"' where uid="+getUid(GameInfo.username);
            sql += ";UPDATE user set password='" + password + "' where uid=" + getUid(GameInfo.username);
            sql+=";UPDATE user set birthday='" + birthday + "' where uid=" + getUid(GameInfo.username);
            sql+=";UPDATE user set sex='"+sex+"' where uid="+getUid(GameInfo.username);
            MySqlCommand mycmd = new MySqlCommand(sql, myConnnect);
            int result = mycmd.ExecuteNonQuery();
            if (result > 0)
                return true;
            else
                return false;
        }
        /**
         * 将数独数据保存到数据库
         */
        public void saveMap(string map,int y)
        {
            int x = getUid(GameInfo.username);
            string sql = "INSERT INTO cboard(content,uid,checkpoint) VALUES ('" + map + "'," + x  + "," + y + ")";
            MySqlCommand mycmd = new MySqlCommand(sql, myConnnect);
            mycmd.ExecuteNonQuery();
        }
         /**
         * 读取九宫格内的数据
         */
        public string getCMap( int checkpoint)
        {
            string sql = "SELECT content from cboard where uid=" + getUid(GameInfo.username)+ " and checkpoint=" + checkpoint;
            MySqlCommand mycmd = new MySqlCommand(sql, myConnnect);
            string result = mycmd.ExecuteScalar().ToString();
            return result;

        }
        /**
         * 读取九宫格内的数据
         */
        public string getMap(int cid, int checkpoint)
        {
            string sql = "SELECT content from board where cid=" + cid + " and checkpoint=" + checkpoint;
            MySqlCommand mycmd = new MySqlCommand(sql, myConnnect);
            string result = mycmd.ExecuteScalar().ToString();
            return result;

        }
        /**
         * 查看使用道具情况
         */
        public int getProps()
        {
            string sql = "SELECT useprops from record where uid=" + getUid(GameInfo.username)+ " and bid=" + getBid(GameInfo.m,GameInfo.p);
            MySqlCommand mycmd = new MySqlCommand(sql, myConnnect);
            Object result = mycmd.ExecuteScalar();
            return int.Parse(result.ToString());
        }
        /**
         * 修改道具使用情况
         */
        public void changeProps()
        {
            string sql = "UPDATE record set useprops=useprops-1 where uid=" + getUid(GameInfo.username) + " and bid=" + getBid(GameInfo.m, GameInfo.p);
            MySqlCommand mycmd = new MySqlCommand(sql, myConnnect);
            int result = mycmd.ExecuteNonQuery();
  
        }
        /**
         * 获取指定用户名的uid
         */
        public int getUid(string username)
        {
            string sql = "SELECT uid from user where username='" + username + "'";
            MySqlCommand mycmd = new MySqlCommand(sql, myConnnect);
            return int.Parse(mycmd.ExecuteScalar().ToString());
        }
        /*
         * 获取bid
         */
        public int getBid(int cid, int checkpoint)
        {
            string sql = "SELECT bid from board where cid=" + cid + " and checkpoint=" + checkpoint;
            MySqlCommand mycmd = new MySqlCommand(sql, myConnnect);
            return int.Parse(mycmd.ExecuteScalar().ToString());
        }
        /*
         * 判断是否存在游戏记录
         */
        public bool isHaveRecord()
        {
            string sql = "SELECT count(*) from record where uid=" + getUid(GameInfo.username) + " and  bid=" + getBid(GameInfo.m, GameInfo.p);
            MySqlCommand mycmd = new MySqlCommand(sql, myConnnect);
            int  result =int.Parse(mycmd.ExecuteScalar().ToString());
            if (result != 0)    //已存在记录
                return true;
            else       //未存在记录
                return false;
        }
        /*
         * 保存记录
         */
        public void saveRecord()
        {
            if (!isHaveRecord())
            {
                string sql = "INSERT INTO record(uid,bid) values(" + getUid(GameInfo.username) + "," + getBid(GameInfo.m, GameInfo.p) + ")";
                MySqlCommand mycmd = new MySqlCommand(sql, myConnnect);
                int result = mycmd.ExecuteNonQuery();  //返回受影响的行数
            }
        }
        /*
         * 判断是否存在记录
         */
        public bool isHaveBoard(int p)
        {
            string sql = "SELECT * FROM board where cid=" + GameInfo.m + " and  checkpoint=" + p;
            MySqlCommand mycmd = new MySqlCommand(sql, myConnnect);
            MySqlDataReader result = mycmd.ExecuteReader();
            bool d = result.HasRows;
            return result.HasRows;
        }
        public bool isHaveCBoard(int p)
        {
            string sql = "SELECT * FROM cboard where uid=" + getUid(GameInfo.username) + " and  checkpoint=" + p;
            MySqlCommand mycmd = new MySqlCommand(sql, myConnnect);
            MySqlDataReader result = mycmd.ExecuteReader();
            bool d = result.HasRows;
            return result.HasRows;
        }
        /*
         * 获取最快时间记录
         */
        public int getfastest_record()
        {
            string sql = "SELECT fastest_record from record where  uid=" + getUid(GameInfo.username) + " and  bid=" + getBid(GameInfo.m, GameInfo.p);
            MySqlCommand mycmd = new MySqlCommand(sql, myConnnect);
            return int.Parse(mycmd.ExecuteScalar().ToString());
        }
        /*
         *保存最短时间
         */
        public void savefastest_record(int time)
        {
            int min = getfastest_record();
            if (min > time)
            {
                string sql = "UPDATE record set fastest_record=" + time + " where  uid=" + getUid(GameInfo.username) + " and  bid=" + getBid(GameInfo.m, GameInfo.p);
                MySqlCommand mycmd = new MySqlCommand(sql, myConnnect);
                int result = mycmd.ExecuteNonQuery();
            }
        }
        /*
         * 获得当前游戏种类的最大关卡
         */
        public int getMaxPoint()
        {
            string sql = "SELECT MAX(checkpoint)from record,board where record.bid=board.bid and uid=" + getUid(GameInfo.username) + " and cid=" + GameInfo.m;
            MySqlCommand mycmd = new MySqlCommand(sql, myConnnect);
            Object d = mycmd.ExecuteScalar();
            if (d.ToString() == "")
                return 1;
            else
            {
                int k = int.Parse(d.ToString());
                return k == 0 ? 1 : k;
            }
        }
        /*
         * 获取排名前五的姓名
         */
        public string[] getTop5()
        {
            string[] top=new string[5];
            string sql = "SELECT username,fastest_record from user,record where record.uid=user.uid and bid=" + getBid(GameInfo.m, GameInfo.p) + " order by fastest_record asc";
            MySqlCommand mycmd = new MySqlCommand(sql, myConnnect);
            MySqlDataReader reader = mycmd.ExecuteReader();
            int num = 0;
            while (reader.Read())
            {
                string data = reader.GetString("username") + "," + reader.GetInt32("fastest_record");
                top[num++] = data;
            }
            return top;
        }
        /*
         * 获取比赛排名前五的姓名
         */
        public string[] getCTop5()
        {
            string[] top = new string[5];
            string sql = "SELECT username,grades from user order by grades desc";
            MySqlCommand mycmd = new MySqlCommand(sql, myConnnect);
            MySqlDataReader reader = mycmd.ExecuteReader();
            int num = 0;
            while (reader.Read())
            {
                string data = reader.GetString("username") + "," + reader.GetInt32("grades");
                top[num++] = data;
            }
            return top;
        }
        /*
         * 保存日志记录
         */
        public void savelog()
        {
            string sql = "INSERT INTO logging(uid,NetBIOS,logtime) values(" + getUid(GameInfo.username) + ",'" + Environment.MachineName + "','" + DateTime.Now.ToLocalTime().ToString()+"')";
            MySqlCommand mycmd = new MySqlCommand(sql, myConnnect);
            int rows = mycmd.ExecuteNonQuery();
        }
        /*
         * 获取日志记录数
         */
        public int getLogCount()
        {
            string sql = "SELECT COUNT(*) FROM logging where uid=" + getUid(GameInfo.username);
            MySqlCommand mysql = new MySqlCommand(sql, myConnnect);
            Object result = mysql.ExecuteScalar();
            return  int.Parse(result.ToString());
        }
        /*
         * 获取日志
         */
        public List<string[]> getLog()
        {
            List<string[]> log = new List<string[]>();
            string sql = "SELECT * FROM  logging where uid=" + getUid(GameInfo.username);
            MySqlCommand mysql = new MySqlCommand(sql, myConnnect);
            MySqlDataReader  result=  mysql.ExecuteReader();
            while (result.Read())
            {
                string[] d = new string[3];
                d[0] = GameInfo.username;
                d[1] = result.GetString("NetBIOS");
                d[2] = result.GetString("logtime");
                log.Add(d);
            }
            return log;
        }
        /*
         * 判断是否存在比赛成绩
         */
        public int getGrades(int u)
        {
            string sql = "SELECT * FROM user where uid=" + u;
            MySqlCommand mysql = new MySqlCommand(sql, myConnnect);
            MySqlDataReader result = mysql.ExecuteReader();
            if (result.HasRows)
            {
                result.Read();
                int t = result.GetInt32("grades");
                result.Close();
                return t;
            }
            else
                return -1;
        }
        /*
         * 保存比赛成绩
         */
        public void saveGrades(int grades)
        {
            string sql="";
            int u = getUid(GameInfo.username);
            int k = getGrades(u);
            if(k < grades)
                sql = "UPDATE user set grades=" + grades + "  where  uid = +" + u;
            else
                return;
            MySqlCommand mysql = new MySqlCommand(sql, myConnnect);
            mysql.ExecuteNonQuery();
        }
        /*
         * 获取游戏介绍
         */
        public string getIntroduction()
        {
            string sql = "SELECT introduction from category where cid=" + GameInfo.m;
            MySqlCommand mysql = new MySqlCommand(sql, myConnnect);
            Object result = mysql.ExecuteScalar();
            return result.ToString();
        }
        /*
         * 删除上一次挑战赛游戏数据
         */
        public void deleteCBoard()
        {
            string sql = "DELETE FROM cboard where uid=" + getUid(GameInfo.username);
            MySqlCommand mysql = new MySqlCommand(sql, myConnnect);
            mysql.ExecuteNonQuery();
        }
        //析构函数，释放数据连接
        ~SqlHelper()
        {
            myConnnect.Close();
        }
    }
}
