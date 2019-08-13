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
        Queue<string> _inOrder = new Queue<string>(); // 中续表达式队列
        Queue<string> _postOrder = new Queue<string>();  // 后续表达式队列
        Stack<string> _operatorStack = new Stack<string>(); // 操作符栈


        public Calculator(string expression)
        {
            _expression = expression.Replace(" ", "") + "#"; //#表示临界符
            _operatorStack.Push("#");
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

        public decimal RunV2()
        {
            Parse();
            InorderToPostorderV2();
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
                        _inOrder.Enqueue(num);
                        _inOrder.Enqueue(c);
                        i = j + 1;
                    }
                    else
                    {
                        _inOrder.Enqueue(c);
                        i++;
                    }
                }
                j++;
            }
        }

        void InorderToPostorder()
        {
            while (_inOrder.Count > 0)
            {
                string item = _inOrder.Dequeue();
                if (!IsOperators(item))
                {
                    _postOrder.Enqueue(item);
                }
                else
                {
                    // 优先级高的操作符往前放
                    string top = _operatorStack.Peek();
                    PriorityV2.EnumPriority priority = PriorityV2.Compare(top, item);
                    while (priority == PriorityV2.EnumPriority.大于)
                    {
                        string temp = _operatorStack.Pop();
                        if (temp != "(" && temp != ")")
                        {
                            _postOrder.Enqueue(temp);
                        }
                        top = _operatorStack.Peek();
                        priority = PriorityV2.Compare(top, item);
                    }
                    if (priority == PriorityV2.EnumPriority.等于)
                    {
                        _operatorStack.Pop();
                    }
                    else if (priority == PriorityV2.EnumPriority.小于)
                    {
                        _operatorStack.Push(item);
                    }
                    else
                    {
                        throw new Exception("表达式错误");
                    }
                }
            }
            Console.WriteLine(string.Join(" ", _postOrder.ToArray()));
        }

        /// <summary>
        /// 中续表达式转后续表达式
        /// 
        /// 中序表达式转换为逆波兰表达式的一般算法是:
        ///     首先需要分配2个栈，一个作为临时存储运算符的栈S1（含一个结束符号），
        /// 一个作为输入逆波兰式的栈S2（空栈），S1栈可先放入优先级最低的运算符#，
        /// 注意，中缀式应以此最低优先级的运算符结束。可指定其他字符，不一定非#不可。
        /// 从中缀式的左端开始取字符，逐序进行如下步骤：
        /// 
        /// step1: 若取出的字符是操作数，将该操作数直接送入S2栈
        /// step2: 若取出的字符是运算符，则将该运算符与S1栈栈顶元素比较，
        ///        如果，该运算符优先级大于S1栈栈顶运算符优先级，则将该运算符进S1栈;
        ///        否则，将S1栈的栈顶运算符弹出，送入S2栈中，直至S1栈栈顶运算符低于（不包括等于）该运算符优先级，则将该运算符送入S1栈。
        /// step3: 若取出的字符是“（”，则直接送入S1栈栈顶
        /// step4: 若取出的字符是“）”，则将距离S1栈栈顶最近的“（”之间的运算符，逐个出栈，依次送入S2栈，此时抛弃“（”。
        /// 
        /// </summary>
        void InorderToPostorderV2()
        {
            Dictionary<string, int> _priorityDic = new Dictionary<string, int>
            {
                { "#" , 0 },
                { "+" , 1 },
                { "-" , 1 },
                { "*" , 2 },
                { "/" , 2 },
            };
            while (_inOrder.Count > 0)
            {
                string item = _inOrder.Dequeue();
                if (!IsOperators(item))
                {
                    _postOrder.Enqueue(item);
                }
                else if (item == "(")
                {
                    _operatorStack.Push(item);
                }
                else if (item == ")")
                {
                    string tempOp;
                    while (_operatorStack.Count > 0 && (tempOp = _operatorStack.Pop()) != "(")
                    {
                        _postOrder.Enqueue(tempOp);
                    }
                }
                else
                {
                    string top = _operatorStack.Peek();
                    if (top != "(" && _priorityDic[item] <= _priorityDic[top])
                    {
                        _postOrder.Enqueue(_operatorStack.Pop());
                    }
                    _operatorStack.Push(item);
                }
            }
            while (_operatorStack.Count > 0)
            {
                string top = _operatorStack.Pop();
                if (top != "#")
                {
                    _postOrder.Enqueue(top);
                }
            }
            Console.WriteLine(string.Join(" ", _postOrder.ToArray()));
        }

        /// <summary>
        /// 对后续表达式进行计算
        /// </summary>
        /// <returns></returns>
        decimal Calculate()
        {
            if (_postOrder.Count == 0)
                throw new Exception("计算失败：表达式错误");

            // 临时栈用于存储数字（利用后进先出的特性）
            Stack<string> tempStack = new Stack<string>();
            while (_postOrder.Count > 0)
            {
                string item = _postOrder.Dequeue();
                if (IsOperators(item))
                {
                    string y = tempStack.Pop(); // 这个是右边的操作数
                    string x = tempStack.Pop(); // 这个是左边的操作数
                    string result = Arithmetic(x, y, item);
                    tempStack.Push(result);
                }
                else
                {
                    tempStack.Push(item);
                }
                Console.WriteLine(string.Join(" ", tempStack.ToArray()));
            }
            return decimal.Parse(tempStack.Pop());
        }

        /// <summary>
        /// 判断是不是操作符
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        bool IsOperators(string c) => Regex.IsMatch(c, @"\+|\-|\*|\/|\(|\)|#");

        /// <summary>
        /// 算术
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="_operator"></param>
        /// <returns></returns>
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
                case "/":
                    {
                        if (b == 0)
                        {
                            throw new Exception("除以0错误");
                        }
                        result = a / b;
                        break;
                    }

            }
            return result.ToString();
        }
    }
}
