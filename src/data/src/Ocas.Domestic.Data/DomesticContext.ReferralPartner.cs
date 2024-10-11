using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<ReferralPartner> GetReferralPartner(Guid referralPartnerId)
        {
            return CrmExtrasProvider.GetAccount<ReferralPartner>(referralPartnerId, AccountType.Vendor);
        }

        public Task<IList<ReferralPartner>> GetReferralPartners()
        {
            var options = new GetAccountsOptions
            {
                AccountType = AccountType.Vendor
            };

            return CrmExtrasProvider.GetAccounts<ReferralPartner>(options);
        }
    }
}
