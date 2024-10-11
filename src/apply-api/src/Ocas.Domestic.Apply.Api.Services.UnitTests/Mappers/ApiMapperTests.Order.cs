using FluentAssertions;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Mappers
{
    public partial class ApiMapperTests
    {
        [Fact]
        [UnitTest("Mappers")]
        public void MapOrder_ShouldPass()
        {
            // Arrange
            var dtoOrder = new Dto.Order
            {
                OrderNumber = _dataFakerFixture.Faker.Random.AlphaNumeric(10).ToUpperInvariant(),
                FinalTotal = _dataFakerFixture.Faker.Finance.Amount()
            };

            // Act
            var order = _apiMapper.MapOrder(dtoOrder);

            // Assert
            order.Should().NotBeNull();
            order.Number.Should().Be(dtoOrder.OrderNumber);
            order.Amount.Should().Be(dtoOrder.FinalTotal);
        }
    }
}
