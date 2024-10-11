using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;

namespace Ocas.Domestic.Data.TestFramework.RuleCollections
{
    public static class ProgramRuleCollection
    {
        public static Faker<TProgramBase> ApplyProgramBaseRules<TProgramBase>(this Faker<TProgramBase> faker, SeedDataFixture seedDataFixture)
            where TProgramBase : Models.ProgramBase
        {
            var open = seedDataFixture.SchoolStatuses.Single(x => x.Code == TestConstants.SchoolStatuses.Open);
            var openColleges = seedDataFixture.Colleges.Where(x => x.SchoolStatusId == open.Id).ToList();
            var activeCycleId = seedDataFixture.ApplicationCycleStatuses.Single(x => x.Code == TestConstants.ApplicationCycleStatuses.Active).Id;
            var activeAppCycles = seedDataFixture.ApplicationCycles.Where(x => x.StatusId == activeCycleId).ToList();
            var collegeAppCycles = seedDataFixture.CollegeApplicationCycles.Where(s => activeAppCycles.Any(x => x.Id == s.ApplicationCycleId));

            var collegeId = ValidCollegeId(seedDataFixture, openColleges);

            return faker
                .RuleFor(o => o.CollegeId, _ => collegeId)
                .RuleFor(o => o.CollegeApplicationCycleId, f => f.PickRandom(collegeAppCycles.Where(s => s.CollegeId == collegeId).Select(x => x.Id)))
                .RuleFor(o => o.CampusId, f => f.PickRandom(seedDataFixture.Campuses.Where(s => s.ParentId == collegeId).Select(x => x.Id)))
                .RuleFor(o => o.Code, f => f.Random.AlphaNumeric(8).ToUpper(System.Globalization.CultureInfo.InvariantCulture))
                .RuleFor(o => o.Title, f => f.Name.JobTitle())
                .RuleFor(o => o.Name, (_, o) => o.Title)
                .RuleFor(o => o.DeliveryId, f => f.PickRandom(seedDataFixture.OfferStudyMethods).Id)
                .RuleFor(o => o.ProgramTypeId, f => f.PickRandom(seedDataFixture.ProgramTypes).Id)
                .RuleFor(o => o.Length, f => f.Random.Number(10))
                .RuleFor(o => o.LengthTypeId, f => f.PickRandom(seedDataFixture.UnitOfMeasures).Id)
                .RuleFor(o => o.McuCodeId, f => f.PickRandom(seedDataFixture.McuCodes).Id)
                .RuleFor(o => o.CredentialId, f => f.PickRandom(seedDataFixture.Credentials).Id)
                .RuleFor(o => o.DefaultEntryLevelId, f => f.PickRandom(seedDataFixture.EntryLevels).Id)
                .RuleFor(o => o.StudyAreaId, f => f.PickRandom(seedDataFixture.StudyAreas).Id)
                .RuleFor(o => o.HighlyCompetitiveId, f => f.PickRandom(seedDataFixture.HighlyCompetitives).Id)
                .RuleFor(o => o.LanguageId, f => f.PickRandom(seedDataFixture.ProgramLanguages).Id)
                .RuleFor(o => o.LevelId, f => f.PickRandom(seedDataFixture.ProgramLevels).Id)
                .RuleFor(o => o.PromotionId, f => f.PickRandom(seedDataFixture.Promotions).Id)
                .RuleFor(o => o.AdultTrainingId, f => f.PickRandom(seedDataFixture.AdultTrainings).Id)
                .RuleFor(o => o.SpecialCodeId, _ => null)
                .RuleFor(o => o.ApsNumber, f => f.Random.Number(1500))
                .RuleFor(o => o.MinistryApprovalId, f => f.PickRandom(seedDataFixture.MinistryApprovals).Id)
                .RuleFor(o => o.Url, f => f.Internet.Url())
                .RuleFor(o => o.ProgramCategory1Id, () =>
                {
                    var rand = new Random();
                    var counter = 0;

                    while (counter < 10)
                    {
                        var categoryId = seedDataFixture.ProgramCategories.ElementAt(rand.Next(seedDataFixture.ProgramCategories.Count())).Id;
                        if (seedDataFixture.ProgramSubCategories.Any(s => s.ProgramCategoryId == categoryId))
                        {
                            return categoryId;
                        }

                        counter++;
                    }

                    throw new Exception("A program category with sub-categories could not be found");
                })
                .RuleFor(o => o.ProgramSubCategory1Id, (f, o) => f.PickRandom(seedDataFixture.ProgramSubCategories.Where(s => s.ProgramCategoryId == o.ProgramCategory1Id).Select(x => x.Id)))
                .RuleFor(o => o.ProgramCategory2Id, _ => null)
                .RuleFor(o => o.ProgramSubCategory2Id, _ => null);
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
