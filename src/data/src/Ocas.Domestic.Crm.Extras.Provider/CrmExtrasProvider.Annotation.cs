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
        public Task<Annotation> GetSupportingDocumentBinaryData(Guid id)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.AnnotationGet.Id, id }
            };

            return Connection.QueryFirstOrDefaultAsync<Annotation>(Sprocs.AnnotationGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
