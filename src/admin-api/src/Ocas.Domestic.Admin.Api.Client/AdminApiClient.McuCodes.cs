using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Ocas.Domestic.Apply.Admin.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Client
{
    public partial class AdminApiClient
    {
        public Task<PagedResult<McuCode>> GetMcuCodes(GetMcuCodeOptions options)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "sortBy", options.SortBy.ToString() }
            };

            if (options.Search != null) queryParams.Add("search", options.Search);
            if (options.Page != null) queryParams.Add("page", options.Page.ToString());
            if (options.PageSize != null) queryParams.Add("pageSize", options.PageSize.ToString());
            if (options.SortDirection != null) queryParams.Add("sortDirection", options.SortDirection.ToString());

            return Get<PagedResult<McuCode>>(QueryHelpers.AddQueryString(Constants.Route.McuCodes, queryParams));
        }

        public Task<McuCode> GetMcuCode(string code)
        {
            return Get<McuCode>($"{Constants.Route.McuCodes}/{code}");
        }
    }
}
