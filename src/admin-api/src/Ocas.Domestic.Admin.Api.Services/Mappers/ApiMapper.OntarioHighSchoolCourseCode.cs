using Ocas.Domestic.Apply.Admin.Models;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Mappers
{
    public partial class ApiMapper
    {
        public OntarioHighSchoolCourseCode MapOntarioHighSchoolCourseCode(Dto.OntarioHighSchoolCourseCode dbDto)
        {
            return _mapper.Map<OntarioHighSchoolCourseCode>(dbDto);
        }
    }
}
