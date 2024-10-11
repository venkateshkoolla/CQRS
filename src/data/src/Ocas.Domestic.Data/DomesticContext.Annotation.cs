using System;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<Annotation> GetSupportingDocumentBinaryData(Guid id)
        {
            return CrmExtrasProvider.GetSupportingDocumentBinaryData(id);
        }
    }
}
