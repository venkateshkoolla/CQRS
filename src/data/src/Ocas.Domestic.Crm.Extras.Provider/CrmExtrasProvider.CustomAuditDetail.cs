using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial class CrmExtrasProvider
    {
        public Task<CustomAuditDetail> GetCustomAuditDetail(Guid id)
        {
            var parameters = new Dictionary<string, object>
            {
               { Sprocs.CustomAuditDetailsGet.Id, id }
            };

            return Connection.QueryFirstOrDefaultAsync<CustomAuditDetail>(Sprocs.CustomAuditDetailsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }

        public Task<IList<CustomAuditDetail>> GetCustomAuditDetails(Guid auditId)
        {
            var parameters = new Dictionary<string, object>
            {
               { Sprocs.CustomAuditDetailsGet.AuditId, auditId }
            };

            return Connection.QueryAsync<CustomAuditDetail>(Sprocs.CustomAuditDetailsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }
    }
}
