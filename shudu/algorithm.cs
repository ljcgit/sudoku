using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace shudu
{
    class algorithm
    {
        int length = 9;      //默认数独是九宫格
        int[,] Data;         //数独数据
        /*
         * 初始化
         */
        public algorithm(int[,] data, int length)
        {  
            this.Data = data;  
            this.length = length;  
        }  
        // 描述数独一个结点  
        public class NodeShuDu  
        {  
            //数独中单元格标识  
            public int key  ;  
            //单元格值  
            public int value = 1;  
            //结束集合  
            public Dictionary<int, bool> dicConstraint;  
        }  
        public delegate Stack<NodeShuDu> DelNext(Stack<NodeShuDu> next, NodeShuDu node);  
       //  计算数独  
        public List<int[,]> Computing(bool allAnswer)  
        {  
            //初使化失败  
            if (Data == null)  
                return null;  
            //生成固定约束  
            //固定约束集合(根据已知条件生成约束)  
            Dictionary<int, Dictionary<int, bool>> dicConstraint = new Dictionary<int, Dictionary<int, bool>>();  
   
            int row = Data.GetLength(0);  
            int col = Data.GetLength(1);  
            int _k = 0;  
            for (int r = 0; r < row; r++)  
            {  
                for (int c = 0; c < col; c++)  
                {  
                    if (Data[r, c] != 0)  
                    {  
                        //检查已知条件是否合法  
                        _k = GetKey(r, c);  
                        CreateConstraint(r, c, Data[r, c], dicConstraint);  
                    }  
                }  
            }  
 
            DelNext GetNext = (strackNext, node) =>  
            {  
                int count = length * length;  
                int v = 0;  
                Dictionary<int, Dictionary<int, bool>> _dicC;  
                if (strackNext == null)  
                {  
                    strackNext = new Stack<NodeShuDu>();  
                }  
                if (node == null)  
                {  
                    node = new NodeShuDu();  
                    node.key = 0;  
                }  
 
                while (strackNext.Count < count)  
                {  
                    int r = 0, c = 0;  
                    GetRowCol(node.key, ref r, ref c);  
                    _dicC = null;  
                    v = GetNextValue(strackNext, dicConstraint, node.key, node.value);  
 
                    if (v > 0)  
                    {  
                        node.value = v;  
                        //将推测单元格节点压入堆栈中  
                        strackNext.Push(node);  
                        //添加约束  
                        _dicC = new Dictionary<int, Dictionary<int, bool>>();  
                        CreateConstraint(r, c, node.value, _dicC);  
                        node.dicConstraint = new Dictionary<int, bool>();  
                        foreach (KeyValuePair<int, Dictionary<int, bool>> p in _dicC)  
                        {  
                            if (node.dicConstraint.ContainsKey(p.Key))  
                                continue;  
                            node.dicConstraint.Add(p.Key, true);  
                        }  
                        int lstKey = node.key;  
                        //初始化下一下单元格  
                        node = new NodeShuDu();  
                        node.key = ++lstKey;  
                    }  
                    else 
                    {  
                        if (strackNext.Count > 0 )  
                        {  
                            node = strackNext.Pop();  
                            node.value++;  
                        }  
                        else 
                        {  
                            return null;  
                        }  
                    }  
                }  
                return strackNext;  
            };  
 
            List<int[,]> lst = new List<int[,]>();  
            Stack<NodeShuDu> strackresult = null;  
            NodeShuDu startNode = null;  
            while ((strackresult = GetNext(strackresult, startNode)) != null)  
            {  
                NodeShuDu[] result = strackresult.ToArray();  
                Array.Reverse(result);  
                int[,] arrResult = new int[length, length];  
                row = 0;  
                col = 0;  
                for (int i = 0; i < result.Length; i++)  
                {  
                    GetRowCol(i, ref row, ref col);  
                    arrResult[row, col] = result[i].value;  
                }  
                lst.Add(arrResult);  
                if (allAnswer)  
                {  
                    startNode = null;  
                    row = 0;  
                    col = 0;  
                     
                    //回朔到第一个程序推测的结点,这个结点值必须 小于 length(保证结点还能进一步推测)  
                    Stack<NodeShuDu> history = new Stack<NodeShuDu> () ;  
                    while (strackresult.Count > 0)  
                    {  
                       startNode = strackresult.Pop();  
                       history.Push(startNode);  
                   ;  
                    }  
                    if (startNode == null) {  
                        break;  
                    }  
                    GetRowCol(startNode.key, ref row, ref col) ;  
                    history.Pop();  
                    while (Data[row, col] > 0)  
                    {  
                        strackresult.Push(startNode);  
                        if (history.Count == 0)  
                        {  
                            startNode = null;  
                            break;  
                        }  
                        startNode = history.Pop();  
                        GetRowCol(startNode.key, ref row, ref col);  
                    }  
                    if (startNode == null)  
                        break;  
                    startNode.value++;  
                }  
                else 
                {  
                    break;  
                }  
            }  
            return lst;  
        }  
        //单元格推测值  
        public int GetNextValue(Stack<NodeShuDu>  strack ,Dictionary<int, Dictionary<int,bool>> dicConstraint , int key , int startValue ){  
            bool r = false;  
            while (startValue <= length)  
            {  
                Dictionary<int, bool> o;  
                if (dicConstraint.TryGetValue(key, out o))  
                {  
                    if (o.ContainsKey(startValue))  
                    {  
                        startValue++;  
                        continue;  
                    }  
                }  
                
                //需要遍历完所有的结点  
                r = false;  
                foreach (NodeShuDu node in strack) {  
                    if (startValue == node.value && node.dicConstraint.ContainsKey(key))  
                    {  
                        startValue++;  
                        r = true;  
                        continue;  
                    }  
                }  
                if (r)  
                    continue;  
                break;  
            }  
            if (startValue <= length)  
            {  
                return startValue;  
            }  
            else {  
                return 0;  
            }  
        }  
        /**
         * 将二维下标转为一维下标  
         */
        public int GetKey(int row, int col) {  
            return row * length + col;  
        }  
 
        /**
         * 将一维下标转为二维下标  
         */
        public void GetRowCol(int key , ref int row, ref int col) {  
            row = key / length;  
            col = key % length;  
        }  
 
        public delegate void AddConstraint(int key );  
        /**
         * 根据行列值生成约束  
        */
        public void CreateConstraint(int row, int col, int value, Dictionary<int, Dictionary<int,bool>> constraint)  
        {  
            if (constraint == null)  
                constraint = new Dictionary<int, Dictionary<int,bool>>();  
            int ck = GetKey(row, col);  
            AddConstraint addConstraint = (k) =>  
            {  
                if (ck  == k)  
                    return;  
                Dictionary<int,bool> result;  
                //取出相应一维坐标在字典中的值
                //如果字典中的值不存在则新建一个字典，添加到constraint字典中
                if (!constraint.TryGetValue(k, out result))  
                {  
                    result = new Dictionary<int,bool>();  
                    constraint.Add(k, result);  
                }  
                if (!result.ContainsKey(value))  
                    result.Add(value,true);  
                 
            };  
            //整行整列约束  
            for (int i = 0; i < length; i++)  
            {  
                addConstraint(GetKey(row, i));  
                addConstraint(GetKey(i, col));  
            }  
            //九宫格约束  
            int t = Convert.ToInt16(Math.Sqrt((double)length)) ;  
            int startR = row - row % t;  
            int startC = col - col % t;  
            for (int r = startR; r < startR + t; r++) {  
                for (int c = startC; c < startC + t ; c++) {  
                    addConstraint(GetKey(r, c));  
                }  
            }  
        }  
    }
}
