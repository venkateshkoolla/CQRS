using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<TranscriptRequestStatus> GetTranscriptRequestStatus(Guid transcriptRequestStatusId, Locale locale);

        Task<IList<TranscriptRequestStatus>> GetTranscriptRequestStatuses(Locale locale);
    }
}
