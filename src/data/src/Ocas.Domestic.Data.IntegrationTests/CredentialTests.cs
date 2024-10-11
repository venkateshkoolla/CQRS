using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class CredentialTests : BaseTest
    {
        [Fact]
        public async Task GetCredentials_ShouldPass()
        {
            var enResult = await Context.GetCredentials(Locale.English);
            var frResult = await Context.GetCredentials(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetCredential_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.Credentials);
            var credential = await Context.GetCredential(result.Id, Locale.English);

            credential.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetCredential_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.Credentials);
            var credential = await Context.GetCredential(result.Id, Locale.French);

            credential.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
