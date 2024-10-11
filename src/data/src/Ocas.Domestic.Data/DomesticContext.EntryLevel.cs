using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<EntryLevel> GetEntryLevel(Guid entryLevelId, Locale locale)
        {
            return CrmExtrasProvider.GetEntryLevel(entryLevelId, locale);
        }

        public Task<IList<EntryLevel>> GetEntryLevels(Locale locale)
        {
            return CrmExtrasProvider.GetEntryLevels(locale);
        }
    }
}
