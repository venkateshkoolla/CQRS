using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class OfferAcceptanceTests : BaseTest
    {
        [Fact]
        public async Task GetOfferAcceptance_ShouldPass()
        {
            // Arrange
            var offerAcceptances = await Context.GetOfferAcceptances(
                new GetOfferAcceptancesOptions
                {
                    ApplicationId = TestConstants.OfferAcceptances.ApplicationWithOffer
                }, Locale.English);

            // Act
            var offerAcceptance = await Context.GetOfferAcceptance(offerAcceptances.First().Id, Locale.English);

            // Assert
            offerAcceptance.Should().NotBeNull();
            offerAcceptance.Should().BeEquivalentTo(offerAcceptances.First());
        }

        [Fact]
        public async Task GetOfferAcceptances_ShouldPass_When_ApplicationId()
        {
            // Arrange & Act
            var offerAcceptancesEn = await Context.GetOfferAcceptances(
                new GetOfferAcceptancesOptions
                {
                    ApplicationId = TestConstants.OfferAcceptances.ApplicationWithOffer
                }, Locale.English);
            var offerAcceptancesFr = await Context.GetOfferAcceptances(
                new GetOfferAcceptancesOptions
                {
                    ApplicationId = TestConstants.OfferAcceptances.ApplicationWithOffer
                }, Locale.French);

            // Assert
            offerAcceptancesEn.Should().NotBeNull();
            offerAcceptancesEn.Should().OnlyContain(x => x.ApplicationId == TestConstants.OfferAcceptances.ApplicationWithOffer);
            offerAcceptancesFr.Should().NotBeNull();
            offerAcceptancesEn.Should().BeEquivalentTo(
                offerAcceptancesFr,
                cols => cols
                    .Excluding(a => a.LocalizedProgramDelivery)
                    .Excluding(a => a.LocalizedOfferStatusDescription));
        }
    }
}