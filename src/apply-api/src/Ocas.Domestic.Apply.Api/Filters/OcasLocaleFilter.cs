using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Ocas.Domestic.Apply.Core;

namespace Ocas.Domestic.Apply.Api.Filters
{
    public class OcasLocaleFilter : IActionFilter
    {
        private readonly RequestCache _requestCache;
        private readonly ILogger<OcasLocaleFilter> _logger;

        private readonly int _maximumAcceptLanguageHeaderValuesToTry = 3;

        public OcasLocaleFilter(RequestCache requestCache, ILoggerFactory loggerFactory)
        {
            _requestCache = requestCache;
            _logger = loggerFactory.CreateLogger<OcasLocaleFilter>();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;

            if (httpContext == null)
            {
                throw new Exception("httpContext is null");
            }

            var fallbackCulture = CultureInfo.GetCultureInfo(Constants.Localization.FallbackLocalization);

            _requestCache.AddOrUpdate(fallbackCulture);

            var acceptLanguageHeader = httpContext.Request.GetTypedHeaders().AcceptLanguage;

            if (acceptLanguageHeader == null || acceptLanguageHeader.Count == 0)
            {
                return;
            }

            var languages = acceptLanguageHeader.AsEnumerable();

            if (_maximumAcceptLanguageHeaderValuesToTry > 0)
            {
                // We take only the first configured number of languages from the header and then order those that we
                // attempt to parse as a CultureInfo to mitigate potentially spinning CPU on lots of parse attempts.
                languages = languages.Take(_maximumAcceptLanguageHeaderValuesToTry);
            }

            var orderedLanguages = languages.OrderByDescending(h => h, StringWithQualityHeaderValueComparer.QualityComparer)
                .Select(x => x.Value).ToList();

            if (orderedLanguages.Count > 0)
            {
                var locale = orderedLanguages.FirstOrDefault(x => Constants.Localization.SupportedLocalizations.Contains(x.Value)).Value ?? Constants.Localization.FallbackLocalization;
                _requestCache.AddOrUpdate(CultureInfo.GetCultureInfo(locale));
            }
        }
    }
}
