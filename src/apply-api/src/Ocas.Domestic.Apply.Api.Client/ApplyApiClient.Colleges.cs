using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Ocas.Domestic.Apply.Enums;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Api.Client
{
    public partial class ApplyApiClient
    {
        public Task<IList<College>> GetColleges()
        {
            return Get<IList<College>>(Constants.Route.Colleges);
        }

        public Task<CollegeTemplate> GetTemplate(Guid collegeId, CollegeTemplateKey key)
        {
            return Get<CollegeTemplate>(QueryHelpers.AddQueryString($"{Constants.Route.Colleges}/{collegeId}/template", "key", key.ToString()));
        }
    }
}
