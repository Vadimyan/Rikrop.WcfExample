using System.ServiceModel;
using System.Threading.Tasks;
using RikropWcfExample.Server.Contracts.Model;

namespace RikropWcfExample.Server.Contracts
{
    [ServiceContract]
    public interface ILoginService
    {
        [OperationContract]
        Task<SessionDto> Login();

        [OperationContract]
        Task Logout();
    }
}
