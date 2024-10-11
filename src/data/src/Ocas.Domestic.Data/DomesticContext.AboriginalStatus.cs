using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<AboriginalStatus> GetAboriginalStatus(Guid aboriginalStatusId, Locale locale)
        {
            return CrmExtrasProvider.GetAboriginalStatus(aboriginalStatusId, locale);
        }

        public Task<IList<AboriginalStatus>> GetAboriginalStatuses(Locale locale)
        {
            return CrmExtrasProvider.GetAboriginalStatuses(locale);
        }
    }
}
