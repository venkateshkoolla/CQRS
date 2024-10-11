using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<ExpiryAction> GetExpiryAction(Guid id, Locale locale)
        {
            return CrmExtrasProvider.GetExpiryAction(id, locale);
        }

        public Task<IList<ExpiryAction>> GetExpiryActions(Locale locale)
        {
            return CrmExtrasProvider.GetExpiryActions(locale);
        }
    }
}
