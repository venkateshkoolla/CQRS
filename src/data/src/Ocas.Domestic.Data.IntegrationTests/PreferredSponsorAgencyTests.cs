using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class PreferredSponsorAgencyTests : BaseTest
    {
        [Fact]
        public async Task GetPreferredSponsorAgencies_ShouldPass()
        {
            var enResult = await Context.GetPreferredSponsorAgencies(Locale.English);
            var frResult = await Context.GetPreferredSponsorAgencies(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetPreferredSponsorAgency_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.PreferredSponsorAgencies);
            var preferredSponsorAgency = await Context.GetPreferredSponsorAgency(result.Id, Locale.English);

            preferredSponsorAgency.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetPreferredCorrespondenceMethod_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.PreferredSponsorAgencies);
            var preferredSponsorAgency = await Context.GetPreferredSponsorAgency(result.Id, Locale.French);

            preferredSponsorAgency.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
