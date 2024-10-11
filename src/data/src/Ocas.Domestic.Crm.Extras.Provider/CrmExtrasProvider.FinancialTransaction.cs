using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Ocas.Domestic.Crm.Extras.Provider.Sprocs;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial class CrmExtrasProvider
    {
        public Task<IList<FinancialTransaction>> GetFinancialTransactions(GetFinancialTransactionOptions options)
        {
            var parameters = new Dictionary<string, object>
            {
                { FinancialTransactionsGet.Id, options.Id },
                { FinancialTransactionsGet.ApplicationId, options.ApplicationId },
                { FinancialTransactionsGet.ApplicantId, options.ApplicantId },
                { FinancialTransactionsGet.OrderId, options.OrderId },
                { FinancialTransactionsGet.ReceiptId, options.ReceiptId }
            };

            return Connection.QueryAsync<FinancialTransaction, Receipt, FinancialTransaction>(
                FinancialTransactionsGet.Sproc,
                (master, detail) =>
                {
                    master.Receipt = detail;

                    return master;
                },
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: _commandTimeout)
                .QueryToListAsync();
        }
    }
}
