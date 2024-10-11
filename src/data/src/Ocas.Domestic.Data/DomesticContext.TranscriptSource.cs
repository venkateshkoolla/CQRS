using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<TranscriptSource> GetTranscriptSource(Guid transcriptSourceId, Locale locale)
        {
            return CrmExtrasProvider.GetTranscriptSource(transcriptSourceId, locale);
        }

        public Task<IList<TranscriptSource>> GetTranscriptSources(Locale locale)
        {
            return CrmExtrasProvider.GetTranscriptSources(locale);
        }
    }
}