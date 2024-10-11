using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class InstituteTests : BaseTest
    {
        [Fact]
        public async Task GetInstitute_ShouldPass()
        {
            var result = await Context.GetInstitute(TestConstants.Institutes.Id);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetInstitutes_ShouldPass()
        {
            var result = await Context.GetInstitutes();

            result.Should().NotBeNullOrEmpty();
        }
    }
}
