using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Calculator
{
    /// <summary>
    /// 四则表达式计算类
    /// </summary>
    public class Calculator
    {
        private string _expression;
        Queue<string> _queue = new Queue<string>();
        List<string> _postOrder = new List<string>();

        public Calculator(string expression)
        {
            _expression = expression.Replace(" ", "") + "#";
        }

        /// <summary>
        /// 获取计算结果
        /// </summary>
        /// <returns></returns>
        public decimal Run()
        {
            Parse();
            InorderToPostorder();
            return Calculate();
        }

        /// <summary>
        /// 解析表达式，并且入队
        /// 1.2 * 2 + ( 3 - 2 ) / 5
        /// </summary>
        void Parse()
        {
            int i = 0; // 标记当前数字的位置
            int j = 0; // 当前字符位置
            while (j < _expression.Length)
            {
                string c = _expression[j].ToString();
                if (IsOperators(c))
                {
                    if (i != j)
                    {
                        string num = _expression.Substring(i, j - i);
                        _queue.Enqueue(num);
                        _queue.Enqueue(c);
                        i = j + 1;
                    }
                    else
                    {
                        _queue.Enqueue(c);
                        i++;
                    }
                }
                j++;
            }
        }

        /// <summary>
        /// 中续表达式转后续表达式
        /// </summary>
        void InorderToPostorder()
        {
            Stack<string> inOrder = new Stack<string>();
            inOrder.Push("#");
            int count = _queue.Count;
            for (int i = 0; i < count; i++)
            {
                string item = _queue.Dequeue();
                if (!IsOperators(item))
                {
                    _postOrder.Add(item);
                }
                else
                {
                    string m = inOrder.Peek();
                    int priority = Priority.Compare(m, item);
                    while (priority == 1)
                    {
                        string temp = inOrder.Pop();
                        if (temp != "(" && temp != ")")
                        {
                            _postOrder.Add(temp);
                        }
                        m = inOrder.Peek();
                        priority = Priority.Compare(m, item);
                    }
                    if (priority == 2)
                    {
                        inOrder.Pop();
                    }
                    else if (priority == 0)
                    {
                        inOrder.Push(item);
                    }
                }
            }
        }

        decimal Calculate()
        {
            if (_postOrder.Count == 0)
                throw new Exception("计算失败：表达式错误");
            try
            {
                int i = 0;
                while (i < _postOrder.Count)
                {
                    string _operator = _postOrder[i];
                    if (IsOperators(_operator))
                    {
                        string x = _postOrder[i - 2];
                        string y = _postOrder[i - 1];
                        string result = Arithmetic(x, y, _operator);
                        _postOrder.RemoveRange(i - 2, 3);
                        _postOrder.Insert(i - 2, result);
                        i -= 2;
                    }
                    i++;
                }
                return decimal.Parse(_postOrder[0]);
            }
            catch (Exception)
            {
                throw new Exception("计算错误");
            }
        }

        /// <summary>
        /// 判断是不是操作符
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        bool IsOperators(string c) => Regex.IsMatch(c, @"\+|\-|\*|\/|\(|\)|#");

        string Arithmetic(string x, string y, string _operator)
        {
            decimal result = 0;
            decimal a = decimal.Parse(x);
            decimal b = decimal.Parse(y);
            switch (_operator)
            {
                case "+": result = a + b; break;
                case "-": result = a - b; break;
                case "*": result = a * b; break;
                case "/": result = a / b; break;
            }
            return result.ToString();
        }
    }
}
