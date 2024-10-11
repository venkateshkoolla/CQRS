using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<PaymentResult> GetPaymentResult(Guid paymentResultId);
        Task<IList<PaymentResult>> GetPaymentResults();
    }
}
