using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<PaymentResult> GetPaymentResult(Guid paymentResultId)
        {
            return CrmExtrasProvider.GetPaymentResult(paymentResultId);
        }

        public Task<IList<PaymentResult>> GetPaymentResults()
        {
            return CrmExtrasProvider.GetPaymentResults();
        }
    }
}
