using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator
{
    public static class Priority
    {
        static int[,] _priority = new int[,] {
             /*行为入栈运算符，列为栈顶运算符*/
             /* 2表示 =，1表示 >，0表示 <，-1表示匹配错误*/

              /* +  -  *  /  (   )   # */
        /* + */{ 1, 1, 0, 0,  0,  1,  1 },
        /* - */{ 1, 1, 0, 0,  0,  1,  1 },
        /* * */{ 1, 1, 1, 1,  0,  1,  1 },
        /* / */{ 1, 1, 1, 1,  0,  1,  1 },
        /* ( */{ 0, 0, 0, 0,  0,  2, -1 },
        /* ) */{ 0, 0, 0, 0, -1,  1,  1 },
        /* # */{ 0, 0, 0, 0,  0, -1,  2 },
        };

        public static int Compare(string _operator1, string _operator2)
        {
            return _priority[GetIndex(_operator1), GetIndex(_operator2)];
        }

        static int GetIndex(string str) => "+-*/()#".IndexOf(str);
    }
}
