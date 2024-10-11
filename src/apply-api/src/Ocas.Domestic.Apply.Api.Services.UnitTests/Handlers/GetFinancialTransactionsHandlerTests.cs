using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Api.Services.Handlers;
using Ocas.Domestic.Apply.Api.Services.Mappers;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Handlers
{
    public class GetFinancialTransactionsHandlerTests
    {
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly ILogger<GetFinancialTransactionsHandler> _logger;
        private readonly IUserAuthorization _userAuthorization;
        private readonly IApiMapper _apiMapper;
        private readonly ModelFakerFixture _models;

        public GetFinancialTransactionsHandlerTests()
        {
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _logger = Mock.Of<ILogger<GetFinancialTransactionsHandler>>();
            _userAuthorization = Mock.Of<IUserAuthorization>();
            _apiMapper = XunitInjectionCollection.AutoMapperFixture.CreateApiMapper();
            _models = XunitInjectionCollection.ModelFakerFixture;
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetFinancialTransactionsHandler_ShouldPass()
        {
            // Arrange
            var application = _models.GetApplication().Generate();
            var request = new GetFinancialTransactions
            {
                ApplicationId = application.Id,
                User = Mock.Of<IPrincipal>()
            };

            var expectedOptions = new Dto.GetFinancialTransactionOptions();
            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetApplication(application.Id)).ReturnsAsync(new Dto.Application
            {
                Id = application.Id,
                ApplicantId = application.ApplicantId,
                ApplicationCycleId = application.ApplicationCycleId,
                ApplicationStatusId = application.ApplicationStatusId,
                EffectiveDate = application.EffectiveDate.ToDateTime()
            });
            domesticContextMock.Setup(m => m.GetFinancialTransactions(It.IsAny<Dto.GetFinancialTransactionOptions>()))
                .Callback<Dto.GetFinancialTransactionOptions>(o => expectedOptions = o)
                .ReturnsAsync(new List<Dto.FinancialTransaction>());

            var handler = new GetFinancialTransactionsHandler(_logger, domesticContextMock.Object, _userAuthorization, _apiMapper);

            // Act
            var financialTransactions = await handler.Handle(request, CancellationToken.None);

            // Assert
            financialTransactions.Should().BeEmpty();
            domesticContextMock.Verify(e => e.GetFinancialTransactions(It.IsAny<Dto.GetFinancialTransactionOptions>()), Times.Once);
            expectedOptions.ApplicantId.Should().NotBeNull().And.Be(application.ApplicantId);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void GetFinancialTransactionsHandler_ShouldThrow_When_ApplicationNotFound()
        {
            // Arrange
            var application = _models.GetApplication().Generate();
            var request = new GetFinancialTransactions
            {
                ApplicationId = application.Id,
                User = Mock.Of<IPrincipal>()
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetApplication(application.Id)).ReturnsAsync((Dto.Application)null);

            var handler = new GetFinancialTransactionsHandler(_logger, domesticContextMock.Object, _userAuthorization, _apiMapper);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<NotFoundException>()
                .And.Message.Should().Be($"Application {request.ApplicationId} not found");
        }
    }
}
