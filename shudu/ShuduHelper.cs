using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace shudu
{
    class ShuduHelper
    {
        private int[,] matrix = new int[6, 6];
        private int[,] map = new int[6, 6];
        private List<int[,]> maps = new List<int[,]>();
   //     private List<int[,]>  maps=new List<int[,]>;
        private int count = 0;   //解的数量
        public ShuduHelper(int[,] s)
        {
            matrix = s;
            execute();
        }
        public List<int[,]> getMap()
        {
            return maps;
        }
        public int getCount()
        {
            return count;
        }
        private bool execute(int i, int j)
        {
            bool flag=false;
            for (int x = i; x < 6; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    if(matrix[x,y]==-1)
                    {
                        i=x;
                        j=y;
                        flag=true;
                        break;
                    }
                }
                if(flag)
                    break;
            }
            if(matrix[i,j]!=-1)
            {
                count++;
                output();
                return false;
            }
            for(int k=1;k<=6;k++)
            {
                if(!check(i,j,k))
                    continue;
                matrix[i,j]=k;
                if(i==5&&j==5)
                {
                    count++;
                    output();
                    return false;
                }
                int nextRow=(j<6-1)?i:i+1;
                int nextCol=(j<6-1)?j+1:0;
                if(execute(nextRow,nextCol))
                    return true;
                matrix[i,j]=-1;
            }
            return false;
        }
        public void output()
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                    map[i, j] = matrix[i, j];
            }
            maps.Add(map);
        }
        public void execute()
        {
            execute(0, 0);
        }
        private bool check(int i, int j, int k)
        {
            for (int index = 0; index < 6; index++)
            {
                if (matrix[i, index] == k) return false;
                if (matrix[index, j] == k) return false;
                int ii = (i / 2) * 2 + index / 3;   //小宫格内的行
                int jj = (j / 3) * 3 + index % 3;   //小宫格内的列
                if (matrix[ii, jj] == k) return false;
            }
            return true;
        }
    }
}
