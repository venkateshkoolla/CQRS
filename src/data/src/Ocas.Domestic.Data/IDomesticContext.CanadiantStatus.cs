using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<CanadianStatus> GetCanadianStatus(Guid canadianStatusId, Locale locale);
        Task<IList<CanadianStatus>> GetCanadianStatuses(Locale locale);
    }
}
