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
        public Task<IList<ProgramCategory>> GetProgramCategories(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ProgramCategoriesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<ProgramCategory>(Sprocs.ProgramCategoriesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<ProgramCategory> GetProgramCategory(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ProgramCategoriesGet.Id, id },
                { Sprocs.ProgramCategoriesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<ProgramCategory>(Sprocs.ProgramCategoriesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
