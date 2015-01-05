using Rikrop.Core.Wcf;
using System.Reflection;

namespace RikropWcfExample.Server
{
    public class WcfHoster
    {
        private readonly ServiceHostManager _serviceHostManager;

        public WcfHoster(ServiceHostManager serviceHostManager)
        {
            _serviceHostManager = serviceHostManager;
        }

        public void Start()
        {
            var assembly = Assembly.GetExecutingAssembly();
            _serviceHostManager.StartServices(assembly);
        }

        public void Stop()
        {
            _serviceHostManager.StopServices();
        }
    }
}
