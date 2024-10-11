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
        public Task<SupportingDocument> GetSupportingDocument(Guid id)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.SupportingDocumentsGet.Id, id }
            };

            return Connection.QueryFirstOrDefaultAsync<SupportingDocument>(Sprocs.SupportingDocumentsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }

        public Task<IList<SupportingDocument>> GetSupportingDocuments(Guid applicantId)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.SupportingDocumentsGet.ApplicantId, applicantId }
            };

            return Connection.QueryAsync<SupportingDocument>(Sprocs.SupportingDocumentsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }
    }
}
