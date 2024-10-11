using System;
using System.Threading.Tasks;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Api.Client
{
    public partial class ApplyApiClient
    {
        public Task<Order> GetOrder(Guid orderId)
        {
            return Get<Order>($"{Constants.Route.Orders}/{orderId.ToString()}");
        }

        public Task<FinancialTransaction> PayOrder(Guid orderId, PayOrderInfo payOrderInfo = null)
        {
            return Post<FinancialTransaction>($"{Constants.Route.Orders}/{orderId.ToString()}/{Constants.Actions.Pay}", payOrderInfo ?? new PayOrderInfo());
        }

        public Task<Order> CreateOrder(CreateOrderInfo createOrderInfo)
        {
            return Post<Order>(Constants.Route.Orders, createOrderInfo);
        }
    }
}
