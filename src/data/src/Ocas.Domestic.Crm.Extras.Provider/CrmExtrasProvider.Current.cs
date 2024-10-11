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
        public Task<IList<Current>> GetCurrents(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.CurrentsGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<Current>(Sprocs.CurrentsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<Current> GetCurrent(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.CurrentsGet.Id, id },
                { Sprocs.CurrentsGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<Current>(Sprocs.CurrentsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
