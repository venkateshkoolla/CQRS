using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<EntryLevel> GetEntryLevel(Guid entryLevelId, Locale locale);
        Task<IList<EntryLevel>> GetEntryLevels(Locale locale);
    }
}
