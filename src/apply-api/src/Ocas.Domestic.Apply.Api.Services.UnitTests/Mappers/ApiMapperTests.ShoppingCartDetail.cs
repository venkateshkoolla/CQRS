using System;
using System.Collections.Generic;
using System.Linq;
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
        public void MapShoppingCart_ShouldPass_When_NsfBalance()
        {
            // Arrange
            var dtoShoppingCart = new Dto.ShoppingCart
            {
                Details = new List<Dto.ShoppingCartDetail>()
            };
            var dtoApplicant = new Dto.Contact
            {
                NsfBalance = _dataFakerFixture.Faker.Finance.Amount()
            };

            // Act
            var shoppingCartDetails = _apiMapper.MapShoppingCartDetail(dtoShoppingCart.Details, dtoApplicant, new Dto.Application());

            // Assert
            shoppingCartDetails.Should().ContainSingle();
            var shoppingCartDetail = shoppingCartDetails.First();
            shoppingCartDetail.Type.Should().Be(ShoppingCartItemType.NsfFee);
            shoppingCartDetail.Amount.Should().Be(dtoApplicant.NsfBalance);
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapShoppingCart_ShouldPass_When_ReturnedPayment()
        {
            // Arrange
            var dtoShoppingCart = new Dto.ShoppingCart
            {
                Details = new List<Dto.ShoppingCartDetail>()
            };
            var dtoApplicant = new Dto.Contact
            {
                ReturnedPayment = _dataFakerFixture.Faker.Finance.Amount()
            };

            // Act
            var shoppingCartDetails = _apiMapper.MapShoppingCartDetail(dtoShoppingCart.Details, dtoApplicant, new Dto.Application());

            // Assert
            shoppingCartDetails.Should().ContainSingle();
            var shoppingCartDetail = shoppingCartDetails.First();
            shoppingCartDetail.Type.Should().Be(ShoppingCartItemType.ReturnedPayment);
            shoppingCartDetail.Amount.Should().Be(dtoApplicant.ReturnedPayment);
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapShoppingCart_ShouldPass_When_AccountBalance()
        {
            // Arrange
            var dtoShoppingCart = new Dto.ShoppingCart
            {
                Details = new List<Dto.ShoppingCartDetail>()
            };
            var dtoApplicant = new Dto.Contact
            {
                Balance = _dataFakerFixture.Faker.Finance.Amount()
            };

            // Act
            var shoppingCartDetails = _apiMapper.MapShoppingCartDetail(dtoShoppingCart.Details, dtoApplicant, new Dto.Application());

            // Assert
            shoppingCartDetails.Should().ContainSingle();
            var shoppingCartDetail = shoppingCartDetails.First();
            shoppingCartDetail.Type.Should().Be(ShoppingCartItemType.AccountCredit);
            shoppingCartDetail.Amount.Should().Be(-dtoApplicant.Balance);
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapShoppingCart_ShouldPass_When_TransferBalance()
        {
            // Arrange
            var dtoShoppingCart = new Dto.ShoppingCart
            {
                Details = new List<Dto.ShoppingCartDetail>()
            };
            var dtoApplication = new Dto.Application
            {
                Balance = _dataFakerFixture.Faker.Finance.Amount()
            };

            // Act
            var shoppingCartDetails = _apiMapper.MapShoppingCartDetail(dtoShoppingCart.Details, new Dto.Contact(), dtoApplication);

            // Assert
            shoppingCartDetails.Should().ContainSingle();
            var shoppingCartDetail = shoppingCartDetails.First();
            shoppingCartDetail.Type.Should().Be(ShoppingCartItemType.TransferBalance);
            shoppingCartDetail.Amount.Should().Be(-dtoApplication.Balance);
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapShoppingCart_ShouldPass_When_Overpayment()
        {
            // Arrange
            var dtoShoppingCart = new Dto.ShoppingCart
            {
                Details = new List<Dto.ShoppingCartDetail>()
            };
            var dtoApplicant = new Dto.Contact
            {
                OverPayment = _dataFakerFixture.Faker.Finance.Amount()
            };

            // Act
            var shoppingCartDetails = _apiMapper.MapShoppingCartDetail(dtoShoppingCart.Details, dtoApplicant, new Dto.Application());

            // Assert
            shoppingCartDetails.Should().ContainSingle();
            var shoppingCartDetail = shoppingCartDetails.First();
            shoppingCartDetail.Type.Should().Be(ShoppingCartItemType.Overpayment);
            shoppingCartDetail.Amount.Should().Be(-dtoApplicant.OverPayment);
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapShoppingCart_ShouldPass_When_Voucher()
        {
            // Arrange
            const string expectedContext = "VOUCHER";
            decimal? expectedAmount = -_dataFakerFixture.Faker.Finance.Amount();
            var dtoShoppingCart = new Dto.ShoppingCart
            {
                Details = new List<Dto.ShoppingCartDetail>
                {
                    new Dto.ShoppingCartDetail
                    {
                        Type = DtoEnum.ShoppingCartItemType.Voucher,
                        Description = expectedContext,
                        ReferenceId = Guid.NewGuid(),
                        Amount = expectedAmount
                    }
                }
            };
            var dtoApplicant = new Dto.Contact();

            // Act
            var shoppingCartDetails = _apiMapper.MapShoppingCartDetail(dtoShoppingCart.Details, dtoApplicant, new Dto.Application());

            // Assert
            shoppingCartDetails.Should().ContainSingle();
            var shoppingCartDetail = shoppingCartDetails.First();
            shoppingCartDetail.ContextId.Should().Be(expectedContext);
            shoppingCartDetail.Type.Should().Be(ShoppingCartItemType.Voucher);
            shoppingCartDetail.Amount.Should().Be(expectedAmount);
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapShoppingCart_ShouldPass_When_NonVoucher_WithReferenceId()
        {
            // Arrange
            var expectedContext = Guid.NewGuid();
            decimal? expectedAmount = _dataFakerFixture.Faker.Finance.Amount();
            var dtoShoppingCart = new Dto.ShoppingCart
            {
                Details = new List<Dto.ShoppingCartDetail>
                {
                    new Dto.ShoppingCartDetail
                    {
                        Type = DtoEnum.ShoppingCartItemType.TranscriptRequestFee,
                        ReferenceId = expectedContext,
                        Amount = expectedAmount
                    }
                }
            };
            var dtoApplicant = new Dto.Contact();

            // Act
            var shoppingCartDetails = _apiMapper.MapShoppingCartDetail(dtoShoppingCart.Details, dtoApplicant, new Dto.Application());

            // Assert
            shoppingCartDetails.Should().ContainSingle();
            var shoppingCartDetail = shoppingCartDetails.First();
            shoppingCartDetail.ContextId.Should().Be(expectedContext.ToString());
            shoppingCartDetail.Type.Should().Be(ShoppingCartItemType.TranscriptRequestFee);
            shoppingCartDetail.Amount.Should().Be(expectedAmount);
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapShoppingCart_ShouldPass_When_NonVoucher_NullReferenceId()
        {
            // Arrange
            decimal? expectedAmount = _dataFakerFixture.Faker.Finance.Amount();
            var dtoShoppingCart = new Dto.ShoppingCart
            {
                Details = new List<Dto.ShoppingCartDetail>
                {
                    new Dto.ShoppingCartDetail
                    {
                        Type = DtoEnum.ShoppingCartItemType.ProgramChoice,
                        ReferenceId = null,
                        Amount = expectedAmount
                    }
                }
            };
            var dtoApplicant = new Dto.Contact();

            // Act
            var shoppingCartDetails = _apiMapper.MapShoppingCartDetail(dtoShoppingCart.Details, dtoApplicant, new Dto.Application());

            // Assert
            shoppingCartDetails.Should().ContainSingle();
            var shoppingCartDetail = shoppingCartDetails.First();
            shoppingCartDetail.ContextId.Should().BeNull();
            shoppingCartDetail.Type.Should().Be(ShoppingCartItemType.ProgramChoice);
            shoppingCartDetail.Amount.Should().Be(expectedAmount);
        }
    }
}
