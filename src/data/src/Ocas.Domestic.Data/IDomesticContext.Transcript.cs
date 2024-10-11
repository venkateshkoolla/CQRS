using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<Transcript> GetTranscript(Guid id);
        Task<IList<Transcript>> GetTranscripts(GetTranscriptOptions options);
        Task<Transcript> CreateTranscript(TranscriptBase transcriptBase);
        Task<Transcript> UpdateTranscript(Transcript transcript);
        Task DeleteTranscript(Guid id);
    }
}
