using System;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;

namespace CommunityBox.Common.Logging.Models
{
    public class LoggerRequestData : ILoggerData
    {
        public string RequestId { get; set; }

        public DateTime DateUtc { get; set; }

        public string HttpUri { get; set; }

        public string HttpMethod { get; set; }

        public string Type => LoggerDataType.Request.ToString();
        
        public string ServiceName { get; set; }

        public string HttpContent { get; set; }

        public KeyValuePair<string, StringValues>[] Headers { get; set; }
    }
}