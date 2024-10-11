using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial class CrmExtrasProvider
    {
        public Task<IList<Currency>> GetCurrencies()
        {
            return Connection.QueryAsync<Currency>(Sprocs.CurrenciesGet.Sproc, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<Currency> GetCurrency(Guid id)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.CurrenciesGet.Id, id }
            };

            return Connection.QueryFirstOrDefaultAsync<Currency>(Sprocs.CurrenciesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
