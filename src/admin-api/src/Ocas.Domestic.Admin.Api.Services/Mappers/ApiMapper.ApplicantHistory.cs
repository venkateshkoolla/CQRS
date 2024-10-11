using System.Collections.Generic;
using Ocas.Domestic.Apply.Admin.Models;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Mappers
{
    public partial class ApiMapper
    {
        public IList<ApplicantHistory> MapApplicantHistories(IList<Dto.CustomAudit> customAudits)
        {
            return _mapper.Map<IList<ApplicantHistory>>(customAudits);
        }
    }
}
