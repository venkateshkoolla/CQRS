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
        public Task<IList<GradeType>> GetGradeTypes(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.GradeTypesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<GradeType>(Sprocs.GradeTypesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<GradeType> GetGradeType(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.GradeTypesGet.Id, id },
                { Sprocs.GradeTypesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<GradeType>(Sprocs.GradeTypesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
