using System;
using System.Linq;
using Bogus;
using FluentAssertions;
using Moq;
using Ocas.Domestic.Apply.Admin.Api.Services.Mappers;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;
using AdminTestFramework = Ocas.Domestic.Apply.Admin.TestFramework;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Mappers
{
    public partial class ApiMapperTests
    {
        private readonly IApiMapper _apiMapper;
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly AdminTestFramework.ModelFakerFixture _models;
        private readonly ITranslationsCache _translationsCache;

        public ApiMapperTests()
        {
            _apiMapper = XunitInjectionCollection.AutoMapperFixture.CreateApiMapper();
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _models = XunitInjectionCollection.ModelFakerFixture;
            _translationsCache = Mock.Of<ITranslationsCache>();
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapOntarioStudentCourseCredit_ShouldPass()
        {
            // Arrange
            var highSchool = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.HighSchools.Where(h => !string.IsNullOrEmpty(h.Mident) && h.Mident.Length == 6));
            var notes = _models.AllAdminLookups.OstNotes;
            var gradeTypes = _models.AllAdminLookups.GradeTypes;

            var dtoOntarioStudentCourseCredit = new Faker<Dto.OntarioStudentCourseCredit>()
                .RuleFor(o => o.Id, f => f.Random.Uuid())
                .RuleFor(o => o.ApplicantId, f => f.Random.Uuid())
                .RuleFor(o => o.GradeTypeId, f => f.PickRandom(gradeTypes.Where(g => g.Code != Constants.OntarioHighSchool.GradeType.Final)).Id)
                .RuleFor(o => o.CompletedDate, (f, o) => gradeTypes.First(g => g.Id == o.GradeTypeId).Code == Constants.OntarioHighSchool.GradeType.Final
                                                            ? f.Date.Past(1).ToString(Constants.DateFormat.CompletedDate)
                                                            : f.Date.Future(1).ToString(Constants.DateFormat.CompletedDate))
                .RuleFor(o => o.CourseCode, f => f.Random.AlphaNumeric(6))
                .RuleFor(o => o.CourseMident, _ => highSchool.Mident)
                .RuleFor(o => o.Credit, f => Math.Round(f.Random.Decimal(), 2))
                .RuleFor(o => o.Grade, f => f.Random.Number(100).ToString())
                .RuleFor(o => o.Notes, f => string.Concat(f.PickRandom(notes, 4).Select(n => n.Code)))
                .RuleFor(o => o.TranscriptId, f => f.Random.Uuid())
                .RuleFor(o => o.CourseStatusId, f => f.Random.Uuid())
                .RuleFor(o => o.CourseTypeId, f => f.Random.Uuid())
                .RuleFor(o => o.CourseDeliveryId, f => f.Random.Uuid())
                .Generate();

            // Act
            var mapped = _apiMapper.MapOntarioStudentCourseCredit(dtoOntarioStudentCourseCredit);

            // Assert
            mapped.Id.Should().Be(dtoOntarioStudentCourseCredit.Id);
            mapped.ApplicantId.Should().Be(dtoOntarioStudentCourseCredit.ApplicantId);
            mapped.CompletedDate.Should().Be(dtoOntarioStudentCourseCredit.CompletedDate.ToNullableDateTime(Constants.DateFormat.CompletedDate).ToStringOrDefault(Constants.DateFormat.YearMonthDashed));
            mapped.CourseCode.Should().Be(dtoOntarioStudentCourseCredit.CourseCode);
            mapped.CourseMident.Should().Be(highSchool.Mident);
            mapped.Credit.Should().Be(dtoOntarioStudentCourseCredit.Credit.Value);
            mapped.Grade.Should().Be(dtoOntarioStudentCourseCredit.Grade);
            mapped.Notes.Should().BeEquivalentTo(dtoOntarioStudentCourseCredit.Notes.Select(c => c.ToString()).ToList());
            mapped.TranscriptId.Should().Be(dtoOntarioStudentCourseCredit.TranscriptId.Value);
            mapped.CourseStatusId.Should().Be(dtoOntarioStudentCourseCredit.CourseStatusId.Value);
            mapped.CourseTypeId.Should().Be(dtoOntarioStudentCourseCredit.CourseTypeId.Value);
            mapped.GradeTypeId.Should().Be(dtoOntarioStudentCourseCredit.GradeTypeId.Value);
            mapped.CourseDeliveryId.Should().Be(dtoOntarioStudentCourseCredit.CourseDeliveryId.Value);
        }
    }
}
