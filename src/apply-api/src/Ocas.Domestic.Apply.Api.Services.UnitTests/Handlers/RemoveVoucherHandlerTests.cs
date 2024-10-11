using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Api.Services.Handlers;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Handlers
{
    public class RemoveVoucherHandlerTests
    {
        private readonly Dto.Application _application;
        private readonly Dto.Contact _contact;
        private readonly Dto.ShoppingCart _shoppingCart;
        private readonly Dto.Voucher _voucher;
        private readonly RemoveVoucher _request;
        private readonly Mock<ILogger<RemoveVoucherHandler>> _logger;
        private readonly RemoveVoucherHandler _handler;
        private readonly DomesticContextMock _domesticContextMock;
        private readonly ModelFakerFixture _modelFakerFixture;
        private readonly IUserAuthorization _userAuthorization;

        public RemoveVoucherHandlerTests()
        {
            _userAuthorization = Mock.Of<IUserAuthorization>();
            _modelFakerFixture = XunitInjectionCollection.ModelFakerFixture;

            _contact = CreateDtoContact();
            _application = CreateDtoApplication(_contact);
            _voucher = CreateVoucher(_application);
            _shoppingCart = CreateShoppingCart(_application, _voucher);
            _logger = new Mock<ILogger<RemoveVoucherHandler>>();
            _domesticContextMock = new DomesticContextMock();
            _domesticContextMock.Setup(m => m.GetApplication(It.IsAny<Guid>())).ReturnsAsync(_application);
            _domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(_contact);
            _domesticContextMock.Setup(m => m.GetVoucher(It.IsAny<Dto.GetVoucherOptions>())).Returns<Dto.GetVoucherOptions>((options) => Task.FromResult(options.ApplicationId is null ? _voucher : null));
            _domesticContextMock.Setup(m => m.GetShoppingCart(It.IsAny<Dto.GetShoppingCartOptions>(), It.IsAny<Locale>())).ReturnsAsync(_shoppingCart);
            _handler = new RemoveVoucherHandler(_logger.Object, _domesticContextMock.Object, _userAuthorization);

            _request = new RemoveVoucher
            {
                User = Mock.Of<IPrincipal>(),
                ApplicationId = _application.Id,
                Code = _voucher.Code
            };
        }

        [Fact]
        [UnitTest("Handlers")]
        public void RemoveVoucher_ShouldPass()
        {
            // Arrange and Act
            Func<Task> func = () => _handler.Handle(_request, CancellationToken.None);

            // Assert
            func.Should().NotThrow();
            _logger.Verify(x => x.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()), Times.Never);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void RemoveVoucher_ShouldThrow_WhenVoucherDoesNotExist()
        {
            // Arrange
            _domesticContextMock.Setup(m => m.GetVoucher(It.IsAny<Dto.GetVoucherOptions>())).Returns(Task.FromResult<Dto.Voucher>(null));

            // Act
            Func<Task> func = () => _handler.Handle(_request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>().And.Message.Should().Be(RemoveVoucherHandler.Error);
            _logger.Verify(x => x.Log(LogLevel.Warning, It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()), Times.Once);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void RemoveVoucher_ShouldThrow_WhenShoppingCartDoesNotExist()
        {
            // Arrange
            _domesticContextMock.Setup(m => m.GetShoppingCart(It.IsAny<Dto.GetShoppingCartOptions>(), It.IsAny<Locale>())).Returns(Task.FromResult<Dto.ShoppingCart>(null));

            // Act
            Func<Task> func = () => _handler.Handle(_request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>().And.Message.Should().Be(RemoveVoucherHandler.Error);
            _logger.Verify(x => x.Log(LogLevel.Critical, It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()), Times.Once);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void RemoveVoucher_ShouldThrow_WhenNotClaimed()
        {
            // Arrange
            _voucher.VoucherState = VoucherState.Blacklisted;

            // Act
            Func<Task> func = () => _handler.Handle(_request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>().And.Message.Should().Be(RemoveVoucherHandler.Error);
            _logger.Verify(x => x.Log(LogLevel.Warning, It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()), Times.Once);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void RemoveVoucher_ShouldThrow_WhenVoucherMissing()
        {
            // Arrange
            var voucherDetail = _shoppingCart.Details.First(x => x.Type == ShoppingCartItemType.Voucher);
            voucherDetail.VoucherId = Guid.NewGuid();

            // Act
            Func<Task> func = () => _handler.Handle(_request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>().And.Message.Should().Be(RemoveVoucherHandler.Error);
            _logger.Verify(x => x.Log(LogLevel.Warning, It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()), Times.Once);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void RemoveVoucher_ShouldThrow_WhenPaymentLocked()
        {
            // Arrange
            _contact.PaymentLocked = true;

            // Act
            Func<Task> func = () => _handler.Handle(_request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>().And.Message.Should().Be(RemoveVoucherHandler.Error);
            _logger.Verify(x => x.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()), Times.Once);
        }

        private Dto.Application CreateDtoApplication(Dto.Contact applicant)
        {
            var application = _modelFakerFixture.GetApplication().Generate();
            return new Dto.Application
            {
                Id = application.Id,
                ApplicantId = applicant.Id
            };
        }

        private Dto.Contact CreateDtoContact()
        {
            var applicant = _modelFakerFixture.GetApplicant().Generate();
            return new Dto.Contact
            {
                Id = applicant.Id
            };
        }

        private Dto.Voucher CreateVoucher(Dto.Application application)
        {
            var voucher = _modelFakerFixture.GetVoucher().Generate();
            voucher.VoucherState = VoucherState.Claimed;
            voucher.ApplicationId = application.Id;

            return voucher;
        }

        private Dto.ShoppingCart CreateShoppingCart(Dto.Application application, Dto.Voucher voucher)
        {
            return new Dto.ShoppingCart
            {
                ApplicantId = application.ApplicantId,
                ApplicationId = application.Id,
                Details = new List<Dto.ShoppingCartDetail>
                {
                    new Dto.ShoppingCartDetail
                    {
                        Type = ShoppingCartItemType.ApplicationFee,
                        ProductId = voucher.ProductId
                    },
                    new Dto.ShoppingCartDetail
                    {
                        Type = ShoppingCartItemType.Voucher,
                        VoucherProductId = voucher.ProductId,
                        VoucherId = voucher.Id
                    }
                }
            };
        }
    }
}
