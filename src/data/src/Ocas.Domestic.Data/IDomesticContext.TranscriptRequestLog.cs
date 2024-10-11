using System;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<TranscriptRequestLog> GetTranscriptRequestLog(Guid id);
        Task<TranscriptRequestLog> CreateTranscriptRequestLog(TranscriptRequestLogBase transcriptRequestLogBase);
        Task<TranscriptRequestLog> UpdateTranscriptRequestLog(TranscriptRequestLog transcriptRequestLog);
        Task DeleteTranscriptRequestLog(TranscriptRequestLog transcriptRequestLog);
        Task DeleteTranscriptRequestLog(Guid id);
    }
}
