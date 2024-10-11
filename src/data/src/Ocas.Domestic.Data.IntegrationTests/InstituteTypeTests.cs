using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class InstituteTypeTests : BaseTest
    {
        [Fact]
        public async Task GetInstituteTypes_ShouldPass()
        {
            var enResult = await Context.GetInstituteTypes(Locale.English);
            var frResult = await Context.GetInstituteTypes(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetInstituteType_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.InstituteTypes);
            var instituteType = await Context.GetInstituteType(result.Id, Locale.English);

            instituteType.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetInstituteType_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.InstituteTypes);
            var instituteType = await Context.GetInstituteType(result.Id, Locale.French);

            instituteType.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
