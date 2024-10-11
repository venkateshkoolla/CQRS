using Ocas.Domestic.Apply.TestFramework;

namespace Ocas.Domestic.Apply.IntegrationTests
{
    public static class XunitInjectionCollection
    {
        public static TestServerFixture TestServerFixture { get; set; } = new TestServerFixture();
        public static DataFakerFixture DataFakerFixture { get; set; } = new DataFakerFixture();
        public static ModelFakerFixture ModelFakerFixture { get; set; } = new ModelFakerFixture();
        public static IdentityUserFixture IdentityUserFixture { get; set; } = new IdentityUserFixture();
        public static MonerisFixture MonerisFixture { get; set; } = new MonerisFixture();
        public static CrmDatabaseFixture CrmDatabaseFixture { get; set; } = new CrmDatabaseFixture(ModelFakerFixture);
        public static EtmsFixture EtmsFixture { get; set; } = new EtmsFixture();
    }
}
