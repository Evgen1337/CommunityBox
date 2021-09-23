using System;

namespace CommunityBox.Common.Logging.Models
{
    public class LoggerUnhandledError : ILoggerData
    {
        public string RequestId { get; set; }

        public DateTime DateUtc { get; set; }

        public string Type => LoggerDataType.UnhandledError.ToString();
        
        public string ServiceName { get; set; }

        public string Message { get; set; }

        public string Exception { get; set; }
    }
}