using CommunityBox.Common.Logging.Abstractions;
using NLog;

namespace CommunityBox.Common.Logging.Implementations
{
    public class LoggerProvider : ILoggerProvider
    {
        public ILoggerContext LoggerContext { get; }


        public LoggerProvider(ILoggerContext loggerContext)
        {
            LoggerContext = loggerContext;
        }

        public ILogger GetLogger<T>() => LogManager.GetLogger(nameof(T));
    }
}