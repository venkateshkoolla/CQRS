using FluentValidation;
using Ocas.Domestic.Apply.Services.Messages;

namespace Ocas.Domestic.Apply.Services.Validators.Messages
{
    public class GetSupportingDocumentFileValidator : AbstractValidator<GetSupportingDocumentFile>
    {
        public GetSupportingDocumentFileValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IIdentityUserValidator());

            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }
}
