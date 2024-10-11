using System;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Ocas.Domestic.Apply.Api.Services.Clients;
using Ocas.Domestic.Apply.Api.Services.Handlers;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Handlers
{
    public class GetOsapTokenHandlerTests
    {
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly ILogger<GetOsapTokenHandler> _logger;
        private readonly ModelFakerFixture _models;

        public GetOsapTokenHandlerTests()
        {
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _logger = Mock.Of<ILogger<GetOsapTokenHandler>>();
            _models = XunitInjectionCollection.ModelFakerFixture;
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetOsapToken_ShouldPass()
        {
            // Arrange
            var applicant = _models.GetApplicant().Generate();
            var request = new GetOsapToken
            {
                ApplicantId = applicant.Id,
                User = Mock.Of<IPrincipal>()
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact
            {
                Id = applicant.Id,
                AccountNumber = applicant.AccountNumber,
                BirthDate = applicant.BirthDate.ToDateTime()
            });

            var osapClientMock = new Mock<IOsapClient>();
            var tokenResponse = new TokenResponse
            {
                AccessToken = _dataFakerFixture.Faker.Random.Hash(),
                ExpiresIn = _dataFakerFixture.Faker.Random.Number(),
                RefreshToken = _dataFakerFixture.Faker.Random.Hash(),
                TokenType = "bearer"
            };
            osapClientMock.Setup(m => m.GetApplicantToken(It.IsAny<string>(), It.IsAny<DateTime>())).ReturnsAsync(tokenResponse);

            var handler = new GetOsapTokenHandler(_logger, domesticContextMock.Object, osapClientMock.Object);

            // Act
            var token = await handler.Handle(request, CancellationToken.None);

            // Assert
            token.Should().NotBeNull();
            token.AccessToken.Should().Be(tokenResponse.AccessToken);
            token.ExpiresIn.Should().Be(tokenResponse.ExpiresIn);
            token.RefreshToken.Should().Be(tokenResponse.RefreshToken);
        }
    }
}
