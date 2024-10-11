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
        public Task<IList<SchoolType>> GetSchoolTypes(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.SchoolTypesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<SchoolType>(Sprocs.SchoolTypesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<SchoolType> GetSchoolType(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.SchoolTypesGet.Id, id },
                { Sprocs.SchoolTypesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<SchoolType>(Sprocs.SchoolTypesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
