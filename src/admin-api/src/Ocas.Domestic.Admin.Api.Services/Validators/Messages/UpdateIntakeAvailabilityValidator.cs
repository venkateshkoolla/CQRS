using System.Linq;
using FluentValidation;
using Ocas.Domestic.Apply.Admin.Messages;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages
{
    public class UpdateIntakeAvailabilityValidator : AbstractValidator<UpdateIntakeAvailability>
    {
        public UpdateIntakeAvailabilityValidator(ILookupsCache lookupsCache)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IIdentityUserValidator());

            RuleFor(x => x.IntakeIds)
                .NotEmpty()
                .ForEach(y => y.NotEmpty());

            RuleFor(x => x.AvailabilityId)
                .NotEmpty()
                .MustAsync(async (x, _) =>
                {
                    var intakeAvailabilities = await lookupsCache.GetIntakeAvailabilities(Constants.Localization.EnglishCanada);
                    return intakeAvailabilities.Any(y => y.Id == x);
                })
                .WithMessage(x => $"'{{PropertyName}}' does not exist: {x.AvailabilityId}");
        }
    }
}
