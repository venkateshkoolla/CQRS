using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Api.Services.Extensions;
using Ocas.Domestic.Apply.Api.Services.Handlers;
using Ocas.Domestic.Apply.Api.Services.Mappers;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.TestFramework;
using Ocas.Domestic.Data.Extras;
using Ocas.Domestic.Enums;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Handlers
{
    public class CreateApplicantBaseHandlerTests
    {
        private readonly IApiMapper _apiMapper;
        private readonly ILookupsCache _lookupsCache;
        private readonly ModelFakerFixture _modelFakerFixture;
        private readonly RequestCache _requestCache;

        public CreateApplicantBaseHandlerTests()
        {
            _apiMapper = XunitInjectionCollection.AutoMapperFixture.CreateApiMapper();
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _modelFakerFixture = XunitInjectionCollection.ModelFakerFixture;
            _requestCache = new RequestCacheMock();
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task CreateApplicantBaseHandler_ShouldPass()
        {
            // Arrange
            var user = new ClaimsPrincipal(
            new ClaimsIdentity(new[] {
            new Claim("email", "sanjaytest_09030019@test.ocas.ca"),
            new Claim("upn", "sanjaytest_09030019@test.ocas.ca"),
            new Claim("sub", Guid.NewGuid().ToString())
            }));

            var applicantBase = _modelFakerFixture.GetApplicantBase().Generate();

            var request = new CreateApplicantBase
            {
                User = user,
                ApplicantBase = applicantBase
            };

            var accountStatusId = _modelFakerFixture.AllApplyLookups.AccountStatuses.First(a => a.Code == ((int)Core.Enums.AccountStatus.Active).ToString()).Id;
            var preferredLanguageId = _modelFakerFixture.AllApplyLookups.PreferredLanguages.First(a => a.Code == ((int)Constants.Localization.EnglishCanada.ToPreferredLanguageEnum()).ToString()).Id;
            var sourceId = _modelFakerFixture.AllApplyLookups.Sources.First(x => x.Code == Constants.Sources.A2C2).Id;
            var sourcePartnerId = _modelFakerFixture.AllApplyLookups.Colleges.First(c => c.AllowCba).Id;

            var dtoContact = new Dto.Contact
            {
                FirstName = request.ApplicantBase.FirstName,
                LastName = request.ApplicantBase.LastName,
                BirthDate = request.ApplicantBase.BirthDate.ToDateTime(Constants.DateFormat.YearMonthDay),
                Email = request.User.GetUpnOrEmail(),
                Username = request.User.GetUpnOrEmail(),
                SubjectId = request.User.GetSubject(),
                AccountStatusId = accountStatusId,
                PreferredLanguageId = preferredLanguageId,
                SourceId = sourceId,
                ModifiedBy = request.User.GetUpnOrEmail(),
                ContactType = ContactType.Applicant,
                DoNotSendMM = true,
                SourcePartnerId = sourcePartnerId
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((Dto.Contact)null);
            domesticContextMock.Setup(m => m.IsDuplicateContact(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).ReturnsAsync(false);
            domesticContextMock.Setup(m => m.IsDuplicateContact(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(false);
            domesticContextMock.Setup(m => m.CreateContact(It.IsAny<Dto.ContactBase>())).ReturnsAsync(dtoContact);

            var domesticContextExtras = new DomesticContextExtras(domesticContextMock.Object);

            var handler = new CreateApplicantBaseHandler(Mock.Of<ILogger<CreateApplicantBaseHandler>>(), domesticContextMock.Object, domesticContextExtras, _apiMapper, _lookupsCache, _requestCache);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().BeOfType<Applicant>().And.NotBeNull();
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task CreateApplicantBaseHandler_ShouldPass_When_ContactExists()
        {
            // Arrange
            var request = new CreateApplicantBase
            {
                User = Mock.Of<IPrincipal>()
            };

            var accountStatusId = _modelFakerFixture.AllApplyLookups.AccountStatuses.First(a => a.Code == ((int)Core.Enums.AccountStatus.Active).ToString()).Id;
            var preferredLanguageId = _modelFakerFixture.AllApplyLookups.PreferredLanguages.First(a => a.Code == ((int)Constants.Localization.EnglishCanada.ToPreferredLanguageEnum()).ToString()).Id;
            var sourceId = _modelFakerFixture.AllApplyLookups.Sources.First(x => x.Code == Constants.Sources.A2C2).Id;
            var sourcePartnerId = _modelFakerFixture.AllApplyLookups.Colleges.First(c => c.AllowCba).Id;

            var applicantBase = _modelFakerFixture.GetApplicantBase().Generate();

            var dtoContact = new Dto.Contact
            {
                FirstName = applicantBase.FirstName,
                LastName = applicantBase.LastName,
                BirthDate = applicantBase.BirthDate.ToDateTime(Constants.DateFormat.YearMonthDay),
                Email = request.User.GetUpnOrEmail(),
                Username = request.User.GetUpnOrEmail(),
                SubjectId = request.User.GetSubject(),
                AccountStatusId = accountStatusId,
                PreferredLanguageId = preferredLanguageId,
                SourceId = sourceId,
                ModifiedBy = request.User.GetUpnOrEmail(),
                ContactType = ContactType.Applicant,
                DoNotSendMM = true,
                SourcePartnerId = sourcePartnerId
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(dtoContact);

            var domesticContextExtrasMock = new Mock<IDomesticContextExtras>();
            domesticContextExtrasMock.Setup(m => m.PatchEducationStatus(It.IsAny<Dto.Contact>(), It.IsAny<string>(), It.IsAny<List<Dto.BasisForAdmission>>(), It.IsAny<List<Dto.Current>>(), It.IsAny<List<Dto.ApplicationCycle>>())).ReturnsAsync(false);

            var handler = new CreateApplicantBaseHandler(Mock.Of<ILogger<CreateApplicantBaseHandler>>(), domesticContextMock.Object, domesticContextExtrasMock.Object, _apiMapper, _lookupsCache, _requestCache);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().BeOfType<Applicant>().And.NotBeNull();
            result.Source.Should().BeNull();
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateApplicantBaseHandler_ShouldThrow_When_IsDuplicateDetails()
        {
            // Arrange
            var applicantBase = _modelFakerFixture.GetApplicantBase().Generate();

            var request = new CreateApplicantBase
            {
                User = Mock.Of<IPrincipal>(),
                ApplicantBase = applicantBase
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((Dto.Contact)null);
            domesticContextMock.Setup(m => m.IsDuplicateContact(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).ReturnsAsync(true);
            domesticContextMock.Setup(m => m.IsDuplicateContact(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(false);

            var domesticContextExtras = new DomesticContextExtras(domesticContextMock.Object);

            var handler = new CreateApplicantBaseHandler(Mock.Of<ILogger<CreateApplicantBaseHandler>>(), domesticContextMock.Object, domesticContextExtras, _apiMapper, _lookupsCache, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ConflictException>()
                .And.Message.Should().Be("Applicant exists with same first name, last name and date of birth");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateApplicantBaseHandler_ShouldThrow_When_IsDuplicateEmail()
        {
            // Arrange
            var applicantBase = _modelFakerFixture.GetApplicantBase().Generate();

            var request = new CreateApplicantBase
            {
                User = Mock.Of<IPrincipal>(),
                ApplicantBase = applicantBase
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((Dto.Contact)null);
            domesticContextMock.Setup(m => m.IsDuplicateContact(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).ReturnsAsync(false);
            domesticContextMock.Setup(m => m.IsDuplicateContact(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(true);

            var domesticContextExtras = new DomesticContextExtras(domesticContextMock.Object);

            var handler = new CreateApplicantBaseHandler(Mock.Of<ILogger<CreateApplicantBaseHandler>>(), domesticContextMock.Object, domesticContextExtras, _apiMapper, _lookupsCache, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ConflictException>()
                .And.Message.Should().Be($"Applicant exists with {request.User.GetUpnOrEmail()}");
        }
    }
}
