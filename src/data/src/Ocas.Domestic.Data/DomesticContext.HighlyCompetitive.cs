using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<HighlyCompetitive> GetHighlyCompetitive(Guid highlyCompetitiveId, Locale locale)
        {
            return CrmExtrasProvider.GetHighlyCompetitive(highlyCompetitiveId, locale);
        }

        public Task<IList<HighlyCompetitive>> GetHighlyCompetitives(Locale locale)
        {
            return CrmExtrasProvider.GetHighlyCompetitives(locale);
        }
    }
}
