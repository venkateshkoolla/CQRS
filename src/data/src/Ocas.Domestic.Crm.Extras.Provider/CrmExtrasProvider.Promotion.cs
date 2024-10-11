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
        public Task<IList<Promotion>> GetPromotions(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.PromotionsGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<Promotion>(Sprocs.PromotionsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<Promotion> GetPromotion(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.PromotionsGet.Id, id },
                { Sprocs.PromotionsGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<Promotion>(Sprocs.PromotionsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
