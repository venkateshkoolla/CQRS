using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<TranscriptRequest> GetTranscriptRequest(Guid id);
        Task<IList<TranscriptRequest>> GetTranscriptRequests(GetTranscriptRequestOptions options);
        Task<TranscriptRequest> CreateTranscriptRequest(TranscriptRequestBase transcriptRequestBase);
        Task<TranscriptRequest> UpdateTranscriptRequest(TranscriptRequest transcriptRequest);
        Task DeleteTranscriptRequest(TranscriptRequest transcriptRequest);
        Task DeleteTranscriptRequest(Guid id);
    }
}
