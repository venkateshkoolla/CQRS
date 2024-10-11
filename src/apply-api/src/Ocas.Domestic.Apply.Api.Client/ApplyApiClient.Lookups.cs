using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Api.Client
{
    public partial class ApplyApiClient
    {
        public Task<AllLookups> GetLookups()
        {
            return Get<AllLookups>(Constants.Route.Lookups);
        }

        public Task<AllLookups> GetLookups(params string[] filters)
        {
            return Get<AllLookups>(QueryHelpers.AddQueryString(Constants.Route.Lookups, "filter", string.Join(",", filters)));
        }

        public Task<AllLookups> PurgeLookups()
        {
            return Get<AllLookups>($"{Constants.Route.Lookups}/purge");
        }

        public Task<AllLookups> PurgeLookups(params string[] filters)
        {
            return Get<AllLookups>(QueryHelpers.AddQueryString($"{Constants.Route.Lookups}/purge", "filter", string.Join(",", filters)));
        }
    }
}
