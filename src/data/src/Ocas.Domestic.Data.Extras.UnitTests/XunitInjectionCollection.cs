using Ocas.Domestic.Data.TestFramework;

namespace Ocas.Domestic.Data.Extras.UnitTests
{
    public static class XunitInjectionCollection
    {
        public static DataFakerFixture DataFakerFixture { get; set; } = new DataFakerFixture();
    }
}
