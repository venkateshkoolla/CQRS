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
        public Task<IList<FirstLanguage>> GetFirstLanguages(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.FirstLanguagesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<FirstLanguage>(Sprocs.FirstLanguagesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<FirstLanguage> GetFirstLanguage(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.FirstLanguagesGet.Id, id },
                { Sprocs.FirstLanguagesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<FirstLanguage>(Sprocs.FirstLanguagesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
