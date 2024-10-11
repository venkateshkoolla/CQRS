using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class ProgramIntakeAvailabilityTests : BaseTest
    {
        [Fact]
        public async Task GetProgramIntakeAvailabilities_ShouldPass()
        {
            var enResult = await Context.GetProgramIntakeAvailabilities(Locale.English);
            var frResult = await Context.GetProgramIntakeAvailabilities(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetProgramIntakeAvailability_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.ProgramIntakeAvailabilities);
            var programIntakeAvailability = await Context.GetProgramIntakeAvailability(result.Id, Locale.English);

            programIntakeAvailability.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetPrivacyStatement_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.ProgramIntakeAvailabilities);
            var programIntakeAvailability = await Context.GetProgramIntakeAvailability(result.Id, Locale.French);

            programIntakeAvailability.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
