using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<PaymentMethod> GetPaymentMethod(Guid paymentMethodId, Locale locale);
        Task<IList<PaymentMethod>> GetPaymentMethods(Locale locale);
    }
}
