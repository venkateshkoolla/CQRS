using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class SchoolTypeTests : BaseTest
    {
        [Fact]
        public async Task GetSchoolTypes_ShouldPass()
        {
            var enResult = await Context.GetSchoolTypes(Locale.English);
            var frResult = await Context.GetSchoolTypes(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetSchoolType_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.SchoolTypes);
            var schoolType = await Context.GetSchoolType(result.Id, Locale.English);

            schoolType.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetSchoolType_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.SchoolTypes);
            var schoolType = await Context.GetSchoolType(result.Id, Locale.French);

            schoolType.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
