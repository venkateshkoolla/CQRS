using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class ProgramTypeTests : BaseTest
    {
        [Fact]
        public async Task GetProgramTypes_ShouldPass()
        {
            var enResult = await Context.GetProgramTypes(Locale.English);
            var frResult = await Context.GetProgramTypes(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetProgramType_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.ProgramTypes);

            var programType = await Context.GetProgramType(result.Id, Locale.English);

            programType.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetProgramType_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.ProgramTypes);

            var programType = await Context.GetProgramType(result.Id, Locale.French);

            programType.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
