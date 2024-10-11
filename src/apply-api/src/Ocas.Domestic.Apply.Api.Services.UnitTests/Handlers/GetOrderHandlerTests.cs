using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Api.Services.Handlers;
using Ocas.Domestic.Apply.Api.Services.Mappers;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Handlers
{
    public class GetOrderHandlerTests
    {
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly IApiMapper _apiMapper;
        private readonly ILogger<GetOrderHandler> _logger;
        private readonly IPrincipal _user;
        private readonly IUserAuthorization _userAuthorization;
        private readonly ModelFakerFixture _models;

        public GetOrderHandlerTests()
        {
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _apiMapper = XunitInjectionCollection.AutoMapperFixture.CreateApiMapper();
            _logger = Mock.Of<ILogger<GetOrderHandler>>();
            _user = Mock.Of<IPrincipal>();
            _userAuthorization = Mock.Of<IUserAuthorization>();
            _models = XunitInjectionCollection.ModelFakerFixture;
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetOrder_ShouldPass()
        {
            // Arrange
            var order = _models.GetOrder().Generate();
            var request = new GetOrder
            {
                OrderId = order.Id,
                User = _user
            };
            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetOrder(It.IsAny<Guid>())).ReturnsAsync(new Dto.Order
            {
                Id = order.Id,
                FinalTotal = order.Amount,
                OrderNumber = order.Number,
                Details = order.Details.Select(d => new Dto.OrderDetail
                {
                    PricePerUnit = d.Amount,
                    ReferenceId = Guid.TryParse(d.ContextId, out var contextId) ? contextId : (Guid?)null,
                    Type = (Domestic.Enums.ShoppingCartItemType)d.Type,
                    Description = Guid.TryParse(d.ContextId, out var notUsedContextId) ? null : d.ContextId
                }).ToList(),
                ApplicantId = Guid.NewGuid()
            });
            domesticContextMock.Setup(m => m.GetFinancialTransactions(It.IsAny<Dto.GetFinancialTransactionOptions>())).ReturnsAsync(new List<Dto.FinancialTransaction>());

            var handler = new GetOrderHandler(_logger, domesticContextMock.Object, _userAuthorization, _apiMapper);

            // Act
            var response = await handler.Handle(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Should().BeEquivalentTo(order);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void GetOrder_ShouldThrow_When_OrderNotFound()
        {
            // Arrange
            var request = new GetOrder
            {
                OrderId = Guid.NewGuid(),
                User = _user
            };
            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetOrder(It.IsAny<Guid>())).ReturnsAsync((Dto.Order)null);

            var handler = new GetOrderHandler(_logger, domesticContextMock.Object, _userAuthorization, _apiMapper);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<NotFoundException>()
                .And.Message.Should().Be($"Order {request.OrderId} not found");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void GetOrder_ShouldThrow_When_ApplicantMissing()
        {
            // Arrange
            var request = new GetOrder
            {
                OrderId = Guid.NewGuid(),
                User = _user
            };
            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetOrder(It.IsAny<Guid>())).ReturnsAsync(new Dto.Order
            {
                Id = request.OrderId,
                ApplicantId = null
            });

            var handler = new GetOrderHandler(_logger, domesticContextMock.Object, _userAuthorization, _apiMapper);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be($"Order {request.OrderId} has no ApplicantId");
        }

        public static string reverseWordsInString(string s)
        {

            var str = s.Split(' ');
            var reversedStr = str.Reverse();
            var result = new StringBuilder();
            foreach(var item in reversedStr)
            {
                if(result.Length> 1)
                {
                    result.Append(" ");
                }
                result.Append(item);
            }
            return result.ToString();
        }
    }
}
