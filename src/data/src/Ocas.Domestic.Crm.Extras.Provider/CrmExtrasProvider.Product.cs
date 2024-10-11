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
        public Task<IList<Product>> GetProducts(ProductServiceType productServiceType)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ProductsGet.ServiceType, productServiceType }
            };

            return Connection.QueryAsync<Product>(Sprocs.ProductsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<Product> GetProduct(Guid id)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ProductsGet.Id, id }
            };

            return Connection.QueryFirstOrDefaultAsync<Product>(Sprocs.ProductsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
