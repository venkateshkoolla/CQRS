using System.Collections.Generic;
using System.Threading.Tasks;
using CacheManager.Core;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Ocas.Domestic.Apply.Api.Services.Caches;
using Ocas.Domestic.Apply.Api.Services.Mappers;
using Ocas.Domestic.Apply.TestFramework;
using Ocas.Domestic.AppSettings.Extras;
using Ocas.Domestic.Enums;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Caches
{
    public class LookupsCacheTests
    {
        private readonly DomesticContextMock _domesticContext;
        private readonly ICacheManager<object> _emptyCache = CacheFactory.Build("emptyCache", settings => settings.WithSystemRuntimeCacheHandle("emptyCacheMemoryLayer"));
        private readonly AppSettingsMock _appSettingsMock = new AppSettingsMock();
        private readonly IAppSettingsExtras _appSettingsExtras = Mock.Of<IAppSettingsExtras>();
        private readonly IApiMapper _apiMapper;

        public LookupsCacheTests()
        {
            _domesticContext = new DomesticContextMock();
            _apiMapper = XunitInjectionCollection.AutoMapperFixture.CreateApiMapper();
        }

        [Fact]
        [UnitTest("Caches")]
        public async Task GetCache_ShouldCallDbOnce_WhenCalledManyTimes()
        {
            // Arrange
            var domesticContext = _domesticContext.Object;
            var lookupsCache = new LookupsCache(_emptyCache, domesticContext, _appSettingsMock, _appSettingsExtras, _apiMapper);
            lookupsCache.PurgeAllLookups(null);

            // Act
            await lookupsCache.GetSources(TestConstants.Locale.English);
            await lookupsCache.GetSources(TestConstants.Locale.English);
            await lookupsCache.GetSources(TestConstants.Locale.English);
            await lookupsCache.GetSources(TestConstants.Locale.English);

            // Assert
            _domesticContext.Verify(e => e.GetSources(Locale.English), Times.Once);
        }

        [Fact]
        [UnitTest("Caches")]
        public async Task GetLookups_ShouldPass_WhenFiltered()
        {
            // Arrange
            var domesticContext = _domesticContext.Object;
            var lookupsCache = new LookupsCache(_emptyCache, domesticContext, _appSettingsMock, _appSettingsExtras, _apiMapper);
            var keys = new[] { "accountStatuses", "sources" };
            var lookupsSerializer = new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                Converters = new List<JsonConverter>
                {
                    new StringEnumConverter()
                },
                NullValueHandling = NullValueHandling.Ignore
            };

            // Act
            var result = await lookupsCache.GetAllLookups(TestConstants.Locale.English, keys);

            // Assert
            var json = JsonConvert.SerializeObject(result, lookupsSerializer);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            dictionary.Should().ContainKeys(keys);
        }
    }
}
