using AutoMapper;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Services.Mappers;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Common.Services.UnitTests
{
    public class AutoMapperFixture
    {
        private readonly MapperConfiguration _mapperCfg;

        public AutoMapperFixture()
        {
            _mapperCfg = new MapperConfiguration(cfg => cfg.AddProfile<MapperProfile>());
        }

        public IApiMapperBase CreateApiMapper()
        {
            return new ApiMapperBase(_mapperCfg.CreateMapper());
        }

        public IMapper CreateMapper()
        {
            return _mapperCfg.CreateMapper();
        }
    }

    // TODO: Add a mapper profile to Ocas.Domestic.Apply.Common.Services.ApiMapperBase
    internal class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Dto.ProgramChoice, ProgramChoice>()
                .ForMember(x => x.IntakeId, opt => opt.MapFrom(pc => pc.ProgramIntakeId))
                .ReverseMap();
        }
    }
}
