using System;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Api.Services.Handlers;
using Ocas.Domestic.Apply.Api.Services.Mappers;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;
using ValidationException = Ocas.Common.Exceptions.ValidationException;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Handlers
{
    public class UpdateApplicantHandlerTests
    {
        private readonly IApiMapper _apiMapper;
        private readonly ILookupsCache _lookupsCache;
        private readonly ModelFakerFixture _modelFakerFixture;
        private readonly IDtoMapper _dtoMapper;
        private readonly DataFakerFixture _dataFakerFixture;

        public UpdateApplicantHandlerTests()
        {
            _apiMapper = XunitInjectionCollection.AutoMapperFixture.CreateApiMapper();
            _dtoMapper = XunitInjectionCollection.AutoMapperFixture.CreateDtoMapper();
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _modelFakerFixture = XunitInjectionCollection.ModelFakerFixture;
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task UpdateApplicantHandler_ShouldPass()
        {
            // Arrange
            var applicant = _modelFakerFixture.GetApplicant().Generate("default, Can");
            var updateApplicant = _modelFakerFixture.GetApplicant().Generate("default, Can");
            updateApplicant.Id = applicant.Id;
            updateApplicant.FirstName = applicant.FirstName;
            updateApplicant.LastName = applicant.LastName;
            updateApplicant.BirthDate = applicant.BirthDate;
            updateApplicant.UserName = applicant.UserName;

            // outside valid date range
            updateApplicant.DateOfArrival = applicant.BirthDate.ToDateTime().AddDays(-1).ToStringOrDefault();

            var request = new UpdateApplicant
            {
                ApplicantId = applicant.Id,
                Applicant = updateApplicant,
                User = Mock.Of<IPrincipal>()
            };

            var dtoContact = new Dto.Contact
            {
                FirstName = applicant.FirstName,
                LastName = applicant.LastName,
                BirthDate = applicant.BirthDate.ToDateTime(),
                Email = applicant.Email,
                Username = applicant.UserName,
                SubjectId = applicant.SubjectId,
                AccountStatusId = applicant.AccountStatusId,
                PreferredLanguageId = applicant.PreferredLanguageId,
                SourceId = _dataFakerFixture.Faker.PickRandom(_modelFakerFixture.AllApplyLookups.Sources).Id,
                ModifiedBy = request.User.GetUpnOrEmail(),
                ContactType = ContactType.Applicant,
                DoNotSendMM = true
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(dtoContact);
            domesticContextMock.Setup(m => m.IsDuplicateContact(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(false);
            domesticContextMock.Setup(m => m.UpdateContact(It.IsAny<Dto.Contact>())).ReturnsAsync(dtoContact);
            domesticContextMock.Setup(m => m.UpdateCompletedSteps(It.IsAny<Guid>())).ReturnsAsync((CompletedSteps?)null);

            var handler = new UpdateApplicantHandler(Mock.Of<ILogger<UpdateApplicantHandler>>(), domesticContextMock.Object, _apiMapper, _dtoMapper, _lookupsCache);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().BeOfType<Applicant>().And.NotBeNull();
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateApplicantHandler_ShouldThrow_When_InvalidBirthDate()
        {
            // Arrange
            var applicant = _modelFakerFixture.GetApplicant().Generate("default, Intl");
            var updateApplicant = _modelFakerFixture.GetApplicant().Generate("default, Intl");
            updateApplicant.Id = applicant.Id;
            updateApplicant.FirstName = applicant.FirstName;
            updateApplicant.LastName = applicant.LastName;
            updateApplicant.BirthDate = applicant.BirthDate;
            updateApplicant.UserName = applicant.UserName;

            // outside valid date range
            updateApplicant.DateOfArrival = applicant.BirthDate.ToDateTime().AddDays(-1).ToStringOrDefault();

            var request = new UpdateApplicant
            {
                ApplicantId = applicant.Id,
                Applicant = updateApplicant,
                User = Mock.Of<IPrincipal>()
            };

            var dtoContact = new Dto.Contact
            {
                FirstName = applicant.FirstName,
                LastName = applicant.LastName,
                BirthDate = applicant.BirthDate.ToDateTime(),
                Email = applicant.Email,
                Username = applicant.UserName,
                SubjectId = applicant.SubjectId,
                AccountStatusId = applicant.AccountStatusId,
                PreferredLanguageId = applicant.PreferredLanguageId,
                SourceId = _dataFakerFixture.Faker.PickRandom(_modelFakerFixture.AllApplyLookups.Sources).Id,
                ModifiedBy = request.User.GetUpnOrEmail(),
                ContactType = ContactType.Applicant,
                DoNotSendMM = true
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(dtoContact);
            domesticContextMock.Setup(m => m.IsDuplicateContact(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(false);

            var handler = new UpdateApplicantHandler(Mock.Of<ILogger<UpdateApplicantHandler>>(), domesticContextMock.Object, _apiMapper, _dtoMapper, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be($"Applicant.DateOfArrival is outside valid range: {request.Applicant.DateOfArrival}");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateApplicantHandler_ShouldThrow_When_ContactNotFound()
        {
            // Arrange
            var applicant = _modelFakerFixture.GetApplicant().Generate("default, Intl");
            var request = new UpdateApplicant
            {
                ApplicantId = applicant.Id,
                Applicant = applicant,
                User = Mock.Of<IPrincipal>()
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync((Dto.Contact)null);

            var handler = new UpdateApplicantHandler(Mock.Of<ILogger<UpdateApplicantHandler>>(), domesticContextMock.Object, _apiMapper, _dtoMapper, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<NotFoundException>()
                .And.Message.Should().Be($"Applicant {request.ApplicantId} not found");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateApplicantHandler_ShouldThrow_When_IsDuplicateEmail()
        {
            // Arrange
            var applicant = _modelFakerFixture.GetApplicant().Generate("default, Intl");
            var request = new UpdateApplicant
            {
                ApplicantId = applicant.Id,
                Applicant = applicant,
                User = Mock.Of<IPrincipal>()
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact());
            domesticContextMock.Setup(m => m.IsDuplicateContact(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(true);

            var handler = new UpdateApplicantHandler(Mock.Of<ILogger<UpdateApplicantHandler>>(), domesticContextMock.Object, _apiMapper, _dtoMapper, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ConflictException>()
                .And.Message.Should().Be($"Applicant exists with {request.Applicant.Email}");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateApplicantHandler_ShouldThrow_When_ChangingLockedField()
        {
            // Arrange
            var applicant = _modelFakerFixture.GetApplicant().Generate("default, Can");
            var updateApplicant = _modelFakerFixture.GetApplicant().Generate("default, Can");
            updateApplicant.Id = applicant.Id;

            var request = new UpdateApplicant
            {
                ApplicantId = applicant.Id,
                Applicant = updateApplicant,
                User = Mock.Of<IPrincipal>()
            };

            var dtoContact = new Dto.Contact
            {
                FirstName = applicant.FirstName,
                LastName = applicant.LastName,
                BirthDate = applicant.BirthDate.ToDateTime(),
                Email = applicant.Email,
                Username = applicant.UserName,
                SubjectId = applicant.SubjectId,
                AccountStatusId = applicant.AccountStatusId,
                PreferredLanguageId = applicant.PreferredLanguageId,
                SourceId = _dataFakerFixture.Faker.PickRandom(_modelFakerFixture.AllApplyLookups.Sources).Id,
                ModifiedBy = request.User.GetUpnOrEmail(),
                ContactType = ContactType.Applicant,
                DoNotSendMM = true
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(dtoContact);
            domesticContextMock.Setup(m => m.IsDuplicateContact(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(false);

            var handler = new UpdateApplicantHandler(Mock.Of<ILogger<UpdateApplicantHandler>>(), domesticContextMock.Object, _apiMapper, _dtoMapper, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be("Cannot change applicant locked fields");
        }
    }
}
