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
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Handlers
{
    public class UpdateApplicantInfoHandlerTests
    {
        private readonly IApiMapper _apiMapper;
        private readonly IDtoMapper _dtoMapper;

        public UpdateApplicantInfoHandlerTests()
        {
            _apiMapper = XunitInjectionCollection.AutoMapperFixture.CreateApiMapper();
            _dtoMapper = XunitInjectionCollection.AutoMapperFixture.CreateDtoMapper();
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task UpdateApplicantInfoHandler_ShouldPass()
        {
            // Arrange
            var request = new UpdateApplicantInfo
            {
                ApplicantId = Guid.NewGuid(),
                ApplicantUpdateInfo = new Faker<ApplicantUpdateInfo>()
                    .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                    .RuleFor(u => u.LastName, f => f.Name.LastName())
                    .RuleFor(u => u.BirthDate, f => f.Date.Past().AsUtc().ToStringOrDefault())
                    .Generate(),
                User = Mock.Of<IPrincipal>()
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>()))
                .ReturnsAsync(new Faker<Dto.Contact>()
                    .RuleFor(u => u.Id, _ => request.ApplicantId)
                    .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                    .RuleFor(u => u.LastName, f => f.Name.LastName())
                    .RuleFor(u => u.BirthDate, f => f.Date.Past().AsUtc())
                    .Generate());
            domesticContextMock.Setup(m => m.IsDuplicateContact(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                .ReturnsAsync(false);
            domesticContextMock.Setup(m => m.UpdateContact(It.IsAny<Dto.Contact>()))
                .ReturnsAsync(new Dto.Contact
                {
                    Id = request.ApplicantId,
                    FirstName = request.ApplicantUpdateInfo.FirstName,
                    LastName = request.ApplicantUpdateInfo.LastName,
                    BirthDate = request.ApplicantUpdateInfo.BirthDate.ToDateTime()
                });

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(x => x.GetUserType(It.IsAny<IPrincipal>())).Returns(UserType.OcasUser);

            var handler = new UpdateApplicantInfoHandler(domesticContextMock.Object, _dtoMapper, userAuthorization.Object, _apiMapper);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.BirthDate.Should().Be(request.ApplicantUpdateInfo.BirthDate);
            result.FirstName.Should().Be(request.ApplicantUpdateInfo.FirstName);
            result.LastName.Should().Be(request.ApplicantUpdateInfo.LastName);
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task UpdateApplicantInfoHandler_ShouldPass_When_NoChanges()
        {
            // Arrange
            var request = new UpdateApplicantInfo
            {
                ApplicantId = Guid.NewGuid(),
                ApplicantUpdateInfo = new Faker<ApplicantUpdateInfo>()
                    .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                    .RuleFor(u => u.LastName, f => f.Name.LastName())
                    .RuleFor(u => u.BirthDate, f => f.Date.Past().AsUtc().ToStringOrDefault())
                    .Generate(),
                User = Mock.Of<IPrincipal>()
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>()))
                .ReturnsAsync(new Faker<Dto.Contact>()
                    .RuleFor(u => u.Id, _ => request.ApplicantId)
                    .RuleFor(u => u.FirstName, _ => request.ApplicantUpdateInfo.FirstName)
                    .RuleFor(u => u.LastName, _ => request.ApplicantUpdateInfo.LastName)
                    .RuleFor(u => u.BirthDate, _ => request.ApplicantUpdateInfo.BirthDate.ToDateTime())
                    .Generate());

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(x => x.GetUserType(It.IsAny<IPrincipal>())).Returns(UserType.OcasUser);

            var handler = new UpdateApplicantInfoHandler(domesticContextMock.Object, _dtoMapper, userAuthorization.Object, _apiMapper);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.BirthDate.Should().Be(request.ApplicantUpdateInfo.BirthDate);
            result.FirstName.Should().Be(request.ApplicantUpdateInfo.FirstName);
            result.LastName.Should().Be(request.ApplicantUpdateInfo.LastName);

            domesticContextMock.Verify(m => m.IsDuplicateContact(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()), Times.Never);
            domesticContextMock.Verify(m => m.UpdateContact(It.IsAny<Dto.Contact>()), Times.Never);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateApplicantInfoHandler_ShouldThrow_When_Duplicate()
        {
            // Arrange
            var request = new UpdateApplicantInfo
            {
                ApplicantId = Guid.NewGuid(),
                ApplicantUpdateInfo = new Faker<ApplicantUpdateInfo>()
                    .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                    .RuleFor(u => u.LastName, f => f.Name.LastName())
                    .RuleFor(u => u.BirthDate, f => f.Date.Past().AsUtc().ToStringOrDefault())
                    .Generate(),
                User = Mock.Of<IPrincipal>()
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>()))
                .ReturnsAsync(new Faker<Dto.Contact>()
                    .RuleFor(u => u.Id, _ => request.ApplicantId)
                    .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                    .RuleFor(u => u.LastName, f => f.Name.LastName())
                    .RuleFor(u => u.BirthDate, f => f.Date.Past().AsUtc())
                    .Generate());
            domesticContextMock.Setup(m => m.IsDuplicateContact(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                .ReturnsAsync(true);

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(x => x.GetUserType(It.IsAny<IPrincipal>())).Returns(UserType.OcasUser);

            var handler = new UpdateApplicantInfoHandler(domesticContextMock.Object, _dtoMapper, userAuthorization.Object, _apiMapper);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ConflictException>()
                .And.Message.Should().Be($"Applicant already exists with info: {request.ApplicantUpdateInfo.FirstName} {request.ApplicantUpdateInfo.LastName}, {request.ApplicantUpdateInfo.BirthDate}");

            domesticContextMock.Verify(m => m.UpdateContact(It.IsAny<Dto.Contact>()), Times.Never);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateApplicantInfoHandler_ShouldThrow_When_Applicant_Not_Found()
        {
            // Arrange
            var request = new UpdateApplicantInfo
            {
                ApplicantId = Guid.NewGuid(),
                ApplicantUpdateInfo = new Faker<ApplicantUpdateInfo>()
                    .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                    .RuleFor(u => u.LastName, f => f.Name.LastName())
                    .RuleFor(u => u.BirthDate, f => f.Date.Past().AsUtc().ToStringOrDefault())
                    .Generate(),
                User = Mock.Of<IPrincipal>()
            };

            var domesticContextMock = new DomesticContextMock();

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(x => x.GetUserType(It.IsAny<IPrincipal>())).Returns(UserType.OcasUser);

            var handler = new UpdateApplicantInfoHandler(domesticContextMock.Object, _dtoMapper, userAuthorization.Object, _apiMapper);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<NotFoundException>()
                .And.Message.Should().Be($"Applicant {request.ApplicantId} not found");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateApplicantInfoHandler_ShouldThrow_When_CollegeUser()
        {
            // Arrange
            var request = new UpdateApplicantInfo
            {
                ApplicantId = Guid.NewGuid(),
                ApplicantUpdateInfo = new Faker<ApplicantUpdateInfo>()
                    .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                    .RuleFor(u => u.LastName, f => f.Name.LastName())
                    .RuleFor(u => u.BirthDate, f => f.Date.Past().AsUtc().ToStringOrDefault())
                    .Generate(),
                User = Mock.Of<IPrincipal>()
            };

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.GetUserType(It.IsAny<IPrincipal>())).Returns(UserType.CollegeUser);

            var domesticContextMock = new DomesticContextMock();
            var handler = new UpdateApplicantInfoHandler(domesticContextMock.Object, _dtoMapper, userAuthorizationMock.Object, _apiMapper);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);
            func.Should().Throw<ForbiddenException>();
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateApplicantInfoHandler_ShouldThrow_When_HsUser()
        {
            // Arrange
            var request = new UpdateApplicantInfo
            {
                ApplicantId = Guid.NewGuid(),
                ApplicantUpdateInfo = new Faker<ApplicantUpdateInfo>()
                    .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                    .RuleFor(u => u.LastName, f => f.Name.LastName())
                    .RuleFor(u => u.BirthDate, f => f.Date.Past().AsUtc().ToStringOrDefault())
                    .Generate(),
                User = Mock.Of<IPrincipal>()
            };

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.GetUserType(It.IsAny<IPrincipal>())).Returns(UserType.HighSchoolUser);

            var domesticContextMock = new DomesticContextMock();
            var handler = new UpdateApplicantInfoHandler(domesticContextMock.Object, _dtoMapper, userAuthorizationMock.Object, _apiMapper);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);
            func.Should().Throw<ForbiddenException>();
        }
    }
}
