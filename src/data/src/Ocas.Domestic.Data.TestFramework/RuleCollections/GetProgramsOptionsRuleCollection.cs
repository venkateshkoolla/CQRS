using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;

namespace Ocas.Domestic.Data.TestFramework.RuleCollections
{
    public static class GetProgramsOptionsRuleCollection
    {
        public static Faker<TGetProgramsOptions> ApplyGetProgramsOptionsRules<TGetProgramsOptions>(this Faker<TGetProgramsOptions> faker, SeedDataFixture seedDataFixture)
            where TGetProgramsOptions : Models.GetProgramsOptions
        {
            var open = seedDataFixture.SchoolStatuses.Single(x => x.Code == TestConstants.SchoolStatuses.Open);
            var openColleges = seedDataFixture.Colleges.Where(x => x.SchoolStatusId == open.Id).ToList();

            var collegeId = ValidCollegeId(seedDataFixture, openColleges);

            return faker
                .RuleFor(o => o.CollegeId, _ => collegeId)
                .RuleFor(o => o.CampusId, f => f.PickRandom(seedDataFixture.Campuses.Where(s => s.ParentId == collegeId).Select(x => x.Id)))
                .RuleFor(o => o.ApplicationCycleId, () => null)
                .RuleFor(o => o.DeliveryId, () => null);
        }

        private static Guid ValidCollegeId(SeedDataFixture seedDataFixture, List<Models.College> openColleges)
        {
            var campusRand = new Random();
            var campusCounter = 0;
            while (campusCounter < 10)
            {
                var collegeId = seedDataFixture.Colleges.ElementAt(campusRand.Next(openColleges.Count())).Id;
                if (seedDataFixture.Campuses.Any(s => s.ParentId == collegeId) && seedDataFixture.CollegeApplicationCycles.Any(s => s.CollegeId == collegeId))
                {
                    return collegeId;
                }

                campusCounter++;
            }

            throw new Exception("A college with campuses or application cycles could not be found");
        }
    }
}
