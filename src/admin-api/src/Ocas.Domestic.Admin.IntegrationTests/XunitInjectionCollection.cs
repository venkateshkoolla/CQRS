using Ocas.Domestic.Apply.TestFramework;
using AdminTestFramework = Ocas.Domestic.Apply.Admin.TestFramework;

namespace Ocas.Domestic.Apply.Admin.IntegrationTests
{
    public static class XunitInjectionCollection
    {
        public static TestServerFixture TestServerFixture { get; set; } = new TestServerFixture();
        public static DataFakerFixture DataFakerFixture { get; set; } = new DataFakerFixture();
        public static IdentityUserFixture IdentityUserFixture { get; set; } = new IdentityUserFixture();
        public static AdminTestFramework.ModelFakerFixture ModelFakerFixture { get; set; } = new AdminTestFramework.ModelFakerFixture();
        public static CrmDatabaseFixture CrmDatabaseFixture { get; set; } = new CrmDatabaseFixture(ModelFakerFixture);
    }
}
