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
        public Task<IList<ProvinceState>> GetProvinceStates(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ProvinceStatesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<ProvinceState>(Sprocs.ProvinceStatesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<ProvinceState> GetProvinceState(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ProvinceStatesGet.Id, id },
                { Sprocs.ProvinceStatesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<ProvinceState>(Sprocs.ProvinceStatesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
