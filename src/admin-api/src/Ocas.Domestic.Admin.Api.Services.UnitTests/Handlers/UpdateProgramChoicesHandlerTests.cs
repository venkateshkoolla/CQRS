using System;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using Moq;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Admin.Api.Services.Handlers;
using Ocas.Domestic.Apply.Admin.Api.Services.Mappers;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Handlers
{
    public class UpdateProgramChoicesHandlerTests
    {
        private readonly IApiMapper _apiMapper;
        private readonly Faker _faker;

        public UpdateProgramChoicesHandlerTests()
        {
            _apiMapper = XunitInjectionCollection.AutoMapperFixture.CreateApiMapper();
            _faker = XunitInjectionCollection.DataFakerFixture.Faker;
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task UpdateProgramChoiceEffectivieDate_ShouldPass()
        {
            // Arrange
            var request = new UpdateProgramChoice
            {
                ProgramChoiceId = Guid.NewGuid(),
                EffectiveDate = _faker.Date.Between(DateTime.Now.AddMonths(-1).AsUtc(), DateTime.Now.AddMonths(-2).AsUtc()).ToStringOrDefault(),
                User = Mock.Of<IPrincipal>()
            };
            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(x => x.GetUserType(It.IsAny<IPrincipal>())).Returns(UserType.OcasUser);

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(x => x.GetProgramChoice(It.IsAny<Guid>())).ReturnsAsync(new Dto.ProgramChoice { Id = request.ProgramChoiceId });
            domesticContextMock.Setup(x => x.UpdateProgramChoice(It.IsAny<Dto.ProgramChoice>())).ReturnsAsync(new Dto.ProgramChoice { Id = request.ProgramChoiceId, EffectiveDate = request.EffectiveDate.ToDateTime() });

            var handler = new UpdateProgramChoicesHandler(domesticContextMock.Object, userAuthorization.Object, _apiMapper);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.EffectiveDate.Should().Be(request.EffectiveDate);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateProgramChoiceEffectivieDate_ShouldThrow_When_NotOcasUser()
        {
            // Arrange
            var request = new UpdateProgramChoice
            {
                ProgramChoiceId = Guid.NewGuid(),
                EffectiveDate = _faker.Date.Past().AsUtc().ToStringOrDefault(),
                User = Mock.Of<IPrincipal>()
            };

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(x => x.GetUserType(It.IsAny<IPrincipal>())).Returns(_faker.PickRandomWithout(UserType.OcasUser));

            var domesticContextMock = new DomesticContextMock();
            var handler = new UpdateProgramChoicesHandler(domesticContextMock.Object, userAuthorization.Object, _apiMapper);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ForbiddenException>();
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateProgramChoiceEffectiveDate_ShouldThrow_When_ProgramChoice_NotFound()
        {
            // Arrange
            var request = new UpdateProgramChoice
            {
                ProgramChoiceId = Guid.NewGuid(),
                EffectiveDate = _faker.Date.Past().AsUtc().ToStringOrDefault(),
                User = Mock.Of<IPrincipal>()
            };

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(x => x.GetUserType(It.IsAny<IPrincipal>())).Returns(UserType.OcasUser);

            var domesticContextMock = new DomesticContextMock();
            var handler = new UpdateProgramChoicesHandler(domesticContextMock.Object, userAuthorization.Object, _apiMapper);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<NotFoundException>().WithMessage($"Program choice {request.ProgramChoiceId} not found");
        }
    }
}
