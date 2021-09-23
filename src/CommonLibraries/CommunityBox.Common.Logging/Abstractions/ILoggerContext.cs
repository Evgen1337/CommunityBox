namespace CommunityBox.Common.Logging.Abstractions
{
    public interface ILoggerContext
    {
        public string RequestId { get; }

        public string ServiceName { get; set; }
    }
}