using System;
using System.Linq;
using Bogus;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Data.TestFramework.RuleCollections
{
    public static class ApplicationRuleCollection
    {
        public static Faker<TApplicationBase> ApplyApplicationBaseRules<TApplicationBase>(this Faker<TApplicationBase> faker, SeedDataFixture seedDataFixture)
            where TApplicationBase : Models.ApplicationBase
        {
            var applicationCycleActiveStatus = seedDataFixture.ApplicationCycleStatuses.Single(x => x.Code == ((char)ApplicationCycleStatusCode.Active).ToString());
            var applicationCycles = seedDataFixture.ApplicationCycles.Where(a => a.StatusId == applicationCycleActiveStatus.Id).ToList();
            var applicationNewApplyStatus = seedDataFixture.ApplicationStatuses.First(s => s.Code == TestConstants.ApplicationStatuses.NewApply);

            return faker
                .RuleFor(o => o.ApplicantId, _ => Guid.NewGuid())
                .RuleFor(o => o.ApplicationCycleId, f => f.PickRandom(applicationCycles).Id)
                .RuleFor(o => o.ApplicationStatusId, _ => applicationNewApplyStatus.Id)
                .RuleFor(o => o.EffectiveDate, _ => new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 0, 0, 0, DateTimeKind.Utc));
        }

        public static Faker<TApplication> ApplyApplicationRules<TApplication>(this Faker<TApplication> faker, SeedDataFixture seedDataFixture)
            where TApplication : Models.Application
        {
            return faker
                .ApplyApplicationBaseRules(seedDataFixture)
                .RuleFor(o => o.Id, _ => Guid.NewGuid())
                .RuleFor(o => o.ApplicationNumber, f => f.Random.AlphaNumeric(10))
                .RuleFor(o => o.BasisForAdmissionLock, f => f.Random.Bool())
                .RuleFor(o => o.CurrentLock, f => f.Random.Bool())
                .RuleFor(o => o.ShoppingCartStatus, f => (int)f.Random.Enum<Crm.Entities.ocaslr_application_ocaslr_shoppingcartstatus>())
                .RuleFor(o => o.CurrentId, _ => Guid.NewGuid())
                .RuleFor(o => o.BasisForAdmissionId, _ => Guid.NewGuid())
                .RuleFor(o => o.Balance, _ => null)
                .RuleFor(o => o.CompletedSteps, f => (int)f.Random.Enum<Crm.Entities.ocaslr_application_ocaslr_completedsteps>());
        }
    }
}
