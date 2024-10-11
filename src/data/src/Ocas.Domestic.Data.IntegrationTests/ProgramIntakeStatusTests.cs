using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class ProgramIntakeStatusTests : BaseTest
    {
        [Fact]
        public async Task GetProgramIntakeStatuses_ShouldPass()
        {
            var enResult = await Context.GetProgramIntakeStatuses(Locale.English);
            var frResult = await Context.GetProgramIntakeStatuses(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetProgramIntakeStatus_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.ProgramIntakeStatuses);
            var programIntakeStatus = await Context.GetProgramIntakeStatus(result.Id, Locale.English);

            programIntakeStatus.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetProgramIntakeStatus_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.ProgramIntakeStatuses);
            var programIntakeStatus = await Context.GetProgramIntakeStatus(result.Id, Locale.French);

            programIntakeStatus.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
