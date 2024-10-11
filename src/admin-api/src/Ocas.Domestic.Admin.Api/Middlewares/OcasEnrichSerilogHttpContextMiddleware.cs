using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Serilog.Events;

namespace Ocas.Domestic.Apply.Admin.Api.Middlewares
{
    public class OcasEnrichSerilogHttpContextMiddleware
    {
        private const string MessageTemplate =
            "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";

        private static readonly Serilog.ILogger _log = Serilog.Log.ForContext<OcasEnrichSerilogHttpContextMiddleware>();

        private readonly RequestDelegate _next;

        public OcasEnrichSerilogHttpContextMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

            var sw = Stopwatch.StartNew();
            try
            {
                await _next(httpContext);
                sw.Stop();

                var statusCode = httpContext.Response?.StatusCode;
                var level = statusCode > 499 ? LogEventLevel.Error : LogEventLevel.Information;

                var log = level == LogEventLevel.Error ? LogForErrorContext(httpContext) : _log;
                log.ForContext("RequestUrl", httpContext.Request.GetDisplayUrl())
                    .Write(level, MessageTemplate, httpContext.Request.Method, httpContext.Request.Path, statusCode, sw.Elapsed.TotalMilliseconds);
            }

            // Never caught, because `LogException()` returns false.
            catch (Exception ex) when (LogException(httpContext, sw, ex)) { }
        }

        private static bool LogException(HttpContext httpContext, Stopwatch sw, Exception ex)
        {
            sw.Stop();

            LogForErrorContext(httpContext)
                .Error(ex, MessageTemplate, httpContext.Request.Method, httpContext.Request.Path, 500, sw.Elapsed.TotalMilliseconds);

            return false;
        }

        private static Serilog.ILogger LogForErrorContext(HttpContext httpContext)
        {
            var request = httpContext.Request;

            var result = _log
                .ForContext("RequestHeaders", request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), destructureObjects: true)
                .ForContext("RequestHost", request.Host)
                .ForContext("RequestConnectionRemoteIpAddress", httpContext.Connection?.RemoteIpAddress?.ToString())
                .ForContext("RequestProtocol", request.Protocol);

            if (request.HasFormContentType)
                result = result.ForContext("RequestForm", request.Form.ToDictionary(v => v.Key, v => v.Value.ToString()));

            return result;
        }
    }
}
