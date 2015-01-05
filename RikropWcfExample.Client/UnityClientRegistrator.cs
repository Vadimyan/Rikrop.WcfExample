using System.Linq;
using System.Reflection;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Rikrop.Core.Framework.Unity.Factories;
using Rikrop.Core.Wcf.Unity.ClientRegistration;
using RikropWcfExample.Client.Presentation;

namespace RikropWcfExample.Client
{
    public static class UnityClientRegistrator
    {
        public static void RegisterClientDependencies(this IUnityContainer container)
        {
            const string ip = "localhost";
            const int port = 5070;

            container
                .RegisterWcf(ip, port)
                .RegisterFactories();
        }

        private static IUnityContainer RegisterWcf(this IUnityContainer container, string serviceIp, int servicePort)
        {
            container
                .RegisterClientWcf(o => o.RegisterServiceExecutor(reg => reg.Standard()
                                                         .WithExceptionConverters()
                                                         .AddFaultToBusinessConverter())
                .RegisterChannelWrapperFactory(reg => reg.Standard())
                .RegisterServiceConnection(reg => reg.NetTcp(serviceIp, servicePort)));

            

            return container;
        }

        private static IUnityContainer RegisterFactories(this IUnityContainer container)
        {
            new[] { Assembly.GetExecutingAssembly(), typeof(CalculateFibonacciCommand).Assembly }
                .SelectMany(assembly => assembly.DefinedTypes.Where(type => type.Name == "ICtor"))
                .Where(type => !container.IsRegistered(type))
                .ForEach(container.RegisterFactory);

            return container;
        }
    }
}
