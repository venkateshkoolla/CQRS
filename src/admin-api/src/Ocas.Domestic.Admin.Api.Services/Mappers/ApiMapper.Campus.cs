using System.Collections.Generic;
using System.Linq;
using Ocas.Domestic.Apply.Models;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Mappers
{
    public partial class ApiMapper
    {
        public IList<Campus> MapCampuses(IList<Dto.Campus> list)
        {
            return list.Select(dtoCampus => _mapper.Map<Campus>(dtoCampus)).ToList();
        }
    }
}