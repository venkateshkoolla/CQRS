using System.Collections.Generic;
using Ocas.Domestic.Apply.Models;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.Mappers
{
    public partial class ApiMapper
    {
        public IList<DocumentPrint> MapDocumentPrints(IList<Dto.DocumentPrint> documentPrints)
        {
            return _mapper.Map<IList<DocumentPrint>>(documentPrints);
        }
    }
}
