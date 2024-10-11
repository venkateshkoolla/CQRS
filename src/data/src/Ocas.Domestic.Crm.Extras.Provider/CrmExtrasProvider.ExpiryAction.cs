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
        public Task<ExpiryAction> GetExpiryAction(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ExpiryActionsGet.Id, id },
                { Sprocs.ExpiryActionsGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<ExpiryAction>(Sprocs.ExpiryActionsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }

        public Task<IList<ExpiryAction>> GetExpiryActions(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ExpiryActionsGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<ExpiryAction>(Sprocs.ExpiryActionsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }
    }
}
