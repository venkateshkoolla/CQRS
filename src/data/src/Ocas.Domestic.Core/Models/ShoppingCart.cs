using System;
using System.Collections.Generic;

namespace Ocas.Domestic.Models
{
    public class ShoppingCart
    {
        public Guid Id { get; set; }
        public Guid ApplicantId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid ApplicationCycleStatusId { get; set; }
        public Guid PriceLevelId { get; set; }
        public Guid TransactionCurrencyId { get; set; }
        public IList<ShoppingCartDetail> Details { get; set; }
    }
}
