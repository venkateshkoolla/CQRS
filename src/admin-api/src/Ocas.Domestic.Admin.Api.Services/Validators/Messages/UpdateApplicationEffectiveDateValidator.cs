using System;
using FluentValidation;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Core.Extensions;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages
{
    public class UpdateApplicationEffectiveDateValidator : AbstractValidator<UpdateApplicationEffectiveDate>
    {
        public UpdateApplicationEffectiveDateValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IIdentityUserValidator());

            RuleFor(x => x.ApplicationId)
                .NotEmpty();

            RuleFor(x => x.EffectiveDate)
                .Must(x => x.IsDate())
                .WithMessage("'{PropertyName}' must be a valid date.")
                .Must(x => x.ToDateTime() <= DateTime.UtcNow)
                .WithMessage("'{PropertyName}' must be less than or equal to current date");
        }
    }
}
