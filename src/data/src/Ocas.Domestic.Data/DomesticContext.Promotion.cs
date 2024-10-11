using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<Promotion> GetPromotion(Guid promotionId, Locale locale)
        {
            return CrmExtrasProvider.GetPromotion(promotionId, locale);
        }

        public Task<IList<Promotion>> GetPromotions(Locale locale)
        {
            return CrmExtrasProvider.GetPromotions(locale);
        }
    }
}
