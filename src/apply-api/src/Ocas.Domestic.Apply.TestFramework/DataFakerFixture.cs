using Bogus;
using Ocas.Domestic.Data.TestFramework;

namespace Ocas.Domestic.Apply.TestFramework
{
    public class DataFakerFixture
    {
        public Faker Faker { get; }
        public SeedDataFixture SeedData { get; set; }

        public DataFakerFixture()
        {
            Faker = new Faker();
            SeedData = new SeedDataFixture();
        }
    }
}
