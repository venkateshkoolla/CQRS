using System;
using FluentValidation;

namespace Ocas.Domestic.Apply.Services.Validators
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilderOptions<T, Guid?> NullableGuidNotEmpty<T>(this IRuleBuilder<T, Guid?> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new NullableGuidEmptyValidator());
        }

        public static IRuleBuilderOptions<T, string> OcasEmailAddress<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                .Length(5, 50)
                .Matches(Patterns.OcasEmailAddress)
                .WithMessage("Please enter a valid Email Address");
        }
    }
}
