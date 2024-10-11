using System;
using System.Linq;
using Bogus;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.TestFramework.RuleCollections
{
    public static class ApplicationRuleCollection
    {
        public static Faker<TApplicationBase> ApplyApplicationBaseRules<TApplicationBase>(this Faker<TApplicationBase> faker, AllLookups lookups)
            where TApplicationBase : ApplicationBase
        {
            var applicationActiveCycles = lookups.ApplicationCycles.Where(x => x.Status == Constants.ApplicationCycleStatuses.Active);

            return faker
                .RuleFor(o => o.ApplicantId, _ => Guid.NewGuid())
                .RuleFor(o => o.ApplicationCycleId, f => f.PickRandom(applicationActiveCycles).Id);
        }

        public static Faker<TApplication> ApplyApplicationRules<TApplication>(this Faker<TApplication> faker, AllLookups lookups)
            where TApplication : Application
        {
            var applicationActiveCycles = lookups.ApplicationCycles.Where(x => x.Status == Constants.ApplicationCycleStatuses.Active);
            var applicationStatuses = lookups.ApplicationStatuses;

            return faker
                .RuleFor(o => o.ApplicantId, _ => Guid.NewGuid())
                .RuleFor(o => o.ApplicationCycleId, f => f.PickRandom(applicationActiveCycles).Id)
                .RuleFor(o => o.ApplicationStatusId, f => f.PickRandom(applicationStatuses).Id)
                .RuleFor(o => o.EffectiveDate, _ => DateTime.UtcNow.ToStringOrDefault())
                .RuleFor(x => x.ApplicationNumber, (f, o) => f.GenerateApplicationNumber(applicationActiveCycles.First(a => a.Id == o.ApplicationCycleId).Year))
                .RuleFor(x => x.Id, Guid.NewGuid);
        }

        public static string GenerateApplicationNumber(this Faker f, string year, string format = "#########")
        {
            var applicationNumber = "10";
            var yearPrefix = year.Substring(2);
            while (int.Parse(applicationNumber) % 9 != 0 || int.Parse(applicationNumber) % 10 == 0)
            {
                applicationNumber = f.Random.ReplaceNumbers(format);
                applicationNumber = yearPrefix + applicationNumber.Substring(2);
            }

            return applicationNumber;
        }
    }
}
