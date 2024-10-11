using FluentValidation;
using Ocas.Domestic.Apply.Admin.Messages;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages
{
    public class UpdateApplicationNumberValidator : AbstractValidator<UpdateApplicationNumber>
    {
        public UpdateApplicationNumberValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IIdentityUserValidator());

            RuleFor(x => x.ApplicationId)
                .NotEmpty();

            RuleFor(x => x.Number)
                .NotEmpty()
                .Length(8, 9)
                .Matches(Patterns.ApplicationNumber)
                .Must(y => int.Parse(y) % 9 == 0 && int.Parse(y) % 10 != 0)
                .WithMessage("'{PropertyName}' must be a numeric value with mod 9, but not mod 10");
        }
    }
}
