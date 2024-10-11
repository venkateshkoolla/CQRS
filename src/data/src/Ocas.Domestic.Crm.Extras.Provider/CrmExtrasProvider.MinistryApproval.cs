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
        public Task<IList<MinistryApproval>> GetMinistryApprovals(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.MinistryApprovalsGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<MinistryApproval>(Sprocs.MinistryApprovalsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<MinistryApproval> GetMinistryApproval(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.MinistryApprovalsGet.Id, id },
                { Sprocs.MinistryApprovalsGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<MinistryApproval>(Sprocs.MinistryApprovalsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
