using Ocas.Domestic.Apply.Models;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.Mappers
{
    public partial class ApiMapper
    {
        public Receipt MapReceipt(Dto.Receipt dbDto)
        {
            return _mapper.Map<Receipt>(dbDto);
        }
    }
}