using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Api.Client
{
    public partial class ApplyApiClient
    {
        public Task<IList<ProgramChoice>> GetProgramChoices(Guid? applicationId = null, Guid? applicantId = null, bool isRemoved = false)
        {
            var queryParams = new Dictionary<string, string>
            {
                { nameof(applicationId), applicationId.ToString() },
                { "applicantId", applicantId.ToString() },
                { "isRemoved", isRemoved.ToString().ToLowerInvariant() }
            };
            return Get<IList<ProgramChoice>>(QueryHelpers.AddQueryString(Constants.Route.ProgramChoices, queryParams));
        }
    }
}
