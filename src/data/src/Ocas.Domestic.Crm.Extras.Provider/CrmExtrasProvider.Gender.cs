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
        public Task<IList<Gender>> GetGenders(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.GendersGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<Gender>(Sprocs.GendersGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<Gender> GetGender(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.GendersGet.Id, id },
                { Sprocs.GendersGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<Gender>(Sprocs.GendersGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
