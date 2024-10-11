using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<HighlyCompetitive> GetHighlyCompetitive(Guid highlyCompetitiveId, Locale locale);
        Task<IList<HighlyCompetitive>> GetHighlyCompetitives(Locale locale);
    }
}
