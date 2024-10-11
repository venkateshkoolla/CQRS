using System;
using FluentAssertions;
using Ocas.Domestic.Apply.Enums;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;
using DtoEnum = Ocas.Domestic.Enums;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Mappers
{
    public partial class ApiMapperTests
    {
        [Fact]
        [UnitTest("Mappers")]
        public void MapOrderDetail_ShouldPass_When_Voucher()
        {
            // Arrange
            var dtoOrderDetail = new Dto.OrderDetail
            {
                Type = DtoEnum.ShoppingCartItemType.Voucher,
                Description = _dataFakerFixture.Faker.Name.JobDescriptor(),
                ReferenceId = Guid.NewGuid(),
                PricePerUnit = _dataFakerFixture.Faker.Finance.Amount()
            };

            // Act
            var orderDetail = _apiMapper.MapOrderDetail(dtoOrderDetail);

            // Assert
            orderDetail.Should().NotBeNull();
            orderDetail.Type.Should().Be(ShoppingCartItemType.Voucher);
            orderDetail.ContextId.Should().Be(dtoOrderDetail.Description);
            orderDetail.Amount.Should().Be(dtoOrderDetail.PricePerUnit);
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapOrderDetail_ShouldPass_When_NotVoucher()
        {
            // Arrange
            var dtoOrderDetail = new Dto.OrderDetail
            {
                Type = DtoEnum.ShoppingCartItemType.ApplicationFee,
                Description = _dataFakerFixture.Faker.Name.JobDescriptor(),
                ReferenceId = Guid.NewGuid(),
                PricePerUnit = _dataFakerFixture.Faker.Finance.Amount()
            };

            // Act
            var orderDetail = _apiMapper.MapOrderDetail(dtoOrderDetail);

            // Assert
            orderDetail.Should().NotBeNull();
            orderDetail.Type.Should().Be(ShoppingCartItemType.ApplicationFee);
            orderDetail.ContextId.Should().Be(dtoOrderDetail.ReferenceId.ToString());
            orderDetail.Amount.Should().Be(dtoOrderDetail.PricePerUnit);
        }
    }
}
