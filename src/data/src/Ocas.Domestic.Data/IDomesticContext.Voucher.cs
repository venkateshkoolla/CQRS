using System;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<Voucher> GetVoucher(GetVoucherOptions options);
        Task AddVoucherToShoppingCart(Voucher model, Guid applicationId, Guid applicantId, Guid shoppingCartId, Guid sourceId, string modifiedBy);
        Task RemoveVoucherFromShoppingCart(Voucher model, string modifiedBy);
        Task<Voucher> UpdateVoucher(Voucher voucher);
        Task<Voucher> LinkOrderToVoucher(Voucher voucher);
    }
}
