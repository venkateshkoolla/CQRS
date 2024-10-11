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
        public Task<IList<AboriginalStatus>> GetAboriginalStatuses(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.AboriginalStatusesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<AboriginalStatus>(Sprocs.AboriginalStatusesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<AboriginalStatus> GetAboriginalStatus(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.AboriginalStatusesGet.Id, id },
                { Sprocs.AboriginalStatusesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<AboriginalStatus>(Sprocs.AboriginalStatusesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
