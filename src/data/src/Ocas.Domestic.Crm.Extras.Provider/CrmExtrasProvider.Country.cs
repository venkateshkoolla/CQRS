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
        public Task<IList<Country>> GetCountries(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.CountriesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<Country>(Sprocs.CountriesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<Country> GetCountry(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.CountriesGet.Id, id },
                { Sprocs.CountriesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<Country>(Sprocs.CountriesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }

        public Task<Country> GetCountry(string name, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.CountriesGet.Locale, (int)locale },
                { Sprocs.CountriesGet.Name, name }
            };

            return Connection.QueryFirstOrDefaultAsync<Country>(Sprocs.CountriesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
