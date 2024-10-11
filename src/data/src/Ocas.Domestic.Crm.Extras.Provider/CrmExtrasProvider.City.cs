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
        public Task<IList<City>> GetCities(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.CitiesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<City>(Sprocs.CitiesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<City> GetCity(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.CitiesGet.Id, id },
                { Sprocs.CitiesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<City>(Sprocs.CitiesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
