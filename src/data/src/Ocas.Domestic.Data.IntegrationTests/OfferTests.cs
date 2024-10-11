using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Models;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class OfferTests : BaseTest
    {
        [Fact]
        public async Task GetOffers_ShouldPass_WithApplicantId()
        {
            var offers = await Context.GetOffers(new GetOfferOptions {
                ApplicantId = TestConstants.Offers.ApplicantWithOffers
            });

            offers.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task GetOffers_ShouldPass_WithApplicationId()
        {
            var offers = await Context.GetOffers(new GetOfferOptions {
                ApplicationId = TestConstants.Offers.ApplicationWithOffers
            });

            offers.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task GetOffer_ShouldPass()
        {
            var offer = await Context.GetOffer(TestConstants.Offers.TestOffer);

            offer.Should().NotBeNull();
        }
    }
}
