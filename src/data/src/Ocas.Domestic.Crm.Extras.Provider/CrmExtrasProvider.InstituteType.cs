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
        public Task<IList<InstituteType>> GetInstituteTypes(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.InstituteTypesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<InstituteType>(Sprocs.InstituteTypesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<InstituteType> GetInstituteType(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.InstituteTypesGet.Id, id },
                { Sprocs.InstituteTypesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<InstituteType>(Sprocs.InstituteTypesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
