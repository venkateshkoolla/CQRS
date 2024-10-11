using FluentValidation;
using Ocas.Domestic.Apply.Core.Messages;

namespace Ocas.Domestic.Apply.Api.Services.Validators.Messages
{
    public class ClaimVoucherValidator : AbstractValidator<ClaimVoucher>
    {
        public ClaimVoucherValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IIdentityUserValidator());

            RuleFor(x => x.Code)
                .NotNull()
                .NotEmpty()
                .MaximumLength(10);

            RuleFor(x => x.ApplicationId)
                .NotEmpty();
        }
    }
}
