using AutoMapper;
using Ocas.Domestic.Apply.Services.Mappers;

namespace Ocas.Domestic.Apply.Api.Services.Mappers
{
    public partial class ApiMapper : ApiMapperBase, IApiMapper
    {
        private readonly IMapper _mapper;

        public ApiMapper(IMapper mapper)
            : base(mapper)
        {
            _mapper = mapper;
        }
    }
}
