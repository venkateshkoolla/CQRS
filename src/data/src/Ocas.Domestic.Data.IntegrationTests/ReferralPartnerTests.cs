using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Models;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class ReferralPartnerTests : BaseTest
    {
        [Fact]
        public async Task GetReferralPartners_ShouldPass()
        {
            var result = await Context.GetReferralPartners();
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetReferralPartner_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.ReferralPartners);

            var referralPartner = await Context.GetReferralPartner(result.Id);
            referralPartner.Should().BeOfType<ReferralPartner>()
                .And.BeEquivalentTo(result);
        }
    }
}
