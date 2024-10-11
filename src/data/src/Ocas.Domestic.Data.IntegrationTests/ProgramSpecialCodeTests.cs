using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class ProgramSpecialCodeTests : BaseTest
    {
        [Fact]
        public async Task GetProgramSpecialCodes_ShouldPass()
        {
            var result = await Context.GetProgramSpecialCodes();

            result.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task GetProgramSpecialCodesByCollegeApplicationCycle_ShouldPass()
        {
            var collegeApplicationCycleId = DataFakerFixture.Models.GenerateCollegeApplicationCycleWithProgramSpecialCode();
            var result = await Context.GetProgramSpecialCodes(collegeApplicationCycleId);
            result.Should().HaveCountGreaterOrEqualTo(1);
            result.Should().OnlyContain(o => o.CollegeApplicationId == collegeApplicationCycleId);
        }

        [Fact]
        public async Task GetProgramSpecialCode_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.ProgramSpecialCodes);

            var programSpecialCode = await Context.GetProgramSpecialCode(result.Id);

            programSpecialCode.Should().BeEquivalentTo(result);
        }
    }
}
