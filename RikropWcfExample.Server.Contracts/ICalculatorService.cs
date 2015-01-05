using System.ServiceModel;
using System.Threading.Tasks;

namespace RikropWcfExample.Server.Contracts
{
    [ServiceContract]
    public interface ICalculatorService
    {
        [OperationContract]
        Task<ulong> GetFibonacciNumber(int n);
    }
}
