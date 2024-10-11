using System;
using System.Security.Principal;
using FluentValidation;
using Ocas.Common.Exceptions;

namespace Ocas.Domestic.Apply.Api.Services.Validators
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilderOptions<T, Guid?> NullableGuidNotEmpty<T>(this IRuleBuilder<T, Guid?> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new NullableGuidEmptyValidator());
        }

        public static IRuleBuilderOptions<T, string> OenValid<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                .Length(9)
                .Matches(Patterns.OntarioEducationNumber)
                .SetValidator(new OenCheckSumValidator());
        }

        public static IRuleBuilderOptions<T, IPrincipal> BeOcasUser<T>(this IRuleBuilder<T, IPrincipal> ruleBuilder, IUserAuthorization userAuthorization)
        {
            return ruleBuilder
                .Must(u => userAuthorization.IsOcasUser(u))
                .OnFailure((_, context) => throw new ForbiddenException($"'{context.DisplayName}' must be an OCAS user."));
        }

        public static IRuleBuilderOptions<T, IPrincipal> NotBeOcasUser<T>(this IRuleBuilder<T, IPrincipal> ruleBuilder, IUserAuthorization userAuthorization)
        {
            return ruleBuilder
                .Must(u => !userAuthorization.IsOcasUser(u))
                .OnFailure((_, context) => throw new ForbiddenException($"'{context.DisplayName}' must not be an OCAS user."));
        }
    }
}
