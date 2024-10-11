using FluentValidation;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators.Models;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Core.Extensions;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages
{
    public class GetApplicantBriefsValidator : AbstractValidator<GetApplicantBriefs>
    {
        public GetApplicantBriefsValidator(ILookupsCache lookupsCache)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IIdentityUserValidator());

            RuleFor(x => x.Params)
                .NotNull()
                .Must(x => !(string.IsNullOrEmpty(x.AccountNumber)
                && x.ApplicationCycleId.IsEmpty()
                && string.IsNullOrEmpty(x.ApplicationNumber)
                && x.ApplicationStatusId.IsEmpty()
                && string.IsNullOrEmpty(x.BirthDate)
                && string.IsNullOrEmpty(x.Email)
                && string.IsNullOrEmpty(x.FirstName)
                && string.IsNullOrEmpty(x.LastName)
                && string.IsNullOrEmpty(x.MiddleName)
                && string.IsNullOrEmpty(x.Mident)
                && string.IsNullOrEmpty(x.OntarioEducationNumber)
                && x.PaymentLocked == null
                && string.IsNullOrEmpty(x.PreviousLastName)
                && string.IsNullOrEmpty(x.PhoneNumber)))
               .WithMessage("'{PropertyName}' must not be empty.")
               .SetValidator(new GetApplicantBriefOptionsValidator(lookupsCache));
        }
    }
}
