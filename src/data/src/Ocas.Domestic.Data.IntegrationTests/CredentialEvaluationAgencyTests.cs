using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class CredentialEvaluationAgencyTests : BaseTest
    {
        [Fact]
        public async Task GetCredentialEvaluationAgencies_ShouldPass()
        {
            var results = await Context.GetCredentialEvaluationAgencies();
            results.Should().HaveCountGreaterOrEqualTo(DataFakerFixture.SeedData.CredentialEvaluationAgencies.Count());
        }

        [Fact]
        public async Task GetCredentialEvaluationAgency_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.CredentialEvaluationAgencies);
            var credentialEvaluationAgency = await Context.GetCredentialEvaluationAgency(result.Id);

            credentialEvaluationAgency.Should().BeEquivalentTo(result);
        }
    }
}
