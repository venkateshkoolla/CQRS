using Ocas.Domestic.Apply.Models;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Mappers
{
    public partial class ApiMapper
    {
        public PrivacyStatement MapPrivacyStatement(Dto.PrivacyStatement dbDto)
        {
            return _mapper.Map<PrivacyStatement>(dbDto);
        }
    }
}