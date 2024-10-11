using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Admin.Api.Services.Handlers;
using Ocas.Domestic.Apply.Admin.Api.Services.Mappers;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;
using AdminTestFramework = Ocas.Domestic.Apply.Admin.TestFramework;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Handlers
{
    public class UpdateOntarioStudentCourseCreditHandlerTests
    {
        private readonly IApiMapper _apiMapper;
        private readonly AdminTestFramework.ModelFakerFixture _models;
        private readonly RequestCache _requestCache;
        private readonly IDtoMapper _dtoMapper;

        public UpdateOntarioStudentCourseCreditHandlerTests()
        {
            _apiMapper = XunitInjectionCollection.AutoMapperFixture.CreateApiMapper();
            _dtoMapper = XunitInjectionCollection.AutoMapperFixture.CreateDtoMapper();
            _models = XunitInjectionCollection.ModelFakerFixture;
            _requestCache = new RequestCacheMock();
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task UpdateOntarioStudentCourseCredit_ShouldPass()
        {
            // Arrange
            var ontarioStudentCourseCredit = _models.GetOntarioStudentCourseCredit().Generate();

            var request = new UpdateOntarioStudentCourseCredit
            {
                User = Mock.Of<IPrincipal>(),
                ApplicantId = ontarioStudentCourseCredit.ApplicantId,
                OntarioStudentCourseCredit = ontarioStudentCourseCredit,
                OntarioStudentCourseCreditId = ontarioStudentCourseCredit.Id
            };

            var dtoOntarioStudentCourseCredit = new Dto.OntarioStudentCourseCredit
            {
                Id = ontarioStudentCourseCredit.Id,
                ApplicantId = ontarioStudentCourseCredit.ApplicantId,
                CourseCode = ontarioStudentCourseCredit.CourseCode,
                CompletedDate = ontarioStudentCourseCredit.CompletedDate.ToDateTime(Constants.DateFormat.YearMonthDashed).ToString(Constants.DateFormat.CompletedDate),
                CourseDeliveryId = ontarioStudentCourseCredit.CourseDeliveryId,
                CourseStatusId = ontarioStudentCourseCredit.CourseStatusId,
                CourseTypeId = ontarioStudentCourseCredit.CourseTypeId,
                GradeTypeId = ontarioStudentCourseCredit.GradeTypeId,
                TranscriptId = ontarioStudentCourseCredit.TranscriptId,
                CourseMident = ontarioStudentCourseCredit.CourseMident,
                Credit = ontarioStudentCourseCredit.Credit,
                Grade = ontarioStudentCourseCredit.Grade,
                ModifiedBy = ontarioStudentCourseCredit.ModifiedBy,
                Notes = string.Concat(ontarioStudentCourseCredit.Notes)
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetOntarioStudentCourseCredits(It.IsAny<Dto.GetOntarioStudentCourseCreditOptions>())).ReturnsAsync(new List<Dto.OntarioStudentCourseCredit> { new Dto.OntarioStudentCourseCredit { ApplicantId = ontarioStudentCourseCredit.ApplicantId } });
            domesticContextMock.Setup(m => m.GetOntarioStudentCourseCredit(It.IsAny<Guid>())).ReturnsAsync(new Dto.OntarioStudentCourseCredit { Id = request.OntarioStudentCourseCredit.Id, ApplicantId = request.ApplicantId });
            domesticContextMock.Setup(m => m.UpdateOntarioStudentCourseCredit(It.IsAny<Dto.OntarioStudentCourseCredit>())).ReturnsAsync(dtoOntarioStudentCourseCredit);

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsHighSchoolUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new UpdateOntarioStudentCourseCreditHandler(Mock.Of<ILogger<UpdateOntarioStudentCourseCreditHandler>>(), domesticContextMock.Object, _dtoMapper, userAuthorizationMock.Object, _apiMapper, _requestCache);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(ontarioStudentCourseCredit, options => options.Excluding(x => x.CompletedDate).Excluding(x => x.ModifiedBy).Excluding(x => x.ModifiedOn).Excluding(x => x.SupplierMident));
            domesticContextMock.Verify(e => e.UpdateOntarioStudentCourseCredit(It.IsAny<Dto.OntarioStudentCourseCredit>()), Times.Once);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateOntarioStudentCourseCredit_ShouldThrow_When_RequestNotFound()
        {
            // Arrange
            var ontarioStudentCourseCredit = _models.GetOntarioStudentCourseCredit().Generate();

            var request = new UpdateOntarioStudentCourseCredit
            {
                User = Mock.Of<IPrincipal>(),
                OntarioStudentCourseCredit = ontarioStudentCourseCredit,
                OntarioStudentCourseCreditId = ontarioStudentCourseCredit.Id
            };

            var domesticContextMock = new DomesticContextMock();
            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsHighSchoolUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new UpdateOntarioStudentCourseCreditHandler(Mock.Of<ILogger<UpdateOntarioStudentCourseCreditHandler>>(), domesticContextMock.Object, _dtoMapper, userAuthorizationMock.Object, _apiMapper, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<NotFoundException>().WithMessage("OntarioStudentCourseCredit does not exist");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateOntarioStudentCourseCredit_ShouldThrow_When_ApplicantMismatch()
        {
            // Arrange
            var ontarioStudentCourseCredit = _models.GetOntarioStudentCourseCredit().Generate();

            var request = new UpdateOntarioStudentCourseCredit
            {
                User = Mock.Of<IPrincipal>(),
                ApplicantId = ontarioStudentCourseCredit.ApplicantId,
                OntarioStudentCourseCredit = ontarioStudentCourseCredit,
                OntarioStudentCourseCreditId = ontarioStudentCourseCredit.Id
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetOntarioStudentCourseCredit(It.IsAny<Guid>())).ReturnsAsync(new Dto.OntarioStudentCourseCredit { Id = request.OntarioStudentCourseCreditId, ApplicantId = Guid.NewGuid() });

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsHighSchoolUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new UpdateOntarioStudentCourseCreditHandler(Mock.Of<ILogger<UpdateOntarioStudentCourseCreditHandler>>(), domesticContextMock.Object, _dtoMapper, userAuthorizationMock.Object, _apiMapper, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>().WithMessage($"'Ontario Student Course Credit' does not belong to applicant: {request.ApplicantId}");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateOntarioStudentCourseCredit_ShouldThrow_When_CourseCodeAndCompletedDate_IsDuplicate()
        {
            // Arrange
            var ontarioStudentCourseCredit = _models.GetOntarioStudentCourseCredit().Generate();

            var request = new UpdateOntarioStudentCourseCredit
            {
                User = Mock.Of<IPrincipal>(),
                OntarioStudentCourseCredit = ontarioStudentCourseCredit
            };

            var domesticContextMock = new DomesticContextMock();
            var lookupsCacheMock = new LookupsCacheMock();
            domesticContextMock.Setup(m => m.GetOntarioStudentCourseCredits(It.IsAny<Dto.GetOntarioStudentCourseCreditOptions>())).ReturnsAsync(new List<Dto.OntarioStudentCourseCredit> { new Dto.OntarioStudentCourseCredit { ApplicantId = ontarioStudentCourseCredit.ApplicantId, CourseCode = ontarioStudentCourseCredit.CourseCode, CompletedDate = ontarioStudentCourseCredit.CompletedDate } });
            domesticContextMock.Setup(m => m.GetOntarioStudentCourseCredit(It.IsAny<Guid>())).ReturnsAsync(new Dto.OntarioStudentCourseCredit { Id = request.OntarioStudentCourseCredit.Id });
            domesticContextMock.Setup(m => m.UpdateOntarioStudentCourseCredit(It.IsAny<Dto.OntarioStudentCourseCredit>())).ReturnsAsync(new Dto.OntarioStudentCourseCredit
            {
                ApplicantId = ontarioStudentCourseCredit.ApplicantId,
                CourseCode = ontarioStudentCourseCredit.CourseCode,
                CompletedDate = ontarioStudentCourseCredit.CompletedDate
            });

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsHighSchoolUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new UpdateOntarioStudentCourseCreditHandler(Mock.Of<ILogger<UpdateOntarioStudentCourseCreditHandler>>(), domesticContextMock.Object, _dtoMapper, userAuthorizationMock.Object, _apiMapper, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>().WithMessage($"OntarioStudentCourseCredit.CourseCode already exists for this Applicant: {request.OntarioStudentCourseCredit.CourseCode}");
            domesticContextMock.Verify(e => e.CreateOntarioStudentCourseCredit(It.IsAny<Dto.OntarioStudentCourseCreditBase>()), Times.Never);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateOntarioStudentCourseCredit_ShouldThrow_When_NoAccess()
        {
            // Arrange

            var request = new UpdateOntarioStudentCourseCredit
            {
                User = Mock.Of<IPrincipal>()
            };

            var domesticContextMock = new DomesticContextMock();

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsHighSchoolUser(It.IsAny<IPrincipal>())).Returns(false);
            userAuthorizationMock.Setup(x => x.IsOcasTier2User(It.IsAny<IPrincipal>())).Returns(false);

            var handler = new UpdateOntarioStudentCourseCreditHandler(Mock.Of<ILogger<UpdateOntarioStudentCourseCreditHandler>>(), domesticContextMock.Object, _dtoMapper, userAuthorizationMock.Object, _apiMapper, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ForbiddenException>();
        }
    }
}