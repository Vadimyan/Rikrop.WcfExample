using Microsoft.Practices.Unity;
using System;
using RikropWcfExample.Client.Presentation;

namespace RikropWcfExample.Client
{
    class Program
    {
        static void Main()
        {
            using (var container = new UnityContainer())
            {
                container.RegisterClientDependencies();

                var calculateFibonacciCommandCtor = container.Resolve<CalculateFibonacciCommand.ICtor>();

                int number;
                while (int.TryParse(GetUserInput(), out number))
                {
                    var command = calculateFibonacciCommandCtor.Create();
                    var result = command.Execute(number);

                    Console.WriteLine("Fibonacci[{0}] = {1}", number, result);
                } 
            }
        }

        private static string GetUserInput()
        {
            Console.Write("Введите индекс или 0 для выхода: ");
            return Console.ReadLine();
        }
    }
}
