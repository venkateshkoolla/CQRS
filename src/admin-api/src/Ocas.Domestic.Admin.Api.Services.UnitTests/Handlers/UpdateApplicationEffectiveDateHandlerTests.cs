using System;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Admin.Api.Services.Handlers;
using Ocas.Domestic.Apply.Admin.Api.Services.Mappers;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Handlers
{
    public class UpdateApplicationEffectiveDateHandlerTests
    {
        private readonly ILogger<UpdateApplicationEffectiveDateHandler> _logger;
        private readonly IApiMapper _apiMapper;
        private readonly Faker _faker;
        private readonly IPrincipal _user;
        private readonly ILookupsCache _lookupsCache;
        private readonly AllLookups _lookups;

        public UpdateApplicationEffectiveDateHandlerTests()
        {
            _logger = Mock.Of<ILogger<UpdateApplicationEffectiveDateHandler>>();
            _apiMapper = XunitInjectionCollection.AutoMapperFixture.CreateApiMapper();
            _faker = XunitInjectionCollection.DataFakerFixture.Faker;
            _user = Mock.Of<IPrincipal>();
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _lookups = XunitInjectionCollection.ModelFakerFixture.AllAdminLookups;
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateApplicationHandler_ShouldPass()
        {
            // Arrange
            var request = new UpdateApplicationEffectiveDate
            {
                ApplicationId = Guid.NewGuid(),
                EffectiveDate = _faker.Date.Past().AsUtc().ToStringOrDefault(),
                User = _user
            };

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.GetUserType(It.IsAny<IPrincipal>())).Returns(UserType.OcasUser);

            var domesticContext = new DomesticContextMock();
            domesticContext.Setup(m => m.GetApplication(It.IsAny<Guid>())).ReturnsAsync(new Dto.Application { Id = request.ApplicationId });
            domesticContext.Setup(m => m.UpdateApplication(It.IsAny<Dto.Application>())).ReturnsAsync(new Dto.Application { Id = request.ApplicationId, EffectiveDate = request.EffectiveDate.ToDateTime() });

            var handler = new UpdateApplicationEffectiveDateHandler(_logger, userAuthorization.Object, domesticContext.Object, _apiMapper);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().NotThrow();
            domesticContext.Verify(m => m.UpdateApplication(It.Is<Dto.Application>(a => a.EffectiveDate == request.EffectiveDate.ToDateTime())), Times.Once);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateApplicationHandler_ShouldFail_When_NotOcasUser()
        {
            // Arrange
            var request = new UpdateApplicationEffectiveDate
            {
                ApplicationId = Guid.NewGuid(),
                EffectiveDate = _faker.Date.Past().AsUtc().ToStringOrDefault(),
                User = _user
            };

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.GetUserType(It.IsAny<IPrincipal>())).Returns(_faker.PickRandomWithout(UserType.OcasUser));

            var domesticContext = new DomesticContextMock();

            var handler = new UpdateApplicationEffectiveDateHandler(_logger, userAuthorization.Object, domesticContext.Object, _apiMapper);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ForbiddenException>();
            domesticContext.Verify(m => m.UpdateApplication(It.IsAny<Dto.Application>()), Times.Never);
        }
    }
}
