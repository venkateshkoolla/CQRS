using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;
using Serilog.Events;

namespace Ocas.Domestic.Apply.Admin.Api.Middlewares
{
    public class OcasHttpRequestBodyLoggingMiddleware
    {
        private const string MessageTemplate =
            "HTTP Request Body Logged in Event Properties";

        private static readonly Serilog.ILogger _log = Serilog.Log.ForContext<OcasEnrichSerilogHttpContextMiddleware>();

        private readonly RequestDelegate _next;

        private readonly int _maxRequestLength = int.MaxValue;

        private readonly ICollection<string> _exclusionPaths;

        public OcasHttpRequestBodyLoggingMiddleware(RequestDelegate next, OcasHttpRequestBodyLoggingOptions options)
        {
            _next = next;

            _maxRequestLength = options?.MaximumRecordedRequestLength ?? _maxRequestLength;

            _exclusionPaths = options?.ExclusionPaths.Select(x => x.Trim('/')).ToList() ?? new List<string>();
        }

        public async Task Invoke(HttpContext httpContext)
        {
            // Skips all this log gathering if they are visiting the specified endpoints
            if (httpContext.Request.Path == "/" || _exclusionPaths.Any(x => httpContext.Request.Path.Value.IndexOf(x, StringComparison.OrdinalIgnoreCase) >= 0))
            {
                await _next(httpContext);
            }
            else
            {
                httpContext.Request.EnableRewind();

                await _next(httpContext);

                var statusCode = httpContext.Response?.StatusCode;
                var level = statusCode > 499 ? LogEventLevel.Error : LogEventLevel.Information;

                var contentType = GetContentType(httpContext.Request.Headers);
                var rawBody = await GetContentAsString(httpContext.Request.Body, contentType, (int)(httpContext.Request.ContentLength ?? 0));

                _log.ForContext("RequestBody", rawBody).Write(level, MessageTemplate);
            }
        }

        private static string GetContentType(IDictionary<string, StringValues> headers)
        {
            const string ContentType = "Content-Type";

            return headers.ContainsKey(ContentType) ? headers[ContentType][0] : null;
        }

        private static Encoding GetEncoding(string contentType)
        {
            var charset = "utf-8";
            var regex = new Regex(@";\s*charset=(?<charset>[^\s;]+)");
            var match = regex.Match(contentType);
            if (match.Success)
                charset = match.Groups["charset"].Value;

            try
            {
                return Encoding.GetEncoding(charset);
            }
            catch (ArgumentException)
            {
                return Encoding.UTF8;
            }
        }

        private static bool IsTextContentType(string contentType)
        {
            if (contentType == null)
                return false;

            return contentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase)
                    || contentType.StartsWith("application/xml", StringComparison.OrdinalIgnoreCase)
                    || contentType.StartsWith("text/", StringComparison.OrdinalIgnoreCase);
        }

        private async Task<string> GetContentAsString(Stream content, string contentType, int contentLength)
        {
            string body = null;
            if (!IsTextContentType(contentType))
            {
                contentType = string.IsNullOrEmpty(contentType) ? "N/A" : contentType;
                body = $"{contentType} [{contentLength} bytes]";
            }
            else
            {
                content.Seek(0, SeekOrigin.Begin);

                var length = Math.Min(_maxRequestLength, contentLength);
                var buffer = new byte[length];
                var count = await content.ReadAsync(buffer, 0, buffer.Length);

                body = GetEncoding(contentType).GetString(buffer, 0, count);

                content.Seek(0, SeekOrigin.Begin);
            }

            return body;
        }
    }
}
