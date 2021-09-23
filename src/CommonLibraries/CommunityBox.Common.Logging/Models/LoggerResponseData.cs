using System;

namespace CommunityBox.Common.Logging.Models
{
    public class LoggerResponseData : ILoggerData
    {
        public string RequestId { get; set; }

        public DateTime DateUtc { get; set; }


        public string Type => LoggerDataType.Response.ToString();
        
        public string ServiceName { get; set; }

        public string HttpContent { get; set; }

        public int StatusCode { get; set; }
    }
}