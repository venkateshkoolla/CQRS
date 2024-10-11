using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<AccountStatus> GetAccountStatus(Guid accountStatusId, Locale locale)
        {
            return CrmExtrasProvider.GetAccountStatus(accountStatusId, locale);
        }

        public Task<IList<AccountStatus>> GetAccountStatuses(Locale locale)
        {
            return CrmExtrasProvider.GetAccountStatuses(locale);
        }
    }
}
