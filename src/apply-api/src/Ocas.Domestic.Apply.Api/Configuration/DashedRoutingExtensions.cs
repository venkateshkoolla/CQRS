using Microsoft.AspNetCore.Mvc;
using Ocas.Domestic.Apply.Api.Configuration;

namespace Ocas.Domestic.Apply.Api
{
    public static class DashedRoutingExtensions
    {
        public static void AddDashedRouting(this MvcOptions options)
        {
            options.Conventions.Add(new DashedRoutingConvention());
        }
    }
}
