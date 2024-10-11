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
        public Task<IList<OfferAcceptance>> GetOfferAcceptances(GetOfferAcceptancesOptions offerAcceptancesOptions, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.OfferAcceptancesGet.Id, offerAcceptancesOptions.Id },
                { Sprocs.OfferAcceptancesGet.Locale, (int)locale },
                { Sprocs.OfferAcceptancesGet.StateCode, offerAcceptancesOptions.StateCode },
                { Sprocs.OfferAcceptancesGet.StatusCode, offerAcceptancesOptions.StatusCode },
                { Sprocs.OfferAcceptancesGet.ApplicationId, offerAcceptancesOptions.ApplicationId }
            };

            return Connection.QueryAsync<OfferAcceptance>(Sprocs.OfferAcceptancesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<OfferAcceptance> GetOfferAcceptance(Guid offerAcceptanceId, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.OfferAcceptancesGet.Id, offerAcceptanceId },
                { Sprocs.OfferAcceptancesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<OfferAcceptance>(Sprocs.OfferAcceptancesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}