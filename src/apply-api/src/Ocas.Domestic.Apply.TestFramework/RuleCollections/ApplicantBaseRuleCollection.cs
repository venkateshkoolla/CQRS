using System;
using Bogus;
using Bogus.Extensions;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.TestFramework.RuleCollections
{
    public static class ApplicantBaseRuleCollection
    {
        public static Faker<TApplicantBase> ApplyApplicantBaseRules<TApplicantBase>(this Faker<TApplicantBase> faker)
            where TApplicantBase : ApplicantBase
        {
            return faker
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.MiddleName, f => f.Name.FirstName().OrDefault(f))
                .RuleFor(u => u.BirthDate, f => f.Date.Past(50, DateTime.UtcNow.AddYears(-18)).ToUniversalTime().ToStringOrDefault());
        }
    }
}
