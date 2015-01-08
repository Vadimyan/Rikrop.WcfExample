using System.Runtime.Serialization;

namespace RikropWcfExample.Server.Contracts.Model
{
    [DataContract(IsReference = true)]
    public class SessionDto
    {
        /// <summary>
        /// Сессия.
        /// </summary>
        [DataMember]
        public string SessionId { get; set; }
        /// <summary>
        /// Данные пользователя.
        /// </summary>
        [DataMember]
        public string Username { get; set; }
    }
}
