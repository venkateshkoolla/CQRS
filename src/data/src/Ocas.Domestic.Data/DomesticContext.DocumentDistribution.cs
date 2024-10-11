using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<IList<DocumentDistribution>> GetDocumentDistributions(Guid applicationId)
        {
            return CrmExtrasProvider.GetDocumentDistributions(applicationId);
        }
    }
}
