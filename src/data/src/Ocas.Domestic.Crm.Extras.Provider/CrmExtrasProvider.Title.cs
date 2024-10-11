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
        public Task<IList<Title>> GetTitles(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.TitlesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<Title>(Sprocs.TitlesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<Title> GetTitle(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.TitlesGet.Id, id },
                { Sprocs.TitlesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<Title>(Sprocs.TitlesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
