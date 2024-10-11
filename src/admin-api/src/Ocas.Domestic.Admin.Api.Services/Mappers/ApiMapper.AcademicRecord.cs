using System.Collections.Generic;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Models;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Mappers
{
    public partial class ApiMapper
    {
        public AcademicRecord MapAcademicRecord(Dto.AcademicRecord dbDto, IList<HighSchool> highSchools)
        {
            var model = _mapper.Map<AcademicRecord>(dbDto);
            model.HighSchools = _mapper.Map<IList<HighSchoolBrief>>(highSchools);

            return model;
        }
    }
}
