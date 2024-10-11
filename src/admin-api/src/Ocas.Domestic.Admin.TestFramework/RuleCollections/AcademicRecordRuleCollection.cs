using System;
using System.Linq;
using Bogus;
using Bogus.Extensions;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Core.Extensions;

namespace Ocas.Domestic.Apply.Admin.TestFramework.RuleCollections
{
    public static class AcademicRecordRuleCollection
    {
        public static Faker<TAcademicRecordBase> ApplyAcademicRecordBaseRules<TAcademicRecordBase>(this Faker<TAcademicRecordBase> faker, AllLookups allLookups)
            where TAcademicRecordBase : AcademicRecordBase
        {
            var communityInvolvements = allLookups.CommunityInvolvements;
            var highestEducations = allLookups.HighestEducations;
            var highskillsMajorIds = allLookups.HighSkillsMajors.Select(x => x.Id).ToList();
            var literacyTests = allLookups.LiteracyTests;
            var highSchools = allLookups.HighSchools.Where(h => h.Address != null).ToList();

            return faker
                .RuleFor(o => o.ApplicantId, _ => Guid.NewGuid())
                .RuleFor(o => o.CommunityInvolvementId, f => f.PickRandom(communityInvolvements).Id)
                .RuleFor(o => o.DateCredentialAchieved, f => f.Date.Past(refDate: DateTime.UtcNow).ToStringOrDefault())
                .RuleFor(o => o.HighestEducationId, f => f.PickRandom(highestEducations).Id)
                .RuleFor(o => o.HighSkillsMajorId, f => f.PickRandom(highskillsMajorIds).OrNull(f))
                .RuleFor(o => o.LiteracyTestId, f => f.PickRandom(literacyTests).Id)
                .RuleFor(o => o.SchoolId, f => f.PickRandom(highSchools).Id);
        }
    }
}
