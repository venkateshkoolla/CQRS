using AutoMapper;
using Ocas.Domestic.Apply.Services.Mappers;
using Ocas.Domestic.Apply.TestFramework;

namespace Ocas.Domestic.Apply.Common.Services.UnitTests.Mappers
{
    public partial class ApiMapperBaseTests
    {
        private readonly IApiMapperBase _apiMapper;
        private readonly IMapper _mapper;
        private readonly ModelFakerFixture _models;

        public ApiMapperBaseTests()
        {
            _apiMapper = XunitInjectionCollection.AutoMapperFixture.CreateApiMapper();
            _mapper = XunitInjectionCollection.AutoMapperFixture.CreateMapper();
            _models = XunitInjectionCollection.ModelFakerFixture;
        }
    }
}
