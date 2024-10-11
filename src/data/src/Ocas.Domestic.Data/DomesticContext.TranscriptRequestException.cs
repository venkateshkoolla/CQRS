using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<TranscriptRequestException> GetTranscriptRequestException(Guid id, Locale locale)
        {
            return CrmExtrasProvider.GetTranscriptRequestException(id, locale);
        }

        public Task<IList<TranscriptRequestException>> GetTranscriptRequestExceptions(Locale locale)
        {
            return CrmExtrasProvider.GetTranscriptRequestExceptions(locale);
        }
    }
}
