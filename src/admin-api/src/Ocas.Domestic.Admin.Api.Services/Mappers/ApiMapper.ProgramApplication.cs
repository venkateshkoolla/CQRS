using System.Collections.Generic;
using System.Linq;
using Ocas.Domestic.Apply.Admin.Models;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Mappers
{
    public partial class ApiMapper
    {
        public IList<IntakeApplicant> MapIntakeApplicants(IList<Dto.ProgramApplication> list)
        {
            return list.Select(a => new IntakeApplicant
            {
                Id = a.ApplicationId,
                Number = a.ApplicationNumber,
                FirstName = a.ApplicantFirstName,
                LastName = a.ApplicantLastName
            }).ToList();
        }
    }
}
