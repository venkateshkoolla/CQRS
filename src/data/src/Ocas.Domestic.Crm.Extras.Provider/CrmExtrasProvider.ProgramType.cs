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
        public Task<IList<ProgramType>> GetProgramTypes(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                {
                    Sprocs.ProgramTypesGet.Locale, (int)locale
                }
            };

            return Connection.QueryAsync<ProgramType>(Sprocs.ProgramTypesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<ProgramType> GetProgramType(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ProgramTypesGet.Id, id },
                { Sprocs.ProgramTypesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<ProgramType>(Sprocs.ProgramTypesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
