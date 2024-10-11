using System;
using Bogus;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Core.Extensions;

namespace Ocas.Domestic.Apply.Admin.TestFramework.RuleCollections
{
    public static class CreateProgramChoiceRequestRuleCollection
    {
        public static Faker<TCreateProgramChoiceRequest> ApplyCreateProgramChoiceRequestRules<TCreateProgramChoiceRequest>(this Faker<TCreateProgramChoiceRequest> faker, AllLookups lookups)
            where TCreateProgramChoiceRequest : CreateProgramChoiceRequest
        {
            var entryLevels = lookups.EntryLevels;

            return faker
                .RuleFor(x => x.ApplicationId, f => f.Random.Guid())
                .RuleFor(x => x.EntryLevelId, f => f.PickRandom(entryLevels).Id)
                .RuleFor(x => x.ProgramId, f => f.Random.Guid())
                .RuleFor(x => x.StartDate, f => f.Date.Past(1, DateTime.UtcNow).ToStringOrDefault(Constants.DateFormat.IntakeStartDate));
        }
    }
}
