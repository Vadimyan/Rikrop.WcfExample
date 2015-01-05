using RikropWcfExample.Server.BL;
using RikropWcfExample.Server.Contracts;
using System.Threading.Tasks;

namespace RikropWcfExample.Server
{
    public class CalculatorService : ICalculatorService
    {
        private readonly FibonacciCalculator _fibonacciCalculator;

        public CalculatorService(FibonacciCalculator.ICtor fibonacciCalculatorCtor)
        {
            _fibonacciCalculator = fibonacciCalculatorCtor.Create();
        }

        public async Task<ulong> GetFibonacciNumber(int n)
        {
            return await _fibonacciCalculator.Calculate(n);
            
        }
    }
}
