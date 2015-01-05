using System.Threading;
using System.Threading.Tasks;
using Rikrop.Core.Framework.Services;
using RikropWcfExample.Server.Contracts;

namespace RikropWcfExample.Client.Presentation
{
    public class CalculateFibonacciCommand
    {
        private readonly IServiceExecutor<ICalculatorService> _serviceExecutor;

        public CalculateFibonacciCommand(IServiceExecutor<ICalculatorService> serviceExecutor)
        {
            _serviceExecutor = serviceExecutor;
        }

        public int Execute(int number)
        {
            var task = Task.Run(async () => await _serviceExecutor.Execute(s => s.GetFibonacciNumber(number)));
            //task.Wait(); 
            Thread.Sleep(1000);
            if (task.Exception != null)
                throw task.Exception;

            return (int) task.Result;
        }

        public interface ICtor
        {
            CalculateFibonacciCommand Create();
        }
    }
}
