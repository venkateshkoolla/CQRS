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
        public Task<IList<PreferredCorrespondenceMethod>> GetPreferredCorrespondenceMethods(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.PreferredCorrespondenceMethodsGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<PreferredCorrespondenceMethod>(Sprocs.PreferredCorrespondenceMethodsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<PreferredCorrespondenceMethod> GetPreferredCorrespondenceMethod(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.PreferredCorrespondenceMethodsGet.Id, id },
                { Sprocs.PreferredCorrespondenceMethodsGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<PreferredCorrespondenceMethod>(Sprocs.PreferredCorrespondenceMethodsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
