using System;
using FluentValidation;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Validators
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilderOptions<T, Guid?> NullableGuidNotEmpty<T>(this IRuleBuilder<T, Guid?> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new NullableGuidEmptyValidator());
        }
    }
}
