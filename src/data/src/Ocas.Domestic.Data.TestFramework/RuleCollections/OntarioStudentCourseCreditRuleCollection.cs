using Bogus;

namespace Ocas.Domestic.Data.TestFramework.RuleCollections
{
    public static class OntarioStudentCourseCreditRuleCollection
    {
        public static Faker<TOntarioStudentCourseCreditBase> ApplyOntarioStudentCourseCreditBaseRules<TOntarioStudentCourseCreditBase>(this Faker<TOntarioStudentCourseCreditBase> faker, SeedDataFixture seedDataFixture)
            where TOntarioStudentCourseCreditBase : Models.OntarioStudentCourseCreditBase
        {
            var courseStatuses = seedDataFixture.CourseStatuses;
            var courseTypes = seedDataFixture.CourseTypes;
            var courseDeliveries = seedDataFixture.CourseDeliveries;
            var gradeTypes = seedDataFixture.GradeTypes;

            return faker
                .RuleFor(x => x.Credit, f => f.Random.Decimal(1, 3))
                .RuleFor(x => x.CourseCode, f => f.Random.AlphaNumeric(6))
                .RuleFor(x => x.CourseMident, f => f.Random.Int(950000, 955000).ToString())
                .RuleFor(x => x.Grade, f => f.Random.Int(50, 100).ToString())
                .RuleFor(x => x.CourseStatusId, (f, _) => f.PickRandom(courseStatuses).Id)
                .RuleFor(x => x.CourseTypeId, (f, _) => f.PickRandom(courseTypes).Id)
                .RuleFor(x => x.GradeTypeId, (f, _) => f.PickRandom(gradeTypes).Id)
                .RuleFor(x => x.CourseDeliveryId, (f, _) => f.PickRandom(courseDeliveries).Id);
        }
    }
}