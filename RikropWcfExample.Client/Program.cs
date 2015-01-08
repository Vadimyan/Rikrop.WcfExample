using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using System;
using Rikrop.Core.Framework.Services;
using RikropWcfExample.Client.Presentation;
using RikropWcfExample.Server.Contracts;

namespace RikropWcfExample.Client
{
    class Program
    {
        static void Main()
        {
            using (var container = new UnityContainer())
            {
                container.RegisterClientDependencies();

                var clientSession = container.Resolve<ClientSession>();
                var loginServiceExecutor = container.Resolve<IServiceExecutor<ILoginService>>();
                var calculateFibonacciCommandCtor = container.Resolve<CalculateFibonacciCommand.ICtor>();

                var sessionDto = Task.Run(async () => await loginServiceExecutor.Execute(s => s.Login())).Result;
                clientSession.Session = sessionDto;

                int number;
                while (int.TryParse(GetUserInput(), out number) && number != 0)
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
