using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Models.Lookups;
using Ocas.Domestic.Apply.Models.Templates;
using Ocas.Domestic.Enums;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Services.Mappers
{
    public class TemplateMapper : ITemplateMapper
    {
        public HighSchoolGradesViewModel MapHighSchoolGrades(
            Dto.AcademicRecord academicRecord,
            IList<Dto.Transcript> transcripts,
            IList<Dto.OntarioStudentCourseCredit> grades,
            IList<HighSchool> highSchools,
            IList<LookupItem> highSkillsMajors,
            IList<LookupItem> highestEducations,
            IList<LookupItem> literacyTests,
            IList<LookupItem> communityInvolvements,
            IList<LookupItem> courseStatuses,
            IList<LookupItem> courseTypes,
            IList<LookupItem> courseDeliveries,
            IList<LookupItem> gradeTypes,
            string locale)
        {
            var schoolsAttended = transcripts
                    .Where(x => x.TranscriptType == TranscriptType.OntarioHighSchoolTranscript)
                    .Select(x => highSchools.FirstOrDefault(y => y.Id == x.PartnerId))
                    .Select(x => x is null ? string.Empty : $"{x.Name} - {x.Mident}")
                    .ToList();

            var viewModel = new HighSchoolGradesViewModel
            {
                CommunityInvolvement = communityInvolvements.FirstOrDefault(x => x.Id == academicRecord.CommunityInvolvementId)?.Label,
                DateCredentialAchieved = academicRecord.DateCredentialAchieved.ToStringOrDefault(),
                HighestEducation = highestEducations.FirstOrDefault(x => x.Id == academicRecord.HighestEducationId)?.Label,
                HighSkillsMajor = highSkillsMajors.FirstOrDefault(x => x.Id == academicRecord.HighSkillsMajorId)?.Label,
                Literacy = literacyTests.FirstOrDefault(x => x.Id == academicRecord.LiteracyTestId)?.Label,
                Grades = new List<HighSchoolGradeViewModel>(),
                SchoolsAttended = schoolsAttended,
                TotalCredits = grades
                    .Select(x => x.Credit ?? 0M)
                    .DefaultIfEmpty(0M)
                    .Sum()
                    .ToString(new CultureInfo(locale))
            };

            foreach (var grade in grades)
            {
                viewModel.Grades.Add(new HighSchoolGradeViewModel
                {
                    CompletedDate = grade.CompletedDate,
                    CourseCode = grade.CourseCode,
                    CourseStatus = courseStatuses.FirstOrDefault(x => x.Id == grade.CourseStatusId)?.Label,
                    CourseType = courseTypes.FirstOrDefault(x => x.Id == grade.CourseTypeId)?.Label,
                    Credit = grade.Credit.ToString(),
                    DeliveryType = courseDeliveries.FirstOrDefault(x => x.Id == grade.CourseDeliveryId)?.Label,
                    Mark = grade.Grade,
                    MarkType = gradeTypes.FirstOrDefault(x => x.Id == grade.GradeTypeId)?.Label,
                    MidentCode = grade.CourseMident,
                    Notes = grade.Notes
                });
            }

            return viewModel;
        }

        public StandardizedTestViewModel MapStandardizedTest(Dto.Test model, IList<Country> countries, IList<ProvinceState> provinceStates, IList<City> cities, IList<LookupItem> testTypes)
        {
            var viewModel = new StandardizedTestViewModel
            {
                ApplicationCycle = model.ApplicationCycleName,
                City = model.CityName,
                DateTestTaken = model.DateTestTaken.ToStringOrDefault(),
                NormingGroup = model.NormingGroupName,
                ProvinceState = model.ProvinceStateName,
                TestDescription = model.Description
            };

            if (model.Details?.Any() != true)
            {
                throw new ValidationException("Invalid data: standardized tests must have details.");
            }

            viewModel.Details = new List<StandardizedTestDetail>();

            foreach (var detail in model.Details)
            {
                viewModel.Details.Add(new StandardizedTestDetail
                {
                    Description = detail.Description,
                    Percentile = detail.Percentile.ToString(),
                    RawScore = detail.Grade.ToString()
                });
            }

            if (!model.CountryId.IsEmpty())
            {
                var country = countries.FirstOrDefault(x => x.Id == model.CountryId);
                viewModel.Country = country?.Label;
            }

            if (!model.CityId.IsEmpty())
            {
                var city = cities.FirstOrDefault(x => x.Id == model.CityId);
                viewModel.City = city?.Label;
            }

            if (!model.ProvinceStateId.IsEmpty())
            {
                var province = provinceStates.FirstOrDefault(x => x.Id == model.ProvinceStateId);
                viewModel.ProvinceState = province?.Label;
            }

            if (!model.TestTypeId.IsEmpty())
            {
                var testType = testTypes.First(x => x.Id == model.TestTypeId);
                viewModel.TestType = testType?.Label;
                viewModel.TestTypeCode = testType?.Code;
            }

            return viewModel;
        }
    }
}
