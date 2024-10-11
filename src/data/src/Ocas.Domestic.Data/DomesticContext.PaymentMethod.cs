using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<PaymentMethod> GetPaymentMethod(Guid paymentMethodId, Locale locale)
        {
            return CrmExtrasProvider.GetPaymentMethod(paymentMethodId, locale);
        }

        public Task<IList<PaymentMethod>> GetPaymentMethods(Locale locale)
        {
            return CrmExtrasProvider.GetPaymentMethods(locale);
        }
    }
}
