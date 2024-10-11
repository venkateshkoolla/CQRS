using System;
using System.Linq;
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
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Handlers
{
    public class UpdateEducationHandlerTests
    {
        private readonly IApiMapper _apiMapper;
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly IDtoMapper _dtoMapper;
        private readonly ILookupsCache _lookupsCache;
        private readonly ModelFakerFixture _models;

        public UpdateEducationHandlerTests()
        {
            _apiMapper = XunitInjectionCollection.AutoMapperFixture.CreateApiMapper();
            _dtoMapper = XunitInjectionCollection.AutoMapperFixture.CreateDtoMapper();
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _models = XunitInjectionCollection.ModelFakerFixture;
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task UpdateEducation_ShouldPass()
        {
            // Arrange
            var education = _models.GetEducation().Generate();

            var request = new UpdateEducation
            {
                User = Mock.Of<IPrincipal>(),
                ApplicantId = education.ApplicantId,
                Education = education,
                EducationId = education.Id
            };

            var dtoEducation = new Dto.Education();
            await _dtoMapper.PatchEducation(dtoEducation, education);
            dtoEducation.Id = education.Id;

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact
            {
                BirthDate = education.AttendedFrom.ToDateTime(Constants.DateFormat.YearMonthDashed).AddYears(-18)
            });
            domesticContextMock.Setup(m => m.GetEducation(It.IsAny<Guid>())).ReturnsAsync(dtoEducation);
            domesticContextMock.Setup(m => m.UpdateEducation(It.IsAny<Dto.Education>())).ReturnsAsync(dtoEducation);

            var handler = new UpdateEducationHandler(Mock.Of<ILogger<UpdateEducationHandler>>(), domesticContextMock.Object, _apiMapper, _dtoMapper, _lookupsCache);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(education);
            domesticContextMock.Verify(e => e.UpdateEducation(It.IsAny<Dto.Education>()), Times.Once);
            domesticContextMock.Verify(e => e.UpdateCompletedSteps(request.ApplicantId), Times.Once);
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task UpdateEducation_ShouldThrow_When_EducationTypeMismatch()
        {
            // Arrange
            var education = _models.GetEducation().Generate("default, Canadian, College");
            var request = new UpdateEducation
            {
                User = Mock.Of<IPrincipal>(),
                ApplicantId = education.ApplicantId,
                Education = education,
                EducationId = education.Id
            };

            var tempEducation = _models.GetEducation().Generate("default, Canadian, University");
            var dtoEducation = new Dto.Education();
            await _dtoMapper.PatchEducation(dtoEducation, tempEducation);
            dtoEducation.ApplicantId = education.ApplicantId;

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact
            {
                BirthDate = education.AttendedFrom.ToDateTime(Constants.DateFormat.YearMonthDashed).AddYears(-18)
            });
            domesticContextMock.Setup(m => m.GetEducation(It.IsAny<Guid>())).ReturnsAsync(dtoEducation);

            var handler = new UpdateEducationHandler(Mock.Of<ILogger<UpdateEducationHandler>>(), domesticContextMock.Object, _apiMapper, _dtoMapper, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be("Cannot change EducationType");
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task UpdateEducation_ShouldThrow_When_OntarioHighSchool_IsDuplicateOen()
        {
            // Arrange
            var education = _models.GetEducation().Generate("default, Canadian, Ontario, Highschool");
            education.OntarioEducationNumber = _models.Oen;
            var request = new UpdateEducation
            {
                User = Mock.Of<IPrincipal>(),
                ApplicantId = education.ApplicantId,
                Education = education,
                EducationId = education.Id
            };

            var dtoEducation = new Dto.Education();
            await _dtoMapper.PatchEducation(dtoEducation, education);

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact
            {
                BirthDate = education.AttendedFrom.ToDateTime(Constants.DateFormat.YearMonthDashed).AddYears(-18)
            });
            domesticContextMock.Setup(m => m.GetEducation(It.IsAny<Guid>())).ReturnsAsync(dtoEducation);
            domesticContextMock.Setup(m => m.IsDuplicateOen(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(true);

            var handler = new UpdateEducationHandler(Mock.Of<ILogger<UpdateEducationHandler>>(), domesticContextMock.Object, _apiMapper, _dtoMapper, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be($"Applicant exists with {education.OntarioEducationNumber}");
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task UpdateEducation_ShouldThrow_When_HighSchool_NotShowInEducation()
        {
            // Arrange
            var education = _models.GetEducation().Generate("default, Canadian, Highschool");
            education.InstituteId = _dataFakerFixture.Faker.PickRandom(_models.AllApplyLookups.HighSchools.Where(h => !h.ShowInEducation)).Id;
            var request = new UpdateEducation
            {
                User = Mock.Of<IPrincipal>(),
                ApplicantId = education.ApplicantId,
                Education = education,
                EducationId = education.Id
            };

            var dtoEducation = new Dto.Education();
            await _dtoMapper.PatchEducation(dtoEducation, education);
            dtoEducation.InstituteId = _dataFakerFixture.Faker.PickRandom(_models.AllApplyLookups.HighSchools.Where(h => h.Id != education.InstituteId)).Id;

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact
            {
                BirthDate = education.AttendedFrom.ToDateTime(Constants.DateFormat.YearMonthDashed).AddYears(-18)
            });
            domesticContextMock.Setup(m => m.GetEducation(It.IsAny<Guid>())).ReturnsAsync(dtoEducation);
            domesticContextMock.Setup(m => m.IsDuplicateOen(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(true);

            var handler = new UpdateEducationHandler(Mock.Of<ILogger<UpdateEducationHandler>>(), domesticContextMock.Object, _apiMapper, _dtoMapper, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be($"'Institute Id' is not an Ontario high school: {education.InstituteId}");
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task UpdateEducation_ShouldThrow_When_University_NotShowInEducation()
        {
            // Arrange
            var education = _models.GetEducation().Generate("default, Canadian, University");
            education.InstituteId = _dataFakerFixture.Faker.PickRandom(_models.AllApplyLookups.Universities.Where(u => !u.ShowInEducation)).Id;
            var request = new UpdateEducation
            {
                User = Mock.Of<IPrincipal>(),
                ApplicantId = education.ApplicantId,
                Education = education,
                EducationId = education.Id
            };

            var dtoEducation = new Dto.Education();
            await _dtoMapper.PatchEducation(dtoEducation, education);
            dtoEducation.InstituteId = _dataFakerFixture.Faker.PickRandom(_models.AllApplyLookups.Universities.Where(u => u.Id != education.InstituteId)).Id;

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact
            {
                BirthDate = education.AttendedFrom.ToDateTime(Constants.DateFormat.YearMonthDashed).AddYears(-18)
            });
            domesticContextMock.Setup(m => m.GetEducation(It.IsAny<Guid>())).ReturnsAsync(dtoEducation);
            domesticContextMock.Setup(m => m.IsDuplicateOen(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(true);

            var handler = new UpdateEducationHandler(Mock.Of<ILogger<UpdateEducationHandler>>(), domesticContextMock.Object, _apiMapper, _dtoMapper, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be($"'Institute Id' is not an Ontario university: {education.InstituteId}");
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task UpdateEducation_ShouldThrow_When_TranscriptRequests_Then_InstituteIdMismatch()
        {
            // Arrange
            var education = _models.GetEducation().Generate("default, Canadian, Ontario, College");
            var request = new UpdateEducation
            {
                User = Mock.Of<IPrincipal>(),
                ApplicantId = education.ApplicantId,
                Education = education,
                EducationId = education.Id
            };

            var dtoEducation = new Dto.Education
            {
                HasTranscripts = true
            };
            await _dtoMapper.PatchEducation(dtoEducation, education);
            dtoEducation.InstituteId = _dataFakerFixture.Faker.PickRandom(_models.AllApplyLookups.Colleges.Where(c => c.Id != education.InstituteId)).Id;

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact
            {
                BirthDate = education.AttendedFrom.ToDateTime(Constants.DateFormat.YearMonthDashed).AddYears(-18)
            });
            domesticContextMock.Setup(m => m.GetEducation(It.IsAny<Guid>())).ReturnsAsync(dtoEducation);

            var handler = new UpdateEducationHandler(Mock.Of<ILogger<UpdateEducationHandler>>(), domesticContextMock.Object, _apiMapper, _dtoMapper, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be("Cannot change province or institute on transcript requested.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task UpdateEducation_ShouldThrow_When_TranscriptRequests_Then_InstituteNameMismatch()
        {
            // Arrange
            var education = _models.GetEducation().Generate("default, Canadian, College");
            var request = new UpdateEducation
            {
                User = Mock.Of<IPrincipal>(),
                ApplicantId = education.ApplicantId,
                Education = education,
                EducationId = education.Id
            };

            var dtoEducation = new Dto.Education
            {
                HasTranscripts = true
            };
            await _dtoMapper.PatchEducation(dtoEducation, education);
            dtoEducation.InstituteName = _dataFakerFixture.Faker.Company.CompanyName();

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact
            {
                BirthDate = education.AttendedFrom.ToDateTime(Constants.DateFormat.YearMonthDashed).AddYears(-18)
            });
            domesticContextMock.Setup(m => m.GetEducation(It.IsAny<Guid>())).ReturnsAsync(dtoEducation);

            var handler = new UpdateEducationHandler(Mock.Of<ILogger<UpdateEducationHandler>>(), domesticContextMock.Object, _apiMapper, _dtoMapper, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be("Cannot change province or institute on transcript requested.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task UpdateEducation_ShouldThrow_When_TranscriptRequests_Then_ProvinceIdMismatch()
        {
            // Arrange
            var education = _models.GetEducation().Generate("default, Canadian, College");
            var request = new UpdateEducation
            {
                User = Mock.Of<IPrincipal>(),
                ApplicantId = education.ApplicantId,
                Education = education,
                EducationId = education.Id
            };

            var dtoEducation = new Dto.Education
            {
                HasTranscripts = true
            };
            await _dtoMapper.PatchEducation(dtoEducation, education);
            dtoEducation.ProvinceId = _dataFakerFixture.Faker.PickRandom(_models.AllApplyLookups.ProvinceStates.Where(c => c.Id != education.ProvinceId)).Id;

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact
            {
                BirthDate = education.AttendedFrom.ToDateTime(Constants.DateFormat.YearMonthDashed).AddYears(-18)
            });
            domesticContextMock.Setup(m => m.GetEducation(It.IsAny<Guid>())).ReturnsAsync(dtoEducation);

            var handler = new UpdateEducationHandler(Mock.Of<ILogger<UpdateEducationHandler>>(), domesticContextMock.Object, _apiMapper, _dtoMapper, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be("Cannot change province or institute on transcript requested.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task UpdateEducation_ShouldThrow_WhenApplicantIdMismatch()
        {
            // Arrange
            var education = _models.GetEducation().Generate();
            var request = new UpdateEducation
            {
                User = Mock.Of<IPrincipal>(),
                ApplicantId = Guid.NewGuid(),
                Education = education
            };

            var dtoEducation = new Dto.Education();
            await _dtoMapper.PatchEducation(dtoEducation, education);
            dtoEducation.InstituteId = _dataFakerFixture.Faker.PickRandom(_models.AllApplyLookups.Colleges.Where(c => c.Id != education.InstituteId)).Id;

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact
            {
                BirthDate = education.AttendedFrom.ToDateTime(Constants.DateFormat.YearMonthDashed).AddYears(-18)
            });
            domesticContextMock.Setup(m => m.GetEducation(It.IsAny<Guid>())).ReturnsAsync(dtoEducation);

            var handler = new UpdateEducationHandler(Mock.Of<ILogger<UpdateEducationHandler>>(), domesticContextMock.Object, _apiMapper, _dtoMapper, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<NotAuthorizedException>()
                .And.Message.Should().Be("Education does not belong to this applicant");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateEducation_ShouldThrow_WhenInvalidDate()
        {
            // Arrange
            var education = _models.GetEducation().Generate();
            var request = new UpdateEducation
            {
                User = Mock.Of<IPrincipal>(),
                ApplicantId = Guid.NewGuid(),
                Education = education
            };
            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact
            {
                BirthDate = education.AttendedFrom.ToDateTime(Constants.DateFormat.YearMonthDashed).AddDays(1)
            });

            var handler = new UpdateEducationHandler(Mock.Of<ILogger<UpdateEducationHandler>>(), domesticContextMock.Object, _apiMapper, _dtoMapper, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>().And.Message.Should().Be($"Education.AttendedFrom must be after applicant's birth: {request.Education.AttendedFrom}");
        }
    }
}
