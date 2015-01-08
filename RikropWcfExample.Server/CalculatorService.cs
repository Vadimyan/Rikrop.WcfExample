using Rikrop.Core.Framework.Exceptions;
using Rikrop.Core.Framework.Logging;
using Rikrop.Core.Wcf.Security.Server;
using RikropWcfExample.Server.BL;
using RikropWcfExample.Server.Contracts;
using System.Threading.Tasks;

namespace RikropWcfExample.Server
{
    public class CalculatorService : ICalculatorService
    {
        private readonly FibonacciCalculator _fibonacciCalculator;
        private readonly ISessionResolver<Session> _sessionResolver;
        private readonly ILogger _logger;

        public CalculatorService(FibonacciCalculator.ICtor fibonacciCalculatorCtor
            , ISessionResolver<Session> sessionResolver
            , ILogger logger)
        {
            _sessionResolver = sessionResolver;
            _logger = logger;
            _fibonacciCalculator = fibonacciCalculatorCtor.Create();
        }

        public async Task<ulong> GetFibonacciNumber(int n)
        {
            if (!_sessionResolver.HasSession())
            {
                const string errorMessage = "Попытка вызвать метод для неавторизированного пользователя";
                _logger.LogWarning(errorMessage);
                throw new BusinessException(errorMessage);
            }

            var session = _sessionResolver.GetSession();
            _logger.LogInfo(
                string.Format("User with SessionId={0} and UserId={1} called CalculatorService.GetFibonacciNumber", session.SessionId, session.UserId));

            return await _fibonacciCalculator.Calculate(n);
            
        }
    }
}
