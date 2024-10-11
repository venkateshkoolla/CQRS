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
        public Task<IList<OfferState>> GetOfferStates(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.OfferStatesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<OfferState>(Sprocs.OfferStatesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<OfferState> GetOfferState(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.OfferStatesGet.Id, id },
                { Sprocs.OfferStatesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<OfferState>(Sprocs.OfferStatesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
