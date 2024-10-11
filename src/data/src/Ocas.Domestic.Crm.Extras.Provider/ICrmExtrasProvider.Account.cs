using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial interface ICrmExtrasProvider
    {
        Task<T> GetAccount<T>(Guid id, AccountType accountType)
            where T : IAccount;

        Task<IList<T>> GetAccounts<T>(GetAccountsOptions accountsOptions)
            where T : IAccount;
    }
}
