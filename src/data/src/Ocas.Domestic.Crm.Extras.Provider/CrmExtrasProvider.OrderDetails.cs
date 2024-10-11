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
        public Task<OrderDetail> GetOrderDetail(Guid id)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.OrdersGet.Id, id }
            };

            return Connection.QueryFirstOrDefaultAsync<OrderDetail>(Sprocs.OrderDetailsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
