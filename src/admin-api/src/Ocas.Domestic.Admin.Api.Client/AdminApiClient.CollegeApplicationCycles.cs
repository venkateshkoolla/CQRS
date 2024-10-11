using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Client
{
    public partial class AdminApiClient
    {
        public Task<IList<CollegeApplicationCycle>> GetCollegeApplicationCycles(Guid collegeId)
        {
            return Get<IList<CollegeApplicationCycle>>(QueryHelpers.AddQueryString(Constants.Route.CollegeApplicationCycles, "collegeId", collegeId.ToString()));
        }

        public Task<SpecialCode> GetSpecialCode(Guid collegeApplicationCycleId, string code)
        {
            return Get<SpecialCode>($"{Constants.Route.CollegeApplicationCycles}/{collegeApplicationCycleId}/special-codes/{code}");
        }

        public Task<PagedResult<SpecialCode>> GetSpecialCodes(Guid collegeApplicationCycleId, GetSpecialCodeOptions options)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "sortBy", options.SortBy.ToString() }
            };

            if (options.Search != null) queryParams.Add("search", options.Search);
            if (options.Page != null) queryParams.Add("page", options.Page.ToString());
            if (options.PageSize != null) queryParams.Add("pageSize", options.PageSize.ToString());
            if (options.SortDirection != null) queryParams.Add("sortDirection", options.SortDirection.ToString());

            return Get<PagedResult<SpecialCode>>(QueryHelpers.AddQueryString($"{Constants.Route.CollegeApplicationCycles}/{collegeApplicationCycleId}/special-codes", queryParams));
        }
    }
}
