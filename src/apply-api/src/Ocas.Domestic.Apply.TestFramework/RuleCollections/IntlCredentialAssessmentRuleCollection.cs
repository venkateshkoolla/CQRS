using Bogus;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.TestFramework.RuleCollections
{
    public static class IntlCredentialAssessmentRuleCollection
    {
        private const string AllowedChars = "0123456789";

        public static Faker<TIntlCredentialAssessment> ApplyIntlCredentialAssessmentRules<TIntlCredentialAssessment>(this Faker<TIntlCredentialAssessment> faker, AllLookups lookups)
            where TIntlCredentialAssessment : IntlCredentialAssessment
        {
            return faker
                .RuleFor(x => x.IntlEvaluatorId, f => f.PickRandom(lookups.CredentialEvaluationAgencies).Id)
                .RuleFor(x => x.IntlReferenceNumber, f => f.Random.String2(7, AllowedChars));
        }
    }
}
