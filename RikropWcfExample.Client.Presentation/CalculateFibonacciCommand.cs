using System.Threading;
using System.Threading.Tasks;
using Rikrop.Core.Framework.Logging;
using Rikrop.Core.Framework.Services;
using RikropWcfExample.Server.Contracts;

namespace RikropWcfExample.Client.Presentation
{
    public class CalculateFibonacciCommand
    {
        private readonly IServiceExecutor<ICalculatorService> _serviceExecutor;
        private readonly ILogger _logger;
        private readonly ClientSession _clientSession;

        public CalculateFibonacciCommand(IServiceExecutor<ICalculatorService> serviceExecutor
            , ILogger logger
            , ClientSession clientSession)
        {
            _serviceExecutor = serviceExecutor;
            _logger = logger;
            _clientSession = clientSession;
        }

        public int Execute(int number)
        {
            if (!string.IsNullOrEmpty(_clientSession.SessionId))
                _logger.LogInfo(string.Format("SessionId {0} with name {1} begin calculate Fibomacci", _clientSession.SessionId, _clientSession.Session.Username));

            var task = Task.Run(async () => await _serviceExecutor.Execute(s => s.GetFibonacciNumber(number)));
            Thread.Sleep(1000);

            if (task.Exception != null)
            {
                _logger.LogError(task.Exception);
                return -1;
            }

            return (int) task.Result;
        }

        public interface ICtor
        {
            CalculateFibonacciCommand Create();
        }
    }
}
