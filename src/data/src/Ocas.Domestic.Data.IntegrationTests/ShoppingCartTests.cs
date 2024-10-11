using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class ShoppingCartTests : BaseTest
    {
        [Fact]
        public async Task GetShoppingCart_ShouldPass_WithApplication()
        {
            // Act
            var shoppingCart = await Context.GetShoppingCart(
                new GetShoppingCartOptions
                {
                    ApplicationId = TestConstants.ShoppingCarts.ApplicationWithShoppingCart
                },
                Locale.English);

            var shoppingCartById = await Context.GetShoppingCart(
                new GetShoppingCartOptions
                {
                    Id = shoppingCart.Id
                },
                Locale.English);

            // Assert
            shoppingCart.Should().NotBeNull();
            shoppingCart.ApplicationId.Should().Be(TestConstants.ShoppingCarts.ApplicationWithShoppingCart);
            shoppingCart.Details.Should().NotBeNull();
            shoppingCart.Details.Should().NotBeEmpty();
            shoppingCart.Details.Should().OnlyContain(x => x.ShoppingCartId == shoppingCart.Id);
            shoppingCart.Details.Should().OnlyHaveUniqueItems(x => x.Id);
            shoppingCart.Should().BeEquivalentTo(shoppingCartById);
        }

        [Fact]
        public async Task GetShoppingCart_ShouldPass_WhenNotFound()
        {
            // Act
            var shoppingCart = await Context.GetShoppingCart(
                new GetShoppingCartOptions
                {
                    ApplicationId = Guid.Empty
                },
                Locale.English);

            // Assert
            shoppingCart.Should().BeNull();
        }

        [Fact]
        public async Task GetShoppingCart_ShouldPass_WithSupplementalFee()
        {
            // Act
            var shoppingCartEn = await Context.GetShoppingCart(
                new GetShoppingCartOptions
                {
                    ApplicationId = TestConstants.ShoppingCarts.ApplicationWithSupplementalFee
                },
                Locale.English);
            var shoppingCartFr = await Context.GetShoppingCart(
                new GetShoppingCartOptions
                {
                    ApplicationId = TestConstants.ShoppingCarts.ApplicationWithSupplementalFee
                },
                Locale.French);

            // Assert
            shoppingCartEn.Should().NotBeNull();
            shoppingCartEn.ApplicationId.Should().Be(TestConstants.ShoppingCarts.ApplicationWithSupplementalFee);
            shoppingCartEn.Details.Should().NotBeNullOrEmpty()
                .And.OnlyContain(x => x.ShoppingCartId == shoppingCartEn.Id)
                .And.OnlyHaveUniqueItems(x => x.Id)
                .And.Contain(x => x.Type == ShoppingCartItemType.SupplementalApplicationFee);

            var supplementalFeeEn = shoppingCartEn.Details.First(x => x.Type == ShoppingCartItemType.SupplementalApplicationFee);
            supplementalFeeEn.Description.Should().NotBeNullOrEmpty();

            var supplementalFeeFr = shoppingCartFr.Details.First(x => x.Type == ShoppingCartItemType.SupplementalApplicationFee);
            supplementalFeeFr.Description.Should().NotBeNullOrEmpty();

            shoppingCartEn.Details.Should().BeEquivalentTo(shoppingCartFr.Details, opts => opts.Excluding(x => x.Description));
        }
    }
}
