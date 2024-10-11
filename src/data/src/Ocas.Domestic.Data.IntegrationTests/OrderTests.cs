using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class OrderTests : BaseTest
    {
        [Fact]
        public async Task GetOrder_ShouldPass()
        {
            var order = await Context.GetOrder(TestConstants.Orders.TestOrder);

            order.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateOrder_ShouldPass()
        {
            var shoppingCart = await Context.GetShoppingCart(
                new GetShoppingCartOptions
                {
                    ApplicationId = TestConstants.Orders.TestApplicant.ApplicationId
                },
                Locale.English);

            var order = await Context.CreateOrder(
                TestConstants.Orders.TestApplicant.ApplicationId,
                TestConstants.Orders.TestApplicant.ApplicantId,
                TestConstants.Orders.TestApplicant.ModifiedBy,
                TestConstants.Orders.TestApplicant.SourceId,
                shoppingCart);

            order.Should().NotBeNull();
        }
    }
}
