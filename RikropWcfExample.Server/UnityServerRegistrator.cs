using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Rikrop.Core.Framework.Logging;
using Rikrop.Core.Framework.Unity.Factories;
using Rikrop.Core.Logging.NLog;
using Rikrop.Core.Wcf.Security.Server;
using Rikrop.Core.Wcf.Unity.ServerRegistration;
using RikropWcfExample.Server.BL;
using System.Linq;
using System.Reflection;
using RikropWcfExample.Server.Contracts;

namespace RikropWcfExample.Server
{
    public static class UnityServerRegistrator
    {
        public static void RegisterServerDependencies(this IUnityContainer container)
        {
            const string ip = "localhost";
            const int port = 5070;

            container
                .RegisterWcfHosting(ip, port)
                .RegisterFactories()
                .RegisterLogger();
        }

        private static IUnityContainer RegisterWcfHosting(this IUnityContainer container, string serviceIp, int servicePort)
        {
            container
                .RegisterType<ISessionResolver<Session>, SessionResolver<Session>>()
                .RegisterServerWcf(
                    o => o.RegisterServiceConnection(reg => reg.NetTcp(serviceIp, servicePort))
                          .RegisterServiceHostFactory(reg => reg.WithBehaviors()
                                                                .AddErrorHandlersBehavior(eReg => eReg.AddBusinessErrorHandler().AddLoggingErrorHandler(NLogger.CreateEventLogTarget()))
                                                                .AddDependencyInjectionBehavior()
                                                                .AddServiceAuthorizationBehavior(sReg => sReg.WithStandardAuthorizationManager()
                                                                                           .WithStandardSessionHeaderInfo("ExampleNamespace", "SessionId")
                                                                                           .WithOperationContextSessionIdInitializer()
                                                                                           .WithSessionAuthStrategy<Session>()
                                                                                           .WithLoginMethod<ILoginService>(s => s.Login())
                                                                                           .WithOperationContextSessionIdResolver()
                                                                                           .WithInMemorySessionRepository()
                                                                                           .WithStandardSessionCopier())
                                                     )
                                  );

            return container;
        }

        private static IUnityContainer RegisterFactories(this IUnityContainer container)
        {
            new[] { Assembly.GetExecutingAssembly(), typeof (FibonacciCalculator).Assembly }
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
