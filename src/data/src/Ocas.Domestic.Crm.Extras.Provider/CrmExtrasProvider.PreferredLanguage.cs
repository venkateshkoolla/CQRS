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
        public Task<IList<PreferredLanguage>> GetPreferredLanguages(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.PreferredLanguagesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<PreferredLanguage>(Sprocs.PreferredLanguagesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<PreferredLanguage> GetPreferredLanguage(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.PreferredLanguagesGet.Id, id },
                { Sprocs.PreferredLanguagesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<PreferredLanguage>(Sprocs.PreferredLanguagesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
