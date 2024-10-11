using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class ApplicationStatusTests : BaseTest
    {
        [Fact]
        public async Task GetApplicationStatuses_ShouldPass()
        {
            var enResult = await Context.GetApplicationStatuses(Locale.English);
            var frResult = await Context.GetApplicationStatuses(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetApplicationStatus_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.ApplicationStatuses);
            var applicationStatus = await Context.GetApplicationStatus(result.Id, Locale.English);

            applicationStatus.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetApplicationStatus_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.ApplicationStatuses);
            var applicationStatus = await Context.GetApplicationStatus(result.Id, Locale.French);

            applicationStatus.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
