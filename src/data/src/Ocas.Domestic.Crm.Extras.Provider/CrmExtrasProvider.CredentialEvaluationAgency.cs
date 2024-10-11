using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial class CrmExtrasProvider
    {
        public Task<IList<CredentialEvaluationAgency>> GetCredentialEvaluationAgencies()
        {
            return Connection.QueryAsync<CredentialEvaluationAgency, Address, CredentialEvaluationAgency>(
                Sprocs.CredentialEvaluationAgenciesGet.Sproc,
                (account, address) =>
                {
                    account.MailingAddress = address;
                    return account;
                },
                commandType: CommandType.StoredProcedure,
                commandTimeout: _commandTimeout,
                splitOn: "MailingAddressSplit")
                .QueryToListAsync();
        }

        public async Task<CredentialEvaluationAgency> GetCredentialEvaluationAgency(Guid id)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.CredentialEvaluationAgenciesGet.Id, id }
            };

            var result = await Connection.QueryAsync<CredentialEvaluationAgency, Address, CredentialEvaluationAgency>(
                Sprocs.CredentialEvaluationAgenciesGet.Sproc,
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
    }
}
