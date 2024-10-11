using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial class CrmExtrasProvider
    {
        public Task<IList<PaymentMethod>> GetPaymentMethods(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.PaymentMethodsGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<PaymentMethod>(Sprocs.PaymentMethodsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<PaymentMethod> GetPaymentMethod(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.PaymentMethodsGet.Id, id },
                { Sprocs.PaymentMethodsGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<PaymentMethod>(Sprocs.PaymentMethodsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
