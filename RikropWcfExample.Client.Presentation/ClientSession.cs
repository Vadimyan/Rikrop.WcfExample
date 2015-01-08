using Rikrop.Core.Wcf.Security;
using RikropWcfExample.Server.Contracts.Model;

namespace RikropWcfExample.Client.Presentation
{
    public class ClientSession : ISessionIdResolver
    {
        public SessionDto Session { get; set; }

        public string SessionId
        {
            get { return Session == null ? null : Session.SessionId; }
        }
    }
}
