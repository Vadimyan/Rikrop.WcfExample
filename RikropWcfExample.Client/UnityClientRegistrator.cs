using System.Linq;
using System.Reflection;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Rikrop.Core.Framework.Logging;
using Rikrop.Core.Framework.Unity.Factories;
using Rikrop.Core.Logging.NLog;
using Rikrop.Core.Wcf.Unity.ClientRegistration;
using RikropWcfExample.Client.Presentation;
using RikropWcfExample.Server.Contracts;

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
                .RegisterFactories()
                .RegisterLogger();
        }

        private static IUnityContainer RegisterWcf(this IUnityContainer container, string serviceIp, int servicePort)
        {
            container
                .RegisterType<ClientSession>(new ContainerControlledLifetimeManager())
                .RegisterClientWcf(o => o.RegisterServiceExecutor(reg => reg.Standard()
                                                         .WithExceptionConverters()
                                                         .AddFaultToBusinessConverter())
                .RegisterChannelWrapperFactory(reg => reg.Standard())
                .RegisterServiceConnection(reg => reg.NetTcp(serviceIp, servicePort)
                                                     .WithBehaviors()
                                                     .AddSessionBehavior(sReg => sReg.WithStandardSessionHeaderInfo("ExampleNamespace", "SessionId")
                                                                                     .WithCustomSessionIdResolver<ClientSession>(new ContainerControlledLifetimeManager())
                                                                                     .WithStandardMessageInspectorFactory<ILoginService>(service => service.Login())
                                                                        )
                                          )
                                  );

            

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

        private static IUnityContainer RegisterLogger(this IUnityContainer container)
        {
            container.RegisterType<ILogger>(new ContainerControlledLifetimeManager(),
                                            new InjectionFactory(f => NLogger.CreateConsoleTarget()));

            return container;
        }
    }
}
