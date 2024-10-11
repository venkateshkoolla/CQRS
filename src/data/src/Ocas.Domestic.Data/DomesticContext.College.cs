using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<College> GetCollege(Guid collegeId)
        {
            return CrmExtrasProvider.GetAccount<College>(collegeId, AccountType.College);
        }

        public Task<IList<College>> GetColleges()
        {
            var collegeOptions = new GetAccountsOptions
            {
                AccountType = AccountType.College
            };

            return CrmExtrasProvider.GetAccounts<College>(collegeOptions);
        }

        public Task<IList<College>> GetColleges(SchoolStatusCode statusCode)
        {
            var collegeOptions = new GetAccountsOptions
            {
                AccountType = AccountType.College,
                SchoolStatusCode = statusCode
            };

            return CrmExtrasProvider.GetAccounts<College>(collegeOptions);
        }
    }
}
