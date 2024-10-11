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
        public Task<IList<CanadianStatus>> GetCanadianStatuses(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.CanadianStatusesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<CanadianStatus>(Sprocs.CanadianStatusesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<CanadianStatus> GetCanadianStatus(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.CanadianStatusesGet.Id, id },
                { Sprocs.CanadianStatusesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<CanadianStatus>(Sprocs.CanadianStatusesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
