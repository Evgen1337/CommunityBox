using NLog;

namespace CommunityBox.Common.Logging.Abstractions
{
    public interface ILoggerProvider
    {
        public ILoggerContext LoggerContext { get; }

        public ILogger GetLogger<T>();
    }
}