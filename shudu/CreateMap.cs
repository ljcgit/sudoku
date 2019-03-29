using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace shudu
{
    class CreateMap
    {
        private static int[,] shudu = new int[9, 9];
        private int[,] matrix = new int[3, 3];
        public Random random = new Random();
        private void Create()
        {
            int i;
            int j;
            for (i=0; i<3; ++i)
            {
                for (j=0; j<3; ++j)
                {
                    matrix[i,j]=i*3+j+1;
                }
            }

            /*随机初始化矩阵*/
            for (i=0; i<9; i++)
            {
                int temp=random.Next(0,9);
                j=matrix[i/3,i%3];
                matrix[i/3,i%3]=matrix[temp/3,temp%3];
                matrix[temp/3,temp%3]=j;
            }


            /* 利用置换的方法组合数独 */

            for (i=0; i<3; ++i)
            {
                for (j=0; j<3; ++j)
                {
                    shudu[i+3,j+3]=matrix[i,j];
                    if (0==i)
                    {
                        shudu[i+5,j+6]=shudu[i+3,j+3];
                        shudu[i+4,j]=shudu[i+3,j+3];
                    }
                    else if (1==i)
                    {
                        shudu[i+2,j+6]=shudu[i+3,j+3];
                        shudu[i+4,j]=shudu[i+3,j+3];
                    }
                    else
                    {
                        shudu[i+2,j+6]=shudu[i+3,j+3];
                        shudu[i+1,j]=shudu[i+3,j+3];
                    }

                }
            }


            for (j=0; j<3; ++j)
            {
                for (i=0; i<3; ++i)
                {
                    if (0==j)
                    {
                        shudu[i,j+4]=shudu[i+3,j+3];
                        shudu[i+6,j+5]=shudu[i+3,j+3];
                    }
                    else if (1==j)
                    {
                        shudu[i,j+4]=shudu[i+3,j+3];
                        shudu[i+6,j+2]=shudu[i+3,j+3];
                    }
                    else
                    {
                        shudu[i,j+1]=shudu[i+3,j+3];
                        shudu[i+6,j+2]=shudu[i+3,j+3];
                    }

                }
            }


            for (j=0; j<3; ++j)
            {
                for (i=0; i<3; ++i)
                {
                    if (0==j)
                    {
                        shudu[i,j+1]=shudu[i+3,j];
                        shudu[i+6,j+2]=shudu[i+3,j];
                    }
                    else if (1==j)
                    {
                        shudu[i,j+1]=shudu[i+3,j];
                        shudu[i+6,j-1]=shudu[i+3,j];
                    }
                    else
                    {
                        shudu[i,j-2]=shudu[i+3,j];
                        shudu[i+6,j-1]=shudu[i+3,j];
                    }

                }
            }


            for (j=0; j<3; ++j)
            {
                for (i=0; i<3; ++i)
                {
                    if (0==j)
                    {
                        shudu[i,j+7]=shudu[i+3,j+6];
                        shudu[i+6,j+8]=shudu[i+3,j+6];
                    }
                    else if (1==j)
                    {
                        shudu[i,j+7]=shudu[i+3,j+6];
                        shudu[i+6,j+5]=shudu[i+3,j+6];
                    }
                    else
                    {
                        shudu[i,j+4]=shudu[i+3,j+6];
                        shudu[i+6,j+5]=shudu[i+3,j+6];
                    }

                }
            }
        }
        /**
         * 返回随机生成的数独
         */
        public int[,] getmap()
        {
            new CreateMap().Create();
            return shudu;
        }
        /**
         * 生成数独字符串
         */
        public string getMapString(int pass)
        {
            string map = "";
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (random.Next(1, 1000) > (pass * 50))
                        map += shudu[i, j].ToString() + ",";
                    else
                        map += "-1,";
                }
            }
            return map;
        }
        /*
         * 获取数独
         */
        public int[,] getmap(int cid, int checkpoint)
        {
            int[,] maps = new int[GameInfo.gnum, GameInfo.gnum];
            string s = new SqlHelper().getMap(cid, checkpoint);
            string[] t = new string[GameInfo.gnum * GameInfo.gnum];
            t = s.Split(',');
            int m = 0;
            for (int i = 0; i < GameInfo.gnum; i++)
            {
                for (int j = 0; j < GameInfo.gnum; j++)
                {
                    maps[i, j] = int.Parse(t[m++]);
                }
            }
            return maps;
        }
        public int[,] getmap( int checkpoint)
        {
            int[,] maps = new int[GameInfo.gnum, GameInfo.gnum];
            string s = new SqlHelper().getCMap( checkpoint);
            string[] t = new string[GameInfo.gnum * GameInfo.gnum];
            t = s.Split(',');
            int m = 0;
            for (int i = 0; i < GameInfo.gnum; i++)
            {
                for (int j = 0; j < GameInfo.gnum; j++)
                {
                    maps[i, j] = int.Parse(t[m++]);
                }
            }
            return maps;
        }
    }

}
