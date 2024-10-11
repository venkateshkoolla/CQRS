using Ocas.Common;
using Ocas.Domestic.Data.TestFramework;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public static class XunitInjectionCollection
    {
        public static AppSettingsFetcher AppSettingsFetcher { get; set; } = new AppSettingsFetcher();
        public static DataFakerFixture DataFakerFixture { get; set; } = new DataFakerFixture();
    }
}