using System;
using System.Linq;
using Bogus;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.TestFramework.RuleCollections
{
    public static class ProgramChoiceRuleCollection
    {
        public static Faker<TProgramChoiceBase> ApplyProgramChoiceBaseRules<TProgramChoiceBase>(this Faker<TProgramChoiceBase> faker, AllLookups lookups)
            where TProgramChoiceBase : ProgramChoiceBase
        {
            var entryLevels = lookups.EntryLevels;

            return faker
                .RuleFor(x => x.ApplicantId, _ => Guid.NewGuid())
                .RuleFor(x => x.ApplicationId, _ => Guid.NewGuid())
                .RuleFor(x => x.IntakeId, _ => Guid.NewGuid())
                .RuleFor(x => x.EntryLevelId, f => f.PickRandom(entryLevels).Id)
                .RuleFor(x => x.PreviousYearApplied, f => f.Date.Past(6, DateTime.UtcNow.AddYears(-1)).Year)
                .RuleFor(x => x.PreviousYearAttended, f => f.Date.Past(4, DateTime.UtcNow.AddYears(-1)).Year);
        }

        public static Faker<TProgramChoice> ApplyProgramChoiceRules<TProgramChoice>(this Faker<TProgramChoice> faker, AllLookups lookups)
        where TProgramChoice : ProgramChoice
        {
            var offerStatuses = lookups.OfferStatuses;
            var campuses = lookups.Campuses;
            var colleges = lookups.Colleges.Where(c => c.IsOpen).WithCampuses(campuses);
            var delivery = lookups.StudyMethods;
            var sources = lookups.Sources.Where(s => s.Code == Constants.Sources.A2C2 || s.Code == Constants.Sources.CBUI);

            return faker
                .ApplyProgramChoiceBaseRules(lookups)
                .RuleFor(x => x.Id, _ => Guid.NewGuid())
                .RuleFor(x => x.OfferStatusId, f => f.PickRandom(offerStatuses).Id)
                .RuleFor(x => x.IntakeStartDate, f => f.Date.Past(10).ToString(Constants.DateFormat.CcExpiry))
                .RuleFor(x => x.DeliveryId, f => f.PickRandom(delivery).Id)
                .RuleFor(x => x.CollegeId, f => f.PickRandom(colleges).Id)
                .RuleFor(x => x.CollegeName, (_, o) => colleges.First(c => c.Id == o.CollegeId).Name)
                .RuleFor(x => x.CampusId, (f, o) => f.PickRandom(campuses.FirstOrDefault(x => x.CollegeId == o.CollegeId)?.Id))
                .RuleFor(x => x.CampusName, (_, o) => campuses.FirstOrDefault(c => c.Id == o.CampusId)?.Name)
                .RuleFor(x => x.SourceId, f => f.PickRandom(sources).Id)
                .RuleFor(x => x.ProgramId, _ => null);
        }
    }
}
