using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<TranscriptTransmission> GetTranscriptTransmissionDate(Guid transcriptTransmissionDateId, Locale locale)
        {
            return CrmExtrasProvider.GetTranscriptTransmissionDate(transcriptTransmissionDateId, locale);
        }

        public Task<IList<TranscriptTransmission>> GetTranscriptTransmissionDates(Locale locale)
        {
            return CrmExtrasProvider.GetTranscriptTransmissionDates(locale);
        }
    }
}
