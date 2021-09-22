using System;
using System.Net;
using System.Runtime.Serialization;

namespace CommunityBox.Common.Exceptions
{
    [Serializable]
    public class EntityNotFoundException : ClientExceptionBase
    {
        private const string DefaultMessage = "Запись не найдена";
        
        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(string message) : base(message)
        {
        }

        protected EntityNotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

        public override string Message => DefaultMessage;
        
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
    }
}