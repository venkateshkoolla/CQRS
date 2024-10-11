using System.Collections.Generic;
using System.Linq;
using Ocas.Domestic.Apply.Models;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Mappers
{
    public partial class ApiMapper
    {
        public IList<College> MapColleges(IList<Dto.College> list, IList<Dto.SchoolStatus> schoolStatuses)
        {
            return list.Select((dtoCollege) =>
                {
                    var college = _mapper.Map<College>(dtoCollege);
                    college.IsOpen = schoolStatuses.FirstOrDefault(x => x.Id == dtoCollege.SchoolStatusId)?.Code == Constants.SchoolStatuses.Open;
                    return college;
                }).ToList();
        }
    }
}
