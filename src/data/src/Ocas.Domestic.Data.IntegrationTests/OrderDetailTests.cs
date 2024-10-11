using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class OrderDetailTests : BaseTest
    {
        [Fact]
        public async Task GetOrderDetail_ShouldPass()
        {
            var orderDetail = await Context.GetOrderDetail(TestConstants.OrderDetails.TestOrderDetail);

            orderDetail.Should().NotBeNull();
        }
    }
}