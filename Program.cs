using System;

namespace Interpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            while(true)
            {
                string text;
                try
                {
                    Console.Write("calc> ");
                    text = Console.ReadLine();   
                }
                catch (System.Exception)
                {
                    break;
                }
                var interpreter = new Interpreter(text);
                var result = interpreter.Expr();
                Console.WriteLine(result);
            }
        }
    }
}
