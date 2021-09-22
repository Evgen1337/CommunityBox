using System;
using System.Net;
using System.Runtime.Serialization;

namespace CommunityBox.Common.Exceptions
{
    [Serializable]
    public abstract class ClientExceptionBase : Exception
    {
        protected ClientExceptionBase()
        {
        }

        protected ClientExceptionBase(string message) : base(message)
        {
        }

        protected ClientExceptionBase(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ClientExceptionBase(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        ///     Тип ошибки по умолчанию
        /// </summary>
        public virtual HttpStatusCode StatusCode => HttpStatusCode.InternalServerError;
    }
}