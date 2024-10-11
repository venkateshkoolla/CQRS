using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Ocas.Domestic.Apply.Admin.Models;

namespace Ocas.Domestic.Apply.Admin.TestFramework.RuleCollections
{
    public static class ProgramRuleCollection
    {
        public static Faker<TProgramBase> ApplyProgramBaseRules<TProgramBase>(this Faker<TProgramBase> faker, AllLookups lookups)
            where TProgramBase : ProgramBase
        {
            var colleges = lookups.Colleges;
            var seneca = colleges.Single(x => x.Code == "SENE");
            var campuses = lookups.Campuses;
            var senecaCampuses = campuses.Where(x => x.CollegeId == seneca.Id).ToList();
            var applicationCycles = lookups.ApplicationCycles;
            var activeApplicationCycles = applicationCycles.Where(x => x.Status != Constants.ApplicationCycleStatuses.Previous);
            var collegeApplicationCycles = lookups.CollegeApplicationCycles;
            var senecaApplicationCycles = collegeApplicationCycles.Where(x => x.CollegeId == seneca.Id && activeApplicationCycles.Any(y => y.Id == x.MasterId)).ToList();

            var adultTrainings = lookups.AdultTrainings;
            var offerStudyMethods = lookups.StudyMethods;
            var credentials = lookups.ProgramCredentials;
            var entryLevels = lookups.EntryLevels;
            var highlyCompetitives = lookups.HighlyCompetitives;
            var mcuCodes = lookups.McuCodes;
            var ministryApprovals = lookups.MinistryApprovals;
            var programTypes = lookups.ProgramTypes;
            var programLanguages = lookups.ProgramLanguages;
            var programLengths = lookups.ProgramLengths;
            var programLevels = lookups.ProgramLevels;
            var programSubCategories = lookups.ProgramSubCategories;
            var promotions = lookups.Promotions;
            var studyAreas = lookups.StudyAreas;

            return faker
                .RuleFor(o => o.CollegeId, _ => seneca.Id)
                .RuleFor(o => o.CampusId, f => f.PickRandom(senecaCampuses).Id)
                .RuleFor(o => o.ApplicationCycleId, f => f.PickRandom(senecaApplicationCycles).Id)
                .RuleFor(o => o.Code, f => f.Random.AlphaNumeric(8).ToUpperInvariant())
                .RuleFor(o => o.Title, f => f.Name.JobTitle())
                .RuleFor(o => o.DeliveryId, f => f.PickRandom(offerStudyMethods).Id)
                .RuleFor(o => o.ProgramTypeId, f => f.PickRandom(programTypes).Id)
                .RuleFor(o => o.Length, f => f.Random.Number(99))
                .RuleFor(o => o.LengthTypeId, f => f.PickRandom(programLengths).Id)
                .RuleFor(o => o.McuCode, f => f.PickRandom(mcuCodes).Code)
                .RuleFor(o => o.CredentialId, f => f.PickRandom(credentials).Id)
                .RuleFor(o => o.DefaultEntryLevelId, f => f.PickRandom(entryLevels).Id)
                .RuleFor(o => o.EntryLevelIds, (_, o) => ValidEntryLevels(o.DefaultEntryLevelId, lookups))
                .RuleFor(o => o.StudyAreaId, f => f.PickRandom(studyAreas).Id)
                .RuleFor(o => o.HighlyCompetitiveId, f => f.PickRandom(highlyCompetitives).Id)
                .RuleFor(o => o.LanguageId, f => f.PickRandom(programLanguages).Id)
                .RuleFor(o => o.LevelId, f => f.PickRandom(programLevels).Id)
                .RuleFor(o => o.PromotionId, f => f.PickRandom(promotions).Id)
                .RuleFor(o => o.AdultTrainingId, f => f.PickRandom(adultTrainings).Id)
                .RuleFor(o => o.SpecialCode, () => null)
                .RuleFor(o => o.ApsNumber, f => f.Random.Number(1500))
                .RuleFor(o => o.MinistryApprovalId, f => f.PickRandom(ministryApprovals).Id)
                .RuleFor(o => o.Url, f => f.Internet.Url())
                .RuleFor(o => o.ProgramCategory1Id, ValidProgramCategory(lookups))
                .RuleFor(o => o.ProgramSubCategory1Id, (f, o) => f.PickRandom(programSubCategories.Where(s => s.CategoryId == o.ProgramCategory1Id).Select(x => x.Id).ToList()))
                .RuleFor(o => o.ProgramCategory2Id, () => null)
                .RuleFor(o => o.ProgramSubCategory2Id, () => null);
        }

        private static IList<Guid> ValidEntryLevels(Guid defaultEntryLevelId, AllLookups lookups)
        {
            return lookups.EntryLevels.Where(p => string.Compare(lookups.EntryLevels.Single(e => e.Id == defaultEntryLevelId).Code, p.Code, StringComparison.InvariantCultureIgnoreCase) <= 0).Select(x => x.Id).ToList();
        }

        private static Guid ValidProgramCategory(AllLookups lookups)
        {
            var rand = new Random();
            var counter = 0;

            while (counter < 10)
            {
                var categoryId = lookups.ProgramCategories.ElementAt(rand.Next(lookups.ProgramCategories.Count())).Id;
                if (lookups.ProgramSubCategories.Any(s => s.CategoryId == categoryId))
                {
                    return categoryId;
                }

                counter++;
            }

            throw new Exception("A program category with sub-categories could not be found to create a program");
        }
    }
}
