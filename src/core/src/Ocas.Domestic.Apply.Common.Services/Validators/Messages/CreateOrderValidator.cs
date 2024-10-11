using FluentValidation;
using Ocas.Domestic.Apply.Services.Messages;

namespace Ocas.Domestic.Apply.Services.Validators.Messages
{
    public class CreateOrderValidator : AbstractValidator<CreateOrder>
    {
        public CreateOrderValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IIdentityUserValidator());

            RuleFor(x => x.ApplicationId)
                .NotEmpty();
        }
    }
}
