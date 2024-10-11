using Microsoft.AspNetCore.Mvc;
using Ocas.Domestic.Apply.Admin.Api.Configuration;

namespace Ocas.Domestic.Apply.Admin.Api
{
    public static class DashedRoutingExtensions
    {
        public static void AddDashedRouting(this MvcOptions options)
        {
            options.Conventions.Add(new DashedRoutingConvention());
        }
    }
}
