using Bogus;

namespace Ocas.Domestic.Data.TestFramework
{
    public class DataFakerFixture
    {
        public Faker Faker { get; }
        public EntityFakerFixture Entities { get; }
        public ModelFakerFixture Models { get; }
        public SeedDataFixture SeedData { get; set; }

        public DataFakerFixture()
        {
            Faker = new Faker();
            Entities = new EntityFakerFixture();
            Models = new ModelFakerFixture();
            SeedData = new SeedDataFixture();
        }
    }
}
