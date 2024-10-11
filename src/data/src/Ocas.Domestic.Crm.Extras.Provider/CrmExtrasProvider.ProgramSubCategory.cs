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
        public Task<IList<ProgramSubCategory>> GetProgramSubCategories(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ProgramSubCategoriesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<ProgramSubCategory>(Sprocs.ProgramSubCategoriesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<IList<ProgramSubCategory>> GetProgramSubCategories(Guid programCategoryId, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ProgramSubCategoriesGet.Locale, (int)locale },
                { Sprocs.ProgramSubCategoriesGet.ProgramCategoryId, programCategoryId }
            };

            return Connection.QueryAsync<ProgramSubCategory>(Sprocs.ProgramSubCategoriesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<ProgramSubCategory> GetProgramSubCategory(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ProgramSubCategoriesGet.Id, id },
                { Sprocs.ProgramSubCategoriesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<ProgramSubCategory>(Sprocs.ProgramSubCategoriesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
