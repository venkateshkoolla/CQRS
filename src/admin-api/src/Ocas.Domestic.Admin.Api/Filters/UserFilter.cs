using System;
using System.Linq;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Ocas.Domestic.Apply.Admin.Core.Settings;

namespace Ocas.Domestic.Apply.Admin.Api.Filters
{
    public class UserFilter : IActionFilter
    {
        private readonly ILogger<UserFilter> _logger;
        private readonly RequestCache _requestCache;
        private readonly IAppSettings _appSettings;

        public UserFilter(ILoggerFactory loggerFactory, RequestCache requestCache, IAppSettings appSettings)
        {
            _logger = loggerFactory.CreateLogger<UserFilter>();
            _requestCache = requestCache;
            _appSettings = appSettings;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext ??
                throw new Exception("httpContext is null");

            if (httpContext.User != null)
            {
                _requestCache.AddOrUpdate<IPrincipal>(httpContext.User);
                _requestCache.AddOrUpdate(Constants.RequestCacheKeys.UserIsOcas, _appSettings.IdSvrRolesOcasUser.Any(x => httpContext.User.IsInRole(x)));
            }
            else
            {
                _requestCache.AddOrUpdate(Constants.RequestCacheKeys.UserIsOcas, false);
            }
        }
    }
}
