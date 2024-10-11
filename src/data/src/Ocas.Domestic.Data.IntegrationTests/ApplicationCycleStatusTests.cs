using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class ApplicationCycleStatusTests : BaseTest
    {
        [Fact]
        public async Task GetApplicationCycleStatuses_ShouldPass()
        {
            var enResult = await Context.GetApplicationCycleStatuses(Locale.English);
            var frResult = await Context.GetApplicationCycleStatuses(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetApplicationCycleStatus_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.ApplicationCycleStatuses);
            var applicationCycleStatus = await Context.GetApplicationCycleStatus(result.Id, Locale.English);

            applicationCycleStatus.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetApplicationCycleStatus_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.ApplicationCycleStatuses);
            var applicationCycleStatus = await Context.GetApplicationCycleStatus(result.Id, Locale.French);

            applicationCycleStatus.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
