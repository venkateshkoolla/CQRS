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
        public Task<IList<OfferType>> GetOfferTypes(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.OfferTypesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<OfferType>(Sprocs.OfferTypesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<OfferType> GetOfferType(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.OfferTypesGet.Id, id },
                { Sprocs.OfferTypesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<OfferType>(Sprocs.OfferTypesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
