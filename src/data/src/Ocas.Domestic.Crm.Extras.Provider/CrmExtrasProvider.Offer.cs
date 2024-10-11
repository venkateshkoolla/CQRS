using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial class CrmExtrasProvider
    {
        public Task<IList<Offer>> GetOffers(GetOfferOptions options)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.OffersGet.ApplicantId, options.ApplicantId },
                { Sprocs.OffersGet.ApplicationId, options.ApplicationId },
                { Sprocs.OffersGet.ApplicationStatusId, options.ApplicationStatusId }
            };

            return Connection.QueryAsync<Offer>(Sprocs.OffersGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<Offer> GetOffer(Guid offerId)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.OffersGet.Id, offerId }
            };

            return Connection.QueryFirstOrDefaultAsync<Offer>(Sprocs.OffersGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
