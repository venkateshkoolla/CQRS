using System.Linq;
using FluentValidation;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Messages;

namespace Ocas.Domestic.Apply.Api.Services.Validators.Messages
{
    public class CreateApplicationValidator : AbstractValidator<CreateApplication>
    {
        public CreateApplicationValidator(ILookupsCache lookupsCache)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IApplicantUserValidator());

            RuleFor(x => x.ApplicationCycleId)
                .NotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var appCycles = await lookupsCache.GetApplicationCycles();
                    return appCycles.Any(x => x.Id == y);
                })
                .WithMessage("'{PropertyName}' does not exist.")
                .MustAsync(async (y, _) =>
                {
                    var appCycles = await lookupsCache.GetApplicationCycles();
                    var appCycle = appCycles.First(x => x.Id == y);
                    return appCycle.Status == Constants.ApplicationCycleStatuses.Active;
                })
                .WithMessage("'{PropertyName}' must be active to create an application.");
        }
    }
}
