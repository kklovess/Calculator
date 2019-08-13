using System;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            //double result = new Calculator("A * 0.8 + B * 0.2 + ( 5 - 2 ) / 2").Run();
            decimal result = new Calculator("5.000*2.0+1.2").Run();

            Console.WriteLine(result);
        }
    }
}
