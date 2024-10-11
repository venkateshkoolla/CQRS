using System;
using System.Linq;
using Bogus;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.TestFramework.RuleCollections
{
    public static class OfferRuleCollection
    {
        public static Faker<TOffer> ApplyOfferRules<TOffer>(this Faker<TOffer> faker, AllLookups lookups)
            where TOffer : Dto.Offer
        {
            var entryLevels = lookups.EntryLevels;
            var campuses = lookups.Campuses;
            var colleges = lookups.Colleges.Where(c => c.IsOpen).WithCampuses(campuses);
            var offerStatuses = lookups.OfferStatuses;
            var studyMethods = lookups.StudyMethods;

            return faker
                .RuleFor(x => x.Id, _ => Guid.NewGuid())
                .RuleFor(x => x.ApplicantId, _ => Guid.NewGuid())
                .RuleFor(x => x.ApplicationId, _ => Guid.NewGuid())
                .RuleFor(x => x.ApplicationStatusId, lookups.ApplicationStatuses.Single(x => x.Code == Constants.ApplicationStatuses.Active).Id)
                .RuleFor(x => x.HardExpiryDate, DateTime.UtcNow.AddDays(1).ToDateInEstAsUtc())
                .RuleFor(x => x.OfferLockReleaseDate, () => null)
                .RuleFor(x => x.OfferStatusId, offerStatuses.Single(x => x.Code == Constants.Offers.Status.NoDecision).Id)
                .RuleFor(x => x.OfferStateId, lookups.OfferStates.Single(x => x.Code == Constants.Offers.State.Active).Id)
                .RuleFor(x => x.EntryLevelId, f => f.PickRandom(entryLevels).Id)
                .RuleFor(x => x.CollegeId, f => f.PickRandom(colleges).Id)
                .RuleFor(x => x.CampusId, (f, o) => f.PickRandom(campuses.FirstOrDefault(x => x.CollegeId == o.CollegeId)?.Id))
                .RuleFor(x => x.OfferStudyMethodId, f => f.PickRandom(studyMethods).Id)
                .RuleSet("accepted", set => set.RuleFor(x => x.OfferStatusId, offerStatuses.Single(x => x.Code == Constants.Offers.Status.Accepted).Id))
                .RuleSet("declined", set => set.RuleFor(x => x.OfferStatusId, offerStatuses.Single(x => x.Code == Constants.Offers.Status.Declined).Id));
        }
    }
}
