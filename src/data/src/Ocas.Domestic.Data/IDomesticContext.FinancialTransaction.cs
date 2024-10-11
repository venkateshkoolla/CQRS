using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<IList<FinancialTransaction>> GetFinancialTransactions(GetFinancialTransactionOptions options);
        Task<Receipt> CreatePaymentTransactionDetail(ReceiptBase receiptBase);
    }
}
