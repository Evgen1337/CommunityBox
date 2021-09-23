using System;
using CommunityBox.Common.Logging.Abstractions;
using CommunityBox.Common.Logging.Models;
using Newtonsoft.Json;
using NLog;

namespace CommunityBox.Common.Logging.Implementations
{
    public static class LoggerExtensions
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = new()
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        public static void LogInfoObj(this ILogger logger, string message, string marker = null,
            ILoggerContext context = null)
        {
            var infoData = new LoggerInfoData
            {
                DateUtc = DateTime.UtcNow,
                RequestId = context?.RequestId,
                Marker = marker,
                Message = message,
            };
            
            var json = JsonConvert.SerializeObject(infoData, JsonSerializerSettings);
            logger.Info(json);
        }

        public static void LogUnhandledErrorObj(this ILogger logger, Exception exception, ILoggerContext context)
        {
            var logUnhandledError = new LoggerUnhandledError
            {
                DateUtc = DateTime.UtcNow,
                RequestId = context.RequestId,
                Message = exception.Message,
                Exception = exception.ToString()
            };

            var json = JsonConvert.SerializeObject(logUnhandledError, JsonSerializerSettings);
            logger.Error(json);
        }

        public static void LogRequest(this ILogger logger, LoggerRequestData request)
        {
            var retval = JsonConvert.SerializeObject(request, JsonSerializerSettings);
            logger.Info(retval);
        }

        public static void LogResponse(this ILogger logger, LoggerResponseData responseData)
        {
            var retval = JsonConvert.SerializeObject(responseData, JsonSerializerSettings);
            logger.Info(retval);
        }
    }
}