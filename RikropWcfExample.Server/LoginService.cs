using System.Threading.Tasks;
using Rikrop.Core.Wcf.Security.Server;
using RikropWcfExample.Server.Contracts;
using RikropWcfExample.Server.Contracts.Model;

namespace RikropWcfExample.Server
{
    public class LoginService : ILoginService
    {
        private readonly ISessionResolver<Session> _sessionResolver;
        private readonly ISessionRepository<Session> _sessionRepository;

        public LoginService(ISessionResolver<Session> sessionResolver, ISessionRepository<Session> sessionRepository)
        {
            _sessionResolver = sessionResolver;
            _sessionRepository = sessionRepository;
        }

        public async Task<SessionDto> Login()
        {
            var sessionDto = await Task.Run(() => LoginInternal());

            return sessionDto;
        }

        private SessionDto LoginInternal()
        {
            var newSession = Session.Create(42);
            _sessionRepository.Add(newSession);
            var sessionDto = new SessionDto { SessionId = newSession.SessionId, Username = "ExampleUserName" };

            return sessionDto;
        }

        public async Task Logout()
        {
            var session = _sessionResolver.GetSession();
            await Task.Run(() => LogoutInternal(session));
        }

        private void LogoutInternal(Session session)
        {
            _sessionRepository.Delete(session.SessionId);
        }
    }
}
