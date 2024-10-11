using FluentValidation;
using Ocas.Domestic.Apply.Admin.Messages;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages
{
    public class GetLookupsValidator : AbstractValidator<GetLookups>
    {
        public GetLookupsValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IIdentityUserValidator());
        }
    }
}
