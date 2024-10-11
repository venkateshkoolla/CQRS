using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Services.Mappers;
using Ocas.Domestic.Apply.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Mappers
{
    public class TemplateMapperTests
    {
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly ILookupsCache _lookupsCache;
        private readonly ITemplateMapper _templateMapper;

        public TemplateMapperTests()
        {
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _templateMapper = XunitInjectionCollection.AutoMapperFixture.CreateTemplateMapper();
        }

        [Fact]
        [UnitTest("Mappers")]
        public async Task MapHighSchoolGrades_Should_Pass()
        {
            // Arrange
            var applicantId = Guid.NewGuid();
            var highSchools = await _lookupsCache.GetHighSchools(nameof(Locale.English));
            var highSkillsMajors = await _lookupsCache.GetHighSkillsMajors(nameof(Locale.English));
            var highestEducations = await _lookupsCache.GetHighestEducations(nameof(Locale.English));
            var literacyTests = await _lookupsCache.GetLiteracyTests(nameof(Locale.English));
            var communityInvolvements = await _lookupsCache.GetCommunityInvolvements(nameof(Locale.English));
            var courseStatuses = await _lookupsCache.GetCourseStatuses(nameof(Locale.English));
            var courseTypes = await _lookupsCache.GetCourseTypes(nameof(Locale.English));
            var courseDeliveries = await _lookupsCache.GetCourseDeliveries(nameof(Locale.English));
            var gradeTypes = await _lookupsCache.GetGradeTypes(nameof(Locale.English));

            var hsPartnerId1 = _dataFakerFixture.Faker.PickRandom(highSchools).Id;
            var hsPartnerId2 = _dataFakerFixture.Faker.PickRandom(highSchools.Where(x => x.Id != hsPartnerId1)).Id;

            var acadRec = new Dto.AcademicRecord
            {
                Id = Guid.NewGuid(),
                ApplicantId = applicantId,
                DateCredentialAchieved = DateTime.UtcNow.AddDays(-300)
            };
            var transcripts = new List<Dto.Transcript>
            {
                new Dto.Transcript { Id = Guid.NewGuid(), ContactId = applicantId, TranscriptType = TranscriptType.OntarioHighSchoolTranscript, PartnerId = hsPartnerId1 },
                new Dto.Transcript { Id = Guid.NewGuid(), ContactId = applicantId, TranscriptType = TranscriptType.OntarioHighSchoolTranscript, PartnerId = hsPartnerId2 }
            };

            var grades = new List<Dto.OntarioStudentCourseCredit>
            {
                new Dto.OntarioStudentCourseCredit { ApplicantId = applicantId, Id = Guid.NewGuid(), CourseMident = "MMCC", CompletedDate = "201904", Notes = "B", Grade = "80" },
                new Dto.OntarioStudentCourseCredit { ApplicantId = applicantId, Id = Guid.NewGuid(), CourseMident = "CCMM", CompletedDate = "201905", Credit = 1.5M, Notes = "C", Grade = "87" }
            };

            var totCredits = grades.Select(x => x.Credit ?? 0M).DefaultIfEmpty(0M).Sum().ToString(new CultureInfo(Constants.Localization.EnglishCanada));

            //Act
            var highSchoolGradesViewModel = _templateMapper.MapHighSchoolGrades(
                acadRec,
                transcripts,
                grades,
                highSchools,
                highSkillsMajors,
                highestEducations,
                literacyTests,
                communityInvolvements,
                courseStatuses,
                courseTypes,
                courseDeliveries,
                gradeTypes,
                Constants.Localization.EnglishCanada);

            //Assert
            highSchoolGradesViewModel.Should().NotBeNull();
            highSchoolGradesViewModel.TotalCredits.Should().BeEquivalentTo(totCredits);
            grades.Should().SatisfyRespectively(
                firstRec =>
                    {
                        firstRec.CourseMident.Should().BeEquivalentTo(highSchoolGradesViewModel.Grades.FirstOrDefault().MidentCode);
                        firstRec.Notes.Should().BeEquivalentTo(highSchoolGradesViewModel.Grades.FirstOrDefault().Notes);
                        firstRec.Grade.Should().BeEquivalentTo(highSchoolGradesViewModel.Grades.FirstOrDefault().Mark);
                        firstRec.CompletedDate.Should().BeEquivalentTo(highSchoolGradesViewModel.Grades.FirstOrDefault().CompletedDate);
                    },
                    second =>
                    {
                        second.CourseMident.Should().BeEquivalentTo(highSchoolGradesViewModel.Grades.LastOrDefault().MidentCode);
                        second.Notes.Should().BeEquivalentTo(highSchoolGradesViewModel.Grades.LastOrDefault().Notes);
                        second.Grade.Should().BeEquivalentTo(highSchoolGradesViewModel.Grades.LastOrDefault().Mark);
                        second.CompletedDate.Should().BeEquivalentTo(highSchoolGradesViewModel.Grades.LastOrDefault().CompletedDate);
                    });
        }
    }
}
