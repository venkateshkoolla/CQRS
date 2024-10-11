using AutoMapper;

namespace Ocas.Domestic.Apply.Services.Mappers
{
    public partial class ApiMapperBase : IApiMapperBase
    {
        private readonly IMapper _mapper;

        public ApiMapperBase(IMapper mapper)
        {
            _mapper = mapper;
        }
    }
}
