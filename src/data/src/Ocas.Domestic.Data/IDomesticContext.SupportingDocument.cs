using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<SupportingDocument> GetSupportingDocument(Guid id);
        Task<IList<SupportingDocument>> GetSupportingDocuments(Guid applicantId);
    }
}
