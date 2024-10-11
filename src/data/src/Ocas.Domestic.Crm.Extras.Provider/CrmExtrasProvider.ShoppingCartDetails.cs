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
        public Task<IList<ShoppingCartDetail>> GetShoppingCartDetails(GetShoppingCartDetailOptions options, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ShoppingCartDetailsGet.Id, options.Id },
                { Sprocs.ShoppingCartDetailsGet.ApplicationId, options.ApplicationId },
                { Sprocs.ShoppingCartDetailsGet.ApplicantId, options.ApplicantId },
                { Sprocs.ShoppingCartDetailsGet.Locale, locale }
            };

            return Connection.QueryAsync<ShoppingCartDetail>(Sprocs.ShoppingCartDetailsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }
    }
}
