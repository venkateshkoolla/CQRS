using System;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<OrderDetail> GetOrderDetail(Guid orderDetailId)
        {
            return CrmExtrasProvider.GetOrderDetail(orderDetailId);
        }
    }
}