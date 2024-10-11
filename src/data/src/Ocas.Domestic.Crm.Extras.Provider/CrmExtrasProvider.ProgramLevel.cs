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
        public Task<IList<ProgramLevel>> GetProgramLevels(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ProgramLevelsGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<ProgramLevel>(Sprocs.ProgramLevelsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<ProgramLevel> GetProgramLevel(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ProgramLevelsGet.Id, id },
                { Sprocs.ProgramLevelsGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<ProgramLevel>(Sprocs.ProgramLevelsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
