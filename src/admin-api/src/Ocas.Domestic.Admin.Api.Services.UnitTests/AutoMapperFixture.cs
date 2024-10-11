using AutoMapper;
using Ocas.Domestic.Apply.Admin.Api.Services.Mappers;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests
{
    public class AutoMapperFixture
    {
        private readonly MapperConfiguration _mapperCfg;

        public AutoMapperFixture()
        {
            _mapperCfg = new MapperConfiguration(cfg => cfg.AddProfile<MapperProfile>());
        }

        public IApiMapper CreateApiMapper()
        {
            return new ApiMapper(_mapperCfg.CreateMapper());
        }

        public IDtoMapper CreateDtoMapper()
        {
            return new DtoMapper();
        }

        public IMapper CreateMapper()
        {
            return _mapperCfg.CreateMapper();
        }
    }
}
