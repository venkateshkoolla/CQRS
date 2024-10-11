using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Api.Client
{
    public partial class ApplyApiClient
    {
        public Task<IList<ApplicantMessage>> GetApplicantMessages(Guid applicantId, DateTimeOffset? after = null)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "applicantId", applicantId.ToString() }
            };

            if (after.HasValue) queryParams.Add("after", after.Value.UtcDateTime.ToString("u"));

            return Get<IList<ApplicantMessage>>(QueryHelpers.AddQueryString(Constants.Route.Messages, queryParams));
        }

        public Task ReadApplicantMessage(Guid id)
        {
            return Post($"{Constants.Route.Messages}/{id.ToString()}/{Constants.Actions.Read}");
        }
    }
}
