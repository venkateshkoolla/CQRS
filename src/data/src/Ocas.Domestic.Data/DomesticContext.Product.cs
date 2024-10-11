using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<Product> GetProduct(Guid productId)
        {
            return CrmExtrasProvider.GetProduct(productId);
        }

        public Task<IList<Product>> GetProducts(ProductServiceType productServiceType)
        {
            return CrmExtrasProvider.GetProducts(productServiceType);
        }
    }
}
