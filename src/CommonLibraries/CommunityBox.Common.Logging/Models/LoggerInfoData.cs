using System;

namespace CommunityBox.Common.Logging.Models
{
    public class LoggerInfoData : ILoggerData
    {
        public string RequestId { get; set; }

        public DateTime DateUtc { get; set; }

        public string Type => LoggerDataType.Info.ToString();
        
        public string ServiceName { get; set; }

        public string Marker { get; set; }

        public string Message { get; set; }
    }
}