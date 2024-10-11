using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<Order> GetOrder(Guid id);
        Task<Order> GetOrder(string orderNumber);
        Task<IList<Order>> GetOrders(GetOrderOptions options);
        Task<Order> CreateOrder(Guid applicationId, Guid applicantId, string modifiedBy, Guid sourceId, ShoppingCart shoppingCart);
        Task<OrderDetail> CreateOrderDetail(Order order, ShoppingCartDetail shoppingCartItem);
        Task<Order> UpdateOrder(Order model);
        Task DeleteOrderDetail(Guid orderDetailId);
    }
}
