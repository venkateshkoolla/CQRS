using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Api.Client
{
    public partial class ApplyApiClient
    {
        public Task<IList<TranscriptRequest>> GetTranscriptRequests(Guid? applicationId, Guid? applicantId = null)
        {
            var parameters = new Dictionary<string, string>
            {
                { "applicantId", applicantId.ToString() },
                { "applicationId", applicationId.ToString() }
            };

            return Get<IList<TranscriptRequest>>(QueryHelpers.AddQueryString(Constants.Route.TranscriptRequests, parameters));
        }

        public Task<IList<TranscriptRequest>> CreateTranscriptRequests(IList<TranscriptRequestBase> transcriptRequests)
        {
            return Post<IList<TranscriptRequest>>(Constants.Route.TranscriptRequests, transcriptRequests);
        }

        public Task DeleteTranscriptRequest(Guid transcriptRequestId)
        {
            return Delete($"{Constants.Route.TranscriptRequests}/{transcriptRequestId}");
        }

        public Task<IList<TranscriptRequest>> ReissueTranscriptRequest(Guid transcriptRequestId)
        {
            return Post<IList<TranscriptRequest>>($"{Constants.Route.TranscriptRequests}/{transcriptRequestId}/reissue");
        }
    }
}
