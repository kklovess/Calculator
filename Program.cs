using System;
using System.Diagnostics;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            decimal result1 = new Calculator("0.2 * 0.8 + 4 * 0.3 / 2 + ( 5 - 2 ) * 2 / ( 2 * ( 1.2 + 0.3 ) )").Run();
            sw.Stop();
            Console.WriteLine($"result1 is {result1}");
            Console.WriteLine($"{sw.ElapsedMilliseconds} ms");

            sw.Start();
            decimal result2 = new Calculator("0.2 * 0.8 + 4 * 0.3 / 2 + ( 5 - 2 ) * 2 / ( 2 * ( 1.2 + 0.3 ) )").RunV2();
            sw.Stop();
            Console.WriteLine($"result2 is {result2}");
            Console.WriteLine($"{sw.ElapsedMilliseconds} ms");
        }
    }
}
