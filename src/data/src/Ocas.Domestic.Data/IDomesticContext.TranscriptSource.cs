using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<TranscriptSource> GetTranscriptSource(Guid transcriptSourceId, Locale locale);
        Task<IList<TranscriptSource>> GetTranscriptSources(Locale locale);
    }
}