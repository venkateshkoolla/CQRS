using FluentValidation;
using Ocas.Domestic.Apply.Core.Messages;

namespace Ocas.Domestic.Apply.Api.Services.Validators.Messages
{
    public class GetPartnerBrandingValidator : AbstractValidator<GetPartnerBranding>
    {
        public GetPartnerBrandingValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Code)
                .NotEmpty()
                .MaximumLength(10);
        }
    }
}
