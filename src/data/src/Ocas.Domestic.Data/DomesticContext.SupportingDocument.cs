using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<SupportingDocument> GetSupportingDocument(Guid id)
        {
            return CrmExtrasProvider.GetSupportingDocument(id);
        }

        public Task<IList<SupportingDocument>> GetSupportingDocuments(Guid applicantId)
        {
            return CrmExtrasProvider.GetSupportingDocuments(applicantId);
        }
    }
}
