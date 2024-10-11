using Ocas.Domestic.Apply.Models;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.Mappers
{
    public partial class ApiMapper
    {
        public OrderDetail MapOrderDetail(Dto.OrderDetail dbDto)
        {
            return _mapper.Map<OrderDetail>(dbDto);
        }
    }
}