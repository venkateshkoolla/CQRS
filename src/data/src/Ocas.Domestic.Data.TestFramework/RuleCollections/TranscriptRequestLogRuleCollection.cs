using System;
using Bogus;

namespace Ocas.Domestic.Data.TestFramework.RuleCollections
{
    public static class TranscriptRequestLogRuleCollection
    {
        public static Faker<TTranscriptRequestLogBase> ApplyTranscriptRequestBaseRules<TTranscriptRequestLogBase>(this Faker<TTranscriptRequestLogBase> faker)
            where TTranscriptRequestLogBase : Models.TranscriptRequestLogBase
        {
            return faker
                .RuleFor(o => o.Name, _ => "Ocas.Domestic.Data.Test")
                .RuleFor(o => o.ProcessStatus, _ => Enums.ProcessStatus.NotProcessed);
        }
    }
}
