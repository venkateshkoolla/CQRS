using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<TranscriptRequestStatus> GetTranscriptRequestStatus(Guid transcriptRequestStatusId, Locale locale)
        {
            return CrmExtrasProvider.GetTranscriptRequestStatus(transcriptRequestStatusId, locale);
        }

        public Task<IList<TranscriptRequestStatus>> GetTranscriptRequestStatuses(Locale locale)
        {
            return CrmExtrasProvider.GetTranscriptRequestStatuses(locale);
        }
    }
}
