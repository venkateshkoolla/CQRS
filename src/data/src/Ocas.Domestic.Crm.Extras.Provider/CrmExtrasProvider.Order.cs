using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial class CrmExtrasProvider
    {
        public Task<Order> GetOrder(Guid id)
        {
            return GetOrder(new GetOrderOptions
            {
                Id = id
            });
        }

        public Task<Order> GetOrder(string orderNumber)
        {
            return GetOrder(new GetOrderOptions
            {
                OrderNumber = orderNumber
            });
        }

        public async Task<IList<Order>> GetOrders(GetOrderOptions options)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.OrdersGet.CustomerId, options.CustomerId }
            };

            // https://dapper-tutorial.net/result-multi-mapping#example-query-multi-mapping-one-to-many
            var resultDictionary = new Dictionary<Guid, Order>();

            await Connection.QueryAsync<Order, OrderDetail, Order>(
                Sprocs.OrdersGet.Sproc,
                (master, detail) =>
                {
                    if (!resultDictionary.TryGetValue(master.Id, out var tempMaster))
                    {
                        tempMaster = master;
                        tempMaster.Details = new List<OrderDetail>();
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

            return resultDictionary.Values.ToList();
        }

        private async Task<Order> GetOrder(GetOrderOptions options)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.OrdersGet.Id, options.Id },
                { Sprocs.OrdersGet.OrderNumber, options.OrderNumber }
            };

            // https://dapper-tutorial.net/result-multi-mapping#example-query-multi-mapping-one-to-many
            var resultDictionary = new Dictionary<Guid, Order>();

            await Connection.QueryAsync<Order, OrderDetail, Order>(
                Sprocs.OrdersGet.Sproc,
                (master, detail) =>
                {
                    if (!resultDictionary.TryGetValue(master.Id, out var tempMaster))
                    {
                        tempMaster = master;
                        tempMaster.Details = new List<OrderDetail>();
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
