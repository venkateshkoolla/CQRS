using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial class CrmExtrasProvider
    {
        public async Task<ShoppingCart> GetShoppingCart(GetShoppingCartOptions options, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ShoppingCartsGet.Id, options.Id },
                { Sprocs.ShoppingCartsGet.ApplicationId, options.ApplicationId },
                { Sprocs.ShoppingCartsGet.Locale, locale }
            };

            // https://dapper-tutorial.net/result-multi-mapping#example-query-multi-mapping-one-to-many
            var resultDictionary = new Dictionary<Guid, ShoppingCart>();

            await Connection.QueryAsync<ShoppingCart, ShoppingCartDetail, ShoppingCart>(
                Sprocs.ShoppingCartsGet.Sproc,
                (master, detail) =>
                {
                    if (!resultDictionary.TryGetValue(master.Id, out var tempMaster))
                    {
                        tempMaster = master;
                        tempMaster.Details = new List<ShoppingCartDetail>();
                        resultDictionary.Add(tempMaster.Id, tempMaster);
                    }

                    if (detail != null)
                    {
                        tempMaster.Details.Add(detail);
                    }

                    return tempMaster;
                },
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: _commandTimeout);

            return resultDictionary.Values.SingleOrDefault();
        }
    }
}
