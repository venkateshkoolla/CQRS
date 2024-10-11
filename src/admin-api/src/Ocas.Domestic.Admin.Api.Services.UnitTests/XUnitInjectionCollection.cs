using Ocas.Domestic.Apply.TestFramework;
using Ocas.Domestic.Data;
using AdminTestFramework = Ocas.Domestic.Apply.Admin.TestFramework;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests
{
    public static class XunitInjectionCollection
    {
        public static AutoMapperFixture AutoMapperFixture { get; set; } = new AutoMapperFixture();
        public static DataFakerFixture DataFakerFixture { get; set; } = new DataFakerFixture();
        public static ILookupsCache LookupsCache { get; set; } = new AdminTestFramework.LookupsCacheMock().Object;
        public static AdminTestFramework.ModelFakerFixture ModelFakerFixture { get; set; } = new AdminTestFramework.ModelFakerFixture();
        public static RequestCacheMock RequestCacheMock { get; set; } = new RequestCacheMock();
        public static IDomesticContext DomesticContext { get; set; } = new DomesticContextMock().Object;
    }
}
