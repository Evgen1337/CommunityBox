using System;
using CommunityBox.Common.Logging.Abstractions;

namespace CommunityBox.Common.Logging.Implementations
{
    public class LoggerContext : ILoggerContext
    {
        public string RequestId { get; }
        
        public string ServiceName { get; set; }

        public LoggerContext()
        {
            RequestId = Guid.NewGuid().ToString();
        }
    }
}