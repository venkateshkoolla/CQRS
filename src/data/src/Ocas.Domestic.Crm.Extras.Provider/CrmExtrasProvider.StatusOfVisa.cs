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
        public Task<IList<StatusOfVisa>> GetStatusOfVisas(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.StatusOfVisasGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<StatusOfVisa>(Sprocs.StatusOfVisasGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<StatusOfVisa> GetStatusOfVisa(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.StatusOfVisasGet.Id, id },
                { Sprocs.StatusOfVisasGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<StatusOfVisa>(Sprocs.StatusOfVisasGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
