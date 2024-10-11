using Ocas.Domestic.Apply.Enums;

namespace Ocas.Domestic.Apply.Models
{
    public class ShoppingCartDetail
    {
        public ShoppingCartItemType Type { get; set; }
        public string ContextId { get; set; }
        public decimal? Amount { get; set; }
    }
}
