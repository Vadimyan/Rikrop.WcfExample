using Microsoft.Practices.Unity;
using System;

namespace RikropWcfExample.Server.ConsoleServiceHost
{
    public class Program
    {
        public static void Main()
        {
            using (var serverContainer = new UnityContainer())
            {
                serverContainer.RegisterServerDependencies();

                var service = serverContainer.Resolve<WcfHoster>();
                service.Start();

                Console.WriteLine("Сервер запущен. Для остановки нажмите Enter.");
                Console.ReadLine();

                service.Stop();
            }
        }
    }
}
