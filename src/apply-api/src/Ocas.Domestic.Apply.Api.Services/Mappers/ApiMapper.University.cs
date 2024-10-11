using System.Collections.Generic;
using Ocas.Domestic.Apply.Models;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.Mappers
{
    public partial class ApiMapper
    {
        public University MapUniversity(Dto.University university)
        {
            return _mapper.Map<University>(university);
        }

        public IList<University> MapUniversities(IList<Dto.University> list)
        {
            return _mapper.Map<IList<University>>(list);
        }
    }
}
