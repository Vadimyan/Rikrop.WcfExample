using System;
using System.Threading.Tasks;
using Rikrop.Core.Framework.Exceptions;

namespace RikropWcfExample.Server.BL
{
    public class FibonacciCalculator
    {
        public async Task<ulong> Calculate(int n)
        {
            if(n < 0)
                throw new BusinessException("Недопустимое значение индекса");
                    
            return await Task.Run(() => CalculateNumber(n));
        }

        private ulong CalculateNumber(int n)
        {
            double sqrt5 = Math.Sqrt(5);
            double p1 = (1 + sqrt5) / 2;
            double p2 = -1 * (p1 - 1);

            double n1 = Math.Pow(p1, n);
            double n2 = Math.Pow(p2, n);
            return (ulong)((n1 - n2) / sqrt5);
        }

        public interface ICtor
        {
            FibonacciCalculator Create();
        }
    }
}
