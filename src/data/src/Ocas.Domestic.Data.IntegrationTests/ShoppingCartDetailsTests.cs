using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class ShoppingCartDetailsTests : BaseTest
    {
        [Fact]
        public async Task GetShoppingCartDetails_ShouldPass_WithApplication()
        {
            // Act
            var shoppingCartDetails = await Context.GetShoppingCartDetails(
                new GetShoppingCartDetailOptions
                {
                    ApplicationId = TestConstants.ShoppingCarts.ApplicationWithShoppingCart
                },
                Locale.English);

            // Assert
            shoppingCartDetails.Should().NotBeNullOrEmpty();
            shoppingCartDetails.Should().OnlyHaveUniqueItems(x => x.Id);
        }

        [Fact]
        public async Task GetShoppingCartDetails_ShouldPass_WithApplicant()
        {
            // Act
            var shoppingCartDetails = await Context.GetShoppingCartDetails(
                new GetShoppingCartDetailOptions
                {
                    ApplicantId = TestConstants.ShoppingCarts.ApplicantWithShoppingCartDetails
                },
                Locale.English);

            // Assert
            shoppingCartDetails.Should().NotBeNullOrEmpty();
            shoppingCartDetails.Should().OnlyHaveUniqueItems(x => x.Id);
        }
    }
}
