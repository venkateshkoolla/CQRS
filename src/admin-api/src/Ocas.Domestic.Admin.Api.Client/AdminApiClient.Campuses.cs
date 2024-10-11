using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Client
{
    public partial class AdminApiClient
    {
        public Task<IList<Campus>> GetCampuses(Guid collegeId)
        {
            return Get<IList<Campus>>(QueryHelpers.AddQueryString(Constants.Route.Campuses, "collegeId", collegeId.ToString()));
        }
    }
}
