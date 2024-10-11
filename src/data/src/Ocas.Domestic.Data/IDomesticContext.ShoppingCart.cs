using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<ShoppingCart> GetShoppingCart(GetShoppingCartOptions options, Locale locale);
        Task DeleteShoppingCartDetail(Guid shoppingCartDetailId);
        Task<IList<ShoppingCartDetail>> GetShoppingCartDetails(GetShoppingCartDetailOptions options, Locale locale);
    }
}