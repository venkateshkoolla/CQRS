using System;
using System.Linq;
using Bogus;

namespace Ocas.Domestic.Data.TestFramework.RuleCollections
{
    public static class TranscriptRequestRuleCollection
    {
        public static Faker<TTranscriptRequestBase> ApplyTranscriptRequestBaseRules<TTranscriptRequestBase>(this Faker<TTranscriptRequestBase> faker, SeedDataFixture seedDataFixture)
            where TTranscriptRequestBase : Models.TranscriptRequestBase
        {
            var fromSchoolType = new Faker().PickRandom<Enums.TranscriptSchoolType>();
            var hsFromSchools = seedDataFixture.HighSchools.Where(h => h.HasEtms && h.TranscriptFee > 0);
            var collegeFromSchools = seedDataFixture.Colleges.Where(c => c.HasEtms && c.TranscriptFee > 0);
            var uniFromSchools = seedDataFixture.Universities.Where(u => u.HasEtms && u.TranscriptFee > 0);
            var transmissions = seedDataFixture.TranscriptTransmissions;
            var requestStatuses = seedDataFixture.TranscriptRequestStatuses;

            return faker
                .RuleFor(x => x.ApplicantId, _ => Guid.NewGuid())
                .RuleFor(x => x.ApplicationId, _ => Guid.NewGuid())
                .RuleFor(x => x.EducationId, _ => Guid.NewGuid())
                .RuleFor(x => x.EtmsTranscriptRequestId, _ => null)
                .RuleFor(x => x.FromSchoolType, _ => fromSchoolType)
                .RuleFor(x => x.FromSchoolId, (f, o) =>
                {
                    switch (o.FromSchoolType)
                    {
                        case Enums.TranscriptSchoolType.HighSchool:
                            return f.PickRandom(hsFromSchools).Id;
                        case Enums.TranscriptSchoolType.College:
                            return f.PickRandom(collegeFromSchools).Id;
                        case Enums.TranscriptSchoolType.University:
                            return f.PickRandom(uniFromSchools).Id;
                        default:
                            return Guid.Empty;
                    }
                })
                .RuleFor(x => x.Name, _ => "Ocas.Domestic.Data")
                .RuleFor(x => x.PeteRequestLogId, _ => null)
                .RuleFor(x => x.ToSchoolId, _ => null)
                .RuleFor(x => x.TranscriptTransmissionId, _ => transmissions.FirstOrDefault(t => t.Code == "N").Id)
                .RuleFor(x => x.TranscriptRequestStatusId, f => f.PickRandom(requestStatuses.Where(r => r.LocalizedName == "In Progress")).Id)
                .RuleFor(x => x.TranscriptFee, (_, o) =>
                {
                    switch (o.FromSchoolType)
                    {
                        case Enums.TranscriptSchoolType.HighSchool:
                            return hsFromSchools.FirstOrDefault(s => s.Id == o.FromSchoolId).TranscriptFee;
                        case Enums.TranscriptSchoolType.College:
                            return collegeFromSchools.FirstOrDefault(s => s.Id == o.FromSchoolId).TranscriptFee;
                        case Enums.TranscriptSchoolType.University:
                            return uniFromSchools.FirstOrDefault(s => s.Id == o.FromSchoolId).TranscriptFee;
                        default:
                            return null;
                    }
                })
                .RuleSet("Highschool", set =>
                {
                    set.RuleFor(x => x.FromSchoolType, _ => Enums.TranscriptSchoolType.HighSchool)
                    .RuleFor(x => x.FromSchoolId, f => f.PickRandom(hsFromSchools).Id)
                    .RuleFor(x => x.TranscriptFee, (_, o) => hsFromSchools.FirstOrDefault(s => s.Id == o.FromSchoolId).TranscriptFee);
                })
                .RuleSet("College", set =>
                {
                    set.RuleFor(x => x.FromSchoolType, _ => Enums.TranscriptSchoolType.College)
                    .RuleFor(x => x.FromSchoolId, f => f.PickRandom(collegeFromSchools).Id)
                    .RuleFor(x => x.TranscriptFee, (_, o) => collegeFromSchools.FirstOrDefault(s => s.Id == o.FromSchoolId).TranscriptFee);
                })
                .RuleSet("University", set =>
                {
                    set.RuleFor(x => x.FromSchoolType, _ => Enums.TranscriptSchoolType.University)
                    .RuleFor(x => x.FromSchoolId, f => f.PickRandom(uniFromSchools).Id)
                    .RuleFor(x => x.TranscriptFee, (_, o) => uniFromSchools.FirstOrDefault(s => s.Id == o.FromSchoolId).TranscriptFee);
                });
        }
    }
}
