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
        public Task<IList<McuCode>> GetMcuCodes(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.McuCodesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<McuCode>(Sprocs.McuCodesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<McuCode> GetMcuCode(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.McuCodesGet.Id, id },
                { Sprocs.McuCodesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<McuCode>(Sprocs.McuCodesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
