using System;
using Bogus;

namespace Ocas.Domestic.Data.TestFramework.RuleCollections
{
    public static class AcademicRecordRuleCollection
    {
        public static Faker<TAcademicDataBase> ApplyAcademicRecordBaseRules<TAcademicDataBase>(this Faker<TAcademicDataBase> faker, SeedDataFixture seedDataFixture)
            where TAcademicDataBase : Models.AcademicRecordBase
        {
            var communityInvolvements = seedDataFixture.CommunityInvolvements;
            var highestEducations = seedDataFixture.HighestEducations;
            var highSkillsMajors = seedDataFixture.HighSkillsMajors;
            var literacyTests = seedDataFixture.LiteracyTests;
            var shsmCompletions = seedDataFixture.ShsmCompletions;

            return faker
                .RuleFor(x => x.CommunityInvolvementId, f => f.PickRandom(communityInvolvements).Id)
                .RuleFor(x => x.DateCredentialAchieved, DateTime.UtcNow)
                .RuleFor(x => x.HighestEducationId, f => f.PickRandom(highestEducations).Id)
                .RuleFor(x => x.HighSkillsMajorId, f => f.PickRandom(highSkillsMajors).Id)
                .RuleFor(x => x.LiteracyTestId, f => f.PickRandom(literacyTests).Id)
                .RuleFor(x => x.Mident, _ => null)
                .RuleFor(x => x.Name, _ => null)
                .RuleFor(x => x.ShsmCompletionId, f => f.PickRandom(shsmCompletions).Id);
        }
    }
}