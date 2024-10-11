using Ocas.Domestic.Apply.Api.Services.Mappers;
using Ocas.Domestic.Apply.TestFramework;
using Ocas.Domestic.AppSettings.Extras;
using Ocas.Domestic.Data;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Mappers
{
    public partial class ApiMapperTests
    {
        private readonly IApiMapper _apiMapper;
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly IAppSettingsExtras _appSettingsExtras;
        private readonly ModelFakerFixture _models;
        private readonly IDomesticContext _domesticContext;

        public ApiMapperTests()
        {
            _apiMapper = XunitInjectionCollection.AutoMapperFixture.CreateApiMapper();
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _appSettingsExtras = new AppSettingsExtras(new AppSettingsMock());
            _models = XunitInjectionCollection.ModelFakerFixture;
            _domesticContext = new DomesticContextMock().Object;
        }
    }
}
