using System.Collections.Generic;
using Ocas.Domestic.Apply.Models;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.Mappers
{
    public partial class ApiMapper
    {
        public HighSchool MapHighSchool(Dto.HighSchool highSchool)
        {
            return _mapper.Map<HighSchool>(highSchool);
        }

        public IList<HighSchool> MapHighSchools(IList<Dto.HighSchool> list)
        {
            return _mapper.Map<IList<HighSchool>>(list);
        }
    }
}
