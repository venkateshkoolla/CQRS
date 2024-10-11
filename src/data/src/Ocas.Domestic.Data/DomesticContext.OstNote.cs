using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<OstNote> GetOstNote(Guid ostNotesId, Locale locale)
        {
            return CrmExtrasProvider.GetOstNote(ostNotesId, locale);
        }

        public Task<IList<OstNote>> GetOstNotes(Locale locale)
        {
            return CrmExtrasProvider.GetOstNotes(locale);
        }
    }
}