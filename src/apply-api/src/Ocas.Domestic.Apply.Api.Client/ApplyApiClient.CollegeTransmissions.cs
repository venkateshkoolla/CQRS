using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Api.Client
{
    public partial class ApplyApiClient
    {
        public Task<IList<CollegeTransmission>> GetCollegeTransmissions(Guid applicationId)
        {
            return Get<IList<CollegeTransmission>>(QueryHelpers.AddQueryString(Constants.Route.CollegeTransmissions, "applicationId", applicationId.ToString()));
        }

        public Task<DateTime?> NextSendDate()
        {
            return Get<DateTime?>($"{Constants.Route.CollegeTransmissions}/{Constants.Actions.NextSendDate}");
        }
    }
}
