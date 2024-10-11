using FluentValidation;
using Ocas.Domestic.Apply.Core.Messages;

namespace Ocas.Domestic.Apply.Api.Services.Validators.Messages
{
    public class GetOrderValidator : AbstractValidator<GetOrder>
    {
        public GetOrderValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IIdentityUserValidator());

            RuleFor(x => x.OrderId)
                .NotEmpty();
        }
    }
}
