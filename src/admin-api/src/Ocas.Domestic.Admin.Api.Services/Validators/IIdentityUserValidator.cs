using FluentValidation;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Validators
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
