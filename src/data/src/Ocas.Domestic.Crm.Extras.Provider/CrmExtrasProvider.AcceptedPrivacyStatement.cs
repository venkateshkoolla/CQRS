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
        public Task<AcceptedPrivacyStatement> GetAcceptedPrivacyStatement(Guid id)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.PrivacyStatementsGet.Id, id },
                { Sprocs.PrivacyStatementsGet.StateCode, State.Active }
            };

            return Connection.QueryFirstOrDefaultAsync<AcceptedPrivacyStatement>(Sprocs.AcceptedPrivacyStatementsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
