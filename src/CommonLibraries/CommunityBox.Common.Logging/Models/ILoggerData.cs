using System;

namespace CommunityBox.Common.Logging.Models
{
    public interface ILoggerData
    {
        public string RequestId { get; }

        public DateTime DateUtc { get; }

        public string Type { get; }

        public string ServiceName { get; set; }
    }
}