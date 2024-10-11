using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<OstNote> GetOstNote(Guid ostNotesId, Locale locale);
        Task<IList<OstNote>> GetOstNotes(Locale locale);
    }
}