using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<IList<FinancialTransaction>> GetFinancialTransactions(GetFinancialTransactionOptions options)
        {
            return CrmExtrasProvider.GetFinancialTransactions(options);
        }

        public async Task<Receipt> CreatePaymentTransactionDetail(ReceiptBase receiptBase)
        {
            if (receiptBase.TimeStamp.HasValue && receiptBase.TimeStamp.Value.Kind != DateTimeKind.Utc)
                throw new ArgumentException($"TimeStamp must be DateTimeKind.Utc but received: {receiptBase.TimeStamp.Value.Kind}", nameof(receiptBase));

            var entity = CrmMapper.MapReceipt(receiptBase);

            var id = await CrmProvider.CreateEntity(entity);

            return await GetReceipt(id);
        }
    }
}