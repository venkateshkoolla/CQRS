using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.TestFramework;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests
{
    public static class XunitInjectionCollection
    {
        public static AutoMapperFixture AutoMapperFixture { get; set; } = new AutoMapperFixture();
        public static DataFakerFixture DataFakerFixture { get; set; } = new DataFakerFixture();
        public static ILookupsCache LookupsCache { get; set; } = new LookupsCacheMock().Object;
        public static ModelFakerFixture ModelFakerFixture { get; set; } = new ModelFakerFixture();
    }
}
