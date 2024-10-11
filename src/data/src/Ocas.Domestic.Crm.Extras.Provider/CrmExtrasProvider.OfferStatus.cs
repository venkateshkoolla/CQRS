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
        public Task<IList<OfferStatus>> GetOfferStatuses(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.OfferStatusesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<OfferStatus>(Sprocs.OfferStatusesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<OfferStatus> GetOfferStatus(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.OfferStatusesGet.Id, id },
                { Sprocs.OfferStatusesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<OfferStatus>(Sprocs.OfferStatusesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
