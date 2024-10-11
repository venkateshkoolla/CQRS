using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class SchoolStatusTests : BaseTest
    {
        [Fact]
        public async Task GetSchoolStatuses_ShouldPass()
        {
            var enResult = await Context.GetSchoolStatuses(Locale.English);
            var frResult = await Context.GetSchoolStatuses(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetSchoolStatus_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.SchoolStatuses);
            var schoolStatus = await Context.GetSchoolStatus(result.Id, Locale.English);

            schoolStatus.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetSchoolStatus_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.SchoolStatuses);
            var schoolStatus = await Context.GetSchoolStatus(result.Id, Locale.French);

            schoolStatus.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
