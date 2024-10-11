using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Services.Messages;

namespace Ocas.Domestic.Apply.Services.Validators.Models
{
    public class UpdateProgramChoicesValidator : AbstractValidator<UpdateProgramChoices>
    {
        public UpdateProgramChoicesValidator(ILookupsCacheBase lookupsCache)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IIdentityUserValidator());

            RuleFor(x => x.ApplicationId)
                .NotEmpty();

            RuleFor(x => x.ProgramChoices)
                .NotNull()
                .Must(x => x.Count() <= Constants.ProgramChoices.MaxTotalChoices)
                .WithMessage($"No more than {Constants.ProgramChoices.MaxTotalChoices} choices.")
                .Must(x => x.Select(c => c.ApplicationId).AllEqual())
                .WithMessage("'{PropertyName}' must be for same application.")
                .Must(x => x.Select(c => new KeyValuePair<Guid, Guid>(c.IntakeId, c.EntryLevelId)).Distinct().Count() == x.Count())
                .WithMessage("'{PropertyName}' must be for different intakes and entry level pairing.")
                .ForEach(r => r.SetValidator(new ProgramChoiceBaseValidator(lookupsCache)));
        }
    }
}
