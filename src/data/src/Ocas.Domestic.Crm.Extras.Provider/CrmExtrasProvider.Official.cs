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
        public Task<Official> GetOfficial(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.OfficialsGet.Id, id },
                { Sprocs.OfficialsGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<Official>(Sprocs.OfficialsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }

        public Task<IList<Official>> GetOfficials(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.OfficialsGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<Official>(Sprocs.OfficialsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }
    }
}
