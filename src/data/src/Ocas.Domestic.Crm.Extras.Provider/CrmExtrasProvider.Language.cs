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
        public Task<IList<Language>> GetLanguages(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.LanguagesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<Language>(Sprocs.LanguagesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<Language> GetLanguage(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.LanguagesGet.Id, id },
                { Sprocs.LanguagesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<Language>(Sprocs.LanguagesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
