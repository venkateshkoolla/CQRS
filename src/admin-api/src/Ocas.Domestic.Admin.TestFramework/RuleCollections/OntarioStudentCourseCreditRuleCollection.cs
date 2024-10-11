using System;
using System.Linq;
using Bogus;
using Ocas.Domestic.Apply.Admin.Models;

namespace Ocas.Domestic.Apply.Admin.TestFramework.RuleCollections
{
    public static class OntarioStudentCourseCreditRuleCollection
    {
        public static Faker<TOntarioStudentCourseCredit> ApplyOntarioStudentCourseCreditRules<TOntarioStudentCourseCredit>(this Faker<TOntarioStudentCourseCredit> faker, AllLookups lookups)
            where TOntarioStudentCourseCredit : OntarioStudentCourseCreditBase
        {
            var courseDeliveries = lookups.CourseDeliveries;
            var courseStatuses = lookups.CourseStatuses;
            var courseTypes = lookups.CourseTypes;
            var gradeTypes = lookups.GradeTypes;
            var highSchools = lookups.HighSchools.Where(h => !string.IsNullOrEmpty(h.Mident) && h.Mident.Length == 6);
            var notes = lookups.OstNotes;
            const string courseCode = "0801A";

            return faker
                .RuleFor(o => o.ApplicantId, _ => Guid.NewGuid())
                .RuleFor(x => x.CourseDeliveryId, f => f.PickRandom(courseDeliveries).Id)
                .RuleFor(x => x.CourseStatusId, f => f.PickRandom(courseStatuses.Where(c => c.Code != Constants.OntarioHighSchool.CourseStatus.Delete)).Id)
                .RuleFor(x => x.CourseTypeId, f => f.PickRandom(courseTypes).Id)
                .RuleFor(x => x.GradeTypeId, f => f.PickRandom(gradeTypes.Where(g => g.Code != Constants.OntarioHighSchool.GradeType.Final)).Id)
                .RuleFor(x => x.CourseCode, courseCode)
                .RuleFor(x => x.Grade, f => f.Random.Number(100).ToString())
                .RuleFor(x => x.Notes, f => f.PickRandom(notes, 4).Select(n => n.Code).ToList())
                .RuleFor(x => x.Credit, f => Math.Round(f.Random.Decimal(), 2))
                .RuleFor(x => x.CourseMident, f => f.PickRandom(highSchools).Mident)
                .RuleFor(x => x.SupplierMident, (_, o) => o.CourseMident)
                .RuleFor(x => x.CompletedDate, (f, o) => gradeTypes.First(g => g.Id == o.GradeTypeId).Code == Constants.OntarioHighSchool.GradeType.Final
                                                            ? f.Date.Past(1).ToString(Constants.DateFormat.YearMonthDashed)
                                                            : f.Date.Future(1).ToString(Constants.DateFormat.YearMonthDashed))
                .RuleSet("GradeFinal", set =>
                    set.RuleFor(x => x.GradeTypeId, _ => gradeTypes.First(g => g.Code == Constants.OntarioHighSchool.GradeType.Final).Id)
                    .RuleFor(x => x.CompletedDate, f => f.Date.Past(1).ToString(Constants.DateFormat.YearMonthDashed)));
        }
    }
}
