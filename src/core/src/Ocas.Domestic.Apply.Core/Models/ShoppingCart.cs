using System.Collections.Generic;

namespace Ocas.Domestic.Apply.Models
{
    public class ShoppingCart
    {
        public IList<ShoppingCartDetail> Details { get; set; }
    }
}
