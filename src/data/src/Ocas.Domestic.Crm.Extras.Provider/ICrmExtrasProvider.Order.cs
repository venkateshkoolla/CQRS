using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial interface ICrmExtrasProvider
    {
        Task<Order> GetOrder(Guid id);
        Task<Order> GetOrder(string orderNumber);
        Task<IList<Order>> GetOrders(GetOrderOptions options);
    }
}
