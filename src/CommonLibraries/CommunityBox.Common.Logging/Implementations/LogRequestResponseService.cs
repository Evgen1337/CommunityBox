using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityBox.Common.Logging.Abstractions;
using CommunityBox.Common.Logging.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

// ReSharper disable HeuristicUnreachableCode
// ReSharper disable once ConditionIsAlwaysTrueOrFalse

namespace CommunityBox.Common.Logging.Implementations
{
    public class LogRequestResponseService : ILogRequestResponseService
    {
        private readonly ILoggerContext _loggerContext;
        private readonly IReadOnlyCollection<string> _availableContentTypes;
        private JsonSerializer _jsonSerializer;

        public LogRequestResponseService(ILoggerContext loggerContext)
        {
            _loggerContext = loggerContext;
            _jsonSerializer = new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            _availableContentTypes = new[]
            {
                "application/json",
                "text/xml",
                "application/vnd.api+json",
                "application/xml",
                "application/json",
                "application/problem+json",
                "application/grpc"
            };
        }

        public async Task<LoggerRequestData> GetHttpRequestAsync(Stream httpContent, HttpRequest request)
        {
            var retvalObject = new LoggerRequestData
            {
                RequestId = _loggerContext.RequestId,
                DateUtc = DateTime.UtcNow,
                HttpMethod = request.Method,
                HttpUri = request.Path,
                Headers = request.Headers.ToArray()
            };

            if (request.ContentType != null)
            {
                retvalObject.HttpContent = IsValidContentType(request.ContentType)
                    ? await GetBodyAsStringAsync(httpContent)
                    : $"content logging is disabled for {request.ContentType}";
            }

            return retvalObject;
        }

        public LoggerRequestData GetGrpcRequest<T>(T content, HttpRequest request)
        {
            var retvalObject = new LoggerRequestData
            {
                RequestId = _loggerContext.RequestId,
                DateUtc = DateTime.UtcNow,
                HttpMethod = request.Method,
                HttpUri = request.Path,
                Headers = request.Headers.ToArray(),
                HttpContent = IsValidContentType(request.ContentType)
                    ? GetBodyAsString(content)
                    : $"content logging is disabled for {request.ContentType}"
            };

            return retvalObject;
        }

        public async Task<LoggerResponseData> GetHttpResponseAsync(HttpResponse response)
        {
            var retvalObject = new LoggerResponseData
            {
                StatusCode = response.StatusCode,
                RequestId = _loggerContext.RequestId,
                DateUtc = DateTime.UtcNow
            };

            if (response.ContentType != null)
            {
                retvalObject.HttpContent = IsValidContentType(response.ContentType)
                    ? await GetBodyAsStringAsync(response.Body)
                    : $"content logging is disabled for content type - {response.ContentType}";
            }

            return retvalObject;
        }

        public LoggerResponseData GetGrpcResponse<T>(T content, HttpResponse response)
        {
            var retvalObject = new LoggerResponseData
            {
                StatusCode = response.StatusCode,
                RequestId = _loggerContext.RequestId,
                DateUtc = DateTime.UtcNow,
                HttpContent = IsValidContentType(response.ContentType)
                    ? GetBodyAsString(content)
                    : $"content logging is disabled for content type - {response.ContentType}"
            };

            return retvalObject;
        }

        private static string GetBodyAsString<T>(T content) =>
            JsonConvert.SerializeObject(content);

        private static async Task<string> GetBodyAsStringAsync(Stream body)
        {
            body.Seek(0, SeekOrigin.Begin);

            await using var ms = new MemoryStream();
            await body.CopyToAsync(ms);
            var bodyAsText = Encoding.UTF8.GetString(ms.ToArray());

            body.Seek(0, SeekOrigin.Begin);

            return bodyAsText;
        }

        private bool IsValidContentType(string contentType) =>
            contentType != null && _availableContentTypes.Any(a => contentType.ToLower().Contains(a));
    }
}