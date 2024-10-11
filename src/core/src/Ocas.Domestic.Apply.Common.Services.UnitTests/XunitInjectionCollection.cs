using Ocas.Domestic.Apply.TestFramework;

namespace Ocas.Domestic.Apply.Common.Services.UnitTests
{
    public static class XunitInjectionCollection
    {
        public static AutoMapperFixture AutoMapperFixture { get; set; } = new AutoMapperFixture();
        public static ModelFakerFixture ModelFakerFixture { get; set; } = new ModelFakerFixture();
    }
}
