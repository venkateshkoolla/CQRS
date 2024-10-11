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
        public Task<IList<AddressType>> GetAddressTypes(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.AddressTypesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<AddressType>(Sprocs.AddressTypesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<AddressType> GetAddressType(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.AddressTypesGet.Id, id },
                { Sprocs.AddressTypesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<AddressType>(Sprocs.AddressTypesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
