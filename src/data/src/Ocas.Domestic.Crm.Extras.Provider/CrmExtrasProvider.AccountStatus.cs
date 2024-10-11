using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial class CrmExtrasProvider
    {
        public Task<IList<AccountStatus>> GetAccountStatuses(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.AccountStatusesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<AccountStatus>(Sprocs.AccountStatusesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<AccountStatus> GetAccountStatus(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.AccountStatusesGet.Id, id },
                { Sprocs.AccountStatusesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<AccountStatus>(Sprocs.AccountStatusesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
