using System;
using System.Linq;
using Bogus;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Data.TestFramework.RuleCollections
{
    public static class TranscriptRuleCollection
    {
        public static Faker<TTranscriptBase> ApplyTranscriptBaseRules<TTranscriptBase>(this Faker<TTranscriptBase> faker)
            where TTranscriptBase : Models.TranscriptBase
        {
            return faker
                .RuleFor(x => x.Name, () => null)
                .RuleFor(x => x.Credentials, () => null)
                .RuleFor(x => x.ModifiedOn, () => null)
                .RuleSet("OntarioHighSchool", set => _ = set.RuleFor(x => x.TranscriptType, _ => TranscriptType.OntarioHighSchoolTranscript))
                .RuleSet("OntarioCollegeUniversity", set => set.RuleFor(x => x.TranscriptType, _ => TranscriptType.OntarioCollegeUniversityTranscript))
                .RuleSet("InternationalCollegeUniversity", set => set.RuleFor(x => x.TranscriptType, _ => TranscriptType.InternationalCollegeUniversityTranscript))
                .RuleSet("InternationalHighSchool", set => set.RuleFor(x => x.TranscriptType, _ => TranscriptType.InternationalHighSchoolTranscript));
        }
    }
}