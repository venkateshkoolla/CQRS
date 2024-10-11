using FluentValidation;
using Ocas.Domestic.Apply.Core;

namespace Ocas.Domestic.Apply.Api.Services.Validators
{
    public class IIdentityUserValidator : AbstractValidator<IIdentityUser>
    {
        public IIdentityUserValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.User)
                .NotEmpty();
        }
    }
}
