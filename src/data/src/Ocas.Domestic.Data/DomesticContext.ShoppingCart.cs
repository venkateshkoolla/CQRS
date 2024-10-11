using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<ShoppingCart> GetShoppingCart(GetShoppingCartOptions options, Locale locale)
        {
            return CrmExtrasProvider.GetShoppingCart(options, locale);
        }

        public Task DeleteShoppingCartDetail(Guid shoppingCartDetailId)
        {
            return CrmProvider.DeleteEntity(Crm.Entities.OpportunityProduct.EntityLogicalName, shoppingCartDetailId);
        }

        public Task<IList<ShoppingCartDetail>> GetShoppingCartDetails(GetShoppingCartDetailOptions options, Locale locale)
        {
            return CrmExtrasProvider.GetShoppingCartDetails(options, locale);
        }
    }
}
