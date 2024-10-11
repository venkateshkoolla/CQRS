using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial class CrmExtrasProvider
    {
        public async Task<T> GetAccount<T>(Guid id, AccountType accountType)
            where T : IAccount
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.AccountsGet.Id, id },
                { Sprocs.AccountsGet.AccountType, (int)accountType },
                { Sprocs.AccountsGet.StateCode, null }
            };

            var result = await Connection.QueryAsync<T, Address, T>(
                Sprocs.AccountsGet.Sproc,
                (account, address) =>
                {
                    account.MailingAddress = address;
                    return account;
                },
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: _commandTimeout,
                splitOn: "MailingAddressSplit");

            return result.FirstOrDefault();
        }

        public Task<IList<T>> GetAccounts<T>(GetAccountsOptions accountsOptions)
            where T : IAccount
        {
            char? collegeStatusCode = null;
            if (accountsOptions.SchoolStatusCode.HasValue)
                collegeStatusCode = (char)accountsOptions.SchoolStatusCode.Value;

            var parameters = new Dictionary<string, object>
            {
                { Sprocs.AccountsGet.AccountType, (int)accountsOptions.AccountType },
                { Sprocs.AccountsGet.CollegeStatusCode, collegeStatusCode },
                { Sprocs.AccountsGet.StateCode, (int)accountsOptions.StateCode },
                { Sprocs.AccountsGet.ParentId, accountsOptions.ParentId }
            };

            return Connection.QueryAsync<T, Address, T>(
                Sprocs.AccountsGet.Sproc,
                (account, address) =>
                {
                    account.MailingAddress = address;
                    return account;
                },
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: _commandTimeout,
                splitOn: "MailingAddressSplit").QueryToListAsync();
        }
    }
}
