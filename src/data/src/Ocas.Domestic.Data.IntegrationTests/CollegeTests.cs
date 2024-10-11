using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class CollegeTests : BaseTest
    {
        [Fact]
        public async Task GetColleges_ShouldPass()
        {
            var result = await Context.GetColleges();
            result.Should().HaveCountGreaterOrEqualTo(1);
        }

        [Fact]
        public async Task GetCollege_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.Colleges);

            var college = await Context.GetCollege(result.Id);
            college.Should().BeOfType<College>()
                .And.BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetCollege_ShouldPass_WhenUsingStatusCode()
        {
            // Arrange
            var schoolStatuses = DataFakerFixture.SeedData.SchoolStatuses;
            var open = schoolStatuses.Single(x => x.Code == TestConstants.SchoolStatuses.Open);
            var closed = schoolStatuses.Single(x => x.Code == TestConstants.SchoolStatuses.Closed);

            // Act
            var allColleges = await Context.GetColleges();
            var openColleges = await Context.GetColleges(SchoolStatusCode.Open);
            var closedColleges = await Context.GetColleges(SchoolStatusCode.Closed);

            // Assert
            allColleges.Should().HaveCount(openColleges.Count + closedColleges.Count);
            openColleges.Should().OnlyContain(x => x.SchoolStatusId == open.Id);
            closedColleges.Should().OnlyContain(x => x.SchoolStatusId == closed.Id);
        }
    }
}
