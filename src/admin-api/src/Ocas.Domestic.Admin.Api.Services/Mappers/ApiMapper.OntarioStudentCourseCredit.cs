using System.Collections.Generic;
using System.Linq;
using Ocas.Domestic.Apply.Admin.Models;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Mappers
{
    public partial class ApiMapper
    {
        public OntarioStudentCourseCredit MapOntarioStudentCourseCredit(Dto.OntarioStudentCourseCredit dbDto)
        {
            return _mapper.Map<OntarioStudentCourseCredit>(dbDto);
        }

        public IList<OntarioStudentCourseCredit> MapOntarioStudentCourseCredits(IList<Dto.OntarioStudentCourseCredit> list)
        {
            return list.Select(MapOntarioStudentCourseCredit).ToList();
        }
    }
}
