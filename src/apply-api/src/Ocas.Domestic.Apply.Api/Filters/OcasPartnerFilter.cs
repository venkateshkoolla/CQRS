using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Ocas.Common.Exceptions;

namespace Ocas.Domestic.Apply.Api.Filters
{
    public class OcasPartnerFilter : IActionFilter
    {
        private const string HeaderKey = "X-Partner";

        private readonly RequestCache _requestCache;
        private readonly ILogger<OcasPartnerFilter> _logger;

        public OcasPartnerFilter(RequestCache requestCache, ILoggerFactory loggerFactory)
        {
            _requestCache = requestCache;
            _logger = loggerFactory.CreateLogger<OcasPartnerFilter>();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext ??
                throw new Exception("HttpContext is null");

            var partnerHeader = httpContext.Request.Headers.TryGetValue(HeaderKey, out var partnerValues);
            if (!partnerHeader || StringValues.IsNullOrEmpty(partnerValues)) return;

            var partnerArray = partnerValues
                .First()
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .ToArray();
            if (partnerArray.Length > 1) throw new ValidationException($"Mulitple {HeaderKey} headers are not allowed.");

            _requestCache.AddOrUpdate(Constants.RequestCacheKeys.Partner, partnerArray.First());
        }
    }
}
