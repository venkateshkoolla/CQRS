using System.Collections.Generic;
using Ocas.Domestic.Apply.Admin.Models;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Mappers
{
    public partial class ApiMapper
    {
        public SpecialCode MapSpecialCode(Dto.ProgramSpecialCode dbDto)
        {
            return _mapper.Map<SpecialCode>(dbDto);
        }

        public IList<SpecialCode> MapSpecialCodes(IList<Dto.ProgramSpecialCode> list)
        {
            return _mapper.Map<IList<SpecialCode>>(list);
        }
    }
}
