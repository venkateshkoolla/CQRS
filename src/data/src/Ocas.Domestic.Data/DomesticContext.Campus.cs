using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<Campus> GetCampus(Guid campusId)
        {
            return CrmExtrasProvider.GetAccount<Campus>(campusId, AccountType.Campus);
        }

        public Task<IList<Campus>> GetCampuses()
        {
            var campusOptions = new GetAccountsOptions
            {
                AccountType = AccountType.Campus
            };

            return CrmExtrasProvider.GetAccounts<Campus>(campusOptions);
        }

        public Task<IList<Campus>> GetCampuses(Guid collegeId)
        {
            var campusOptions = new GetAccountsOptions
            {
                AccountType = AccountType.Campus,
                ParentId = collegeId
            };

            return CrmExtrasProvider.GetAccounts<Campus>(campusOptions);
        }
    }
}
