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
        public Task<IList<ProgramLanguage>> GetProgramLanguages(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ProgramLanguagesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<ProgramLanguage>(Sprocs.ProgramLanguagesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<ProgramLanguage> GetProgramLanguage(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ProgramLanguagesGet.Id, id },
                { Sprocs.ProgramLanguagesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<ProgramLanguage>(Sprocs.ProgramLanguagesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
