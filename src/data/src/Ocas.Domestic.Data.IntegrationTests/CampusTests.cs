using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Models;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class CampusTests : BaseTest
    {
        [Fact]
        public async Task GetCampuses_ShouldPass()
        {
            var result = await Context.GetCampuses();
            result.Should().HaveCountGreaterOrEqualTo(1);
        }

        [Fact]
        public async Task GetCampusByCollege_ShouldPass()
        {
            var college = DataFakerFixture.SeedData.Colleges.Single(x => x.Code == TestConstants.TestCollege.Code);

            var results = await Context.GetCampuses(college.Id);
            results.Should().HaveCountGreaterOrEqualTo(1)
                .And.OnlyContain(c => c.ParentId == college.Id);
        }

        [Fact]
        public async Task GetCampus_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.Campuses);

            var campus = await Context.GetCampus(result.Id);
            campus.Should().BeOfType<Campus>()
                .And.BeEquivalentTo(result);
        }
    }
}
