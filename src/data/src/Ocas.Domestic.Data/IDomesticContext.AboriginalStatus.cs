using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<AboriginalStatus> GetAboriginalStatus(Guid aboriginalStatusId, Locale locale);
        Task<IList<AboriginalStatus>> GetAboriginalStatuses(Locale locale);
    }
}
