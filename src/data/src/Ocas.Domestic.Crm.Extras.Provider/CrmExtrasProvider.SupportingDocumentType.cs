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
        public Task<SupportingDocumentType> GetSupportingDocumentType(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.DocumentTypesGet.Id, id },
                { Sprocs.DocumentTypesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<SupportingDocumentType>(Sprocs.DocumentTypesGet.Sprocs, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }

        public Task<IList<SupportingDocumentType>> GetSupportingDocumentTypes(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.DocumentTypesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<SupportingDocumentType>(Sprocs.DocumentTypesGet.Sprocs, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }
    }
}
