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
        public Task<IList<Source>> GetSources(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.SourcesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<Source>(Sprocs.SourcesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<Source> GetSource(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.SourcesGet.Id, id },
                { Sprocs.SourcesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<Source>(Sprocs.SourcesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
