using System;
using System.Linq;
using Bogus;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.TestFramework.RuleCollections
{
    public static class TranscriptRequestRuleCollection
    {
        public static Faker<TTranscriptRequest> ApplyTranscriptRequestRules<TTranscriptRequest>(this Faker<TTranscriptRequest> faker, AllLookups lookups)
            where TTranscriptRequest : TranscriptRequestBase
        {
            var instituteType = new Faker().PickRandom(lookups.InstituteTypes);

            var hsInstitute = lookups.HighSchools.Where(h => h.HasEtms);
            var collegeInstitute = lookups.Colleges.Where(c => c.HasEtms);
            var uniInstitute = lookups.Universities.Where(u => u.HasEtms);
            var transmissions = lookups.TranscriptTransmissions;
            var transmissionSendNow = transmissions.First(x => x.Code == Constants.TranscriptTransmissions.SendTranscriptNow);

            return faker
                .RuleFor(x => x.ApplicationId, _ => Guid.NewGuid())
                .RuleFor(x => x.FromInstituteId, f =>
                {
                    switch (instituteType.Code)
                    {
                        case Constants.InstituteTypes.HighSchool:
                            return f.PickRandom(hsInstitute).Id;
                        case Constants.InstituteTypes.College:
                            return f.PickRandom(collegeInstitute).Id;
                        case Constants.InstituteTypes.University:
                            return f.PickRandom(uniInstitute).Id;
                        default:
                            return Guid.NewGuid();
                    }
                })
                .RuleFor(x => x.ToInstituteId, (f, o) => instituteType.Code == Constants.InstituteTypes.HighSchool ? (Guid?)null : f.PickRandom(collegeInstitute.Where(c => c.Id != o.FromInstituteId)).Id)
                .RuleFor(x => x.TransmissionId, f => instituteType.Code == Constants.InstituteTypes.HighSchool ? transmissionSendNow.Id : f.PickRandom(transmissions.Where(x => x.InstituteTypeId == null || x.InstituteTypeId == instituteType.Id)).Id)
                .RuleSet("Highschool", set =>
                {
                    set.RuleFor(x => x.FromInstituteId, f => f.PickRandom(hsInstitute).Id)
                    .RuleFor(x => x.ToInstituteId, _ => null)
                    .RuleFor(x => x.TransmissionId, _ => transmissionSendNow.Id);
                })
                .RuleSet("College", set =>
                {
                    set.RuleFor(x => x.FromInstituteId, f => f.PickRandom(collegeInstitute).Id)
                    .RuleFor(x => x.ToInstituteId, (f, o) => f.PickRandom(collegeInstitute.Where(c => c.Id != o.FromInstituteId)).Id)
                    .RuleFor(x => x.TransmissionId, f => f.PickRandom(transmissions.Where(x => x.InstituteTypeId == null || x.InstituteTypeId == lookups.InstituteTypes.First(i => i.Code == Constants.InstituteTypes.College).Id)).Id);
                })
                .RuleSet("University", set =>
                {
                    set.RuleFor(x => x.FromInstituteId, f => f.PickRandom(uniInstitute).Id)
                    .RuleFor(x => x.ToInstituteId, f => f.PickRandom(collegeInstitute).Id)
                    .RuleFor(x => x.TransmissionId, f => f.PickRandom(transmissions.Where(x => x.InstituteTypeId == null || x.InstituteTypeId == lookups.InstituteTypes.First(i => i.Code == Constants.InstituteTypes.University).Id)).Id);
                });
        }
    }
}
