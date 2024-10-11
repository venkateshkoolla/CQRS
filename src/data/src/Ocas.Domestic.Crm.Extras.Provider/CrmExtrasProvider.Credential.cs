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
        public Task<IList<Credential>> GetCredentials(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.CredentialsGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<Credential>(Sprocs.CredentialsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<Credential> GetCredential(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.CredentialsGet.Id, id },
                { Sprocs.CredentialsGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<Credential>(Sprocs.CredentialsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
