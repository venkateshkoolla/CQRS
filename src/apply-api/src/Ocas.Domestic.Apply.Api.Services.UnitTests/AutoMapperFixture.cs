using AutoMapper;
using Ocas.Domestic.Apply.Api.Services.Mappers;
using Ocas.Domestic.Apply.Services.Mappers;
using Ocas.Domestic.Apply.TestFramework;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests
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

        public IApiMapperBase CreateApiMapperBase()
        {
            return new ApiMapperBase(_mapperCfg.CreateMapper());
        }

        public IDtoMapperBase CreateDtoMapperBase()
        {
            return new DtoMapperBase();
        }

        public IDtoMapper CreateDtoMapper()
        {
            return new DtoMapper(XunitInjectionCollection.LookupsCache, new DomesticContextMock().Object, _mapperCfg.CreateMapper());
        }

        public IMapper CreateMapper()
        {
            return _mapperCfg.CreateMapper();
        }

        public ITemplateMapper CreateTemplateMapper()
        {
            return new TemplateMapper();
        }
    }
}
