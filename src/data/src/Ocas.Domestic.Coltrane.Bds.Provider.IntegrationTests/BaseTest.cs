using Ocas.Common;
using Xunit;

namespace Ocas.Domestic.Coltrane.Bds.Provider.IntegrationTests
{
    [Collection(nameof(XunitInjectionCollection))]
    public abstract class BaseTest
    {
        protected IColtraneBdsProvider Provider { get; }

        protected BaseTest()
        {
            var appSettingsFetcher = new AppSettingsFetcher();
            var connectionString = appSettingsFetcher.GetConnectionString("ColtraneBDS");
            Provider = new ColtraneBdsProvider(connectionString, 60);
        }
    }
}
