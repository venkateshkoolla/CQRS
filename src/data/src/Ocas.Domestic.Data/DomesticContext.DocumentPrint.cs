using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<IList<DocumentPrint>> GetDocumentPrints()
        {
            return CrmExtrasProvider.GetDocumentPrints();
        }
    }
}
