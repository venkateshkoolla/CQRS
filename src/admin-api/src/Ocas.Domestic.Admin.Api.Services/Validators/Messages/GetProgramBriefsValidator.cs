using System.Linq;
using FluentValidation;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators.Models;
using Ocas.Domestic.Apply.Admin.Messages;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages
{
    public class GetProgramBriefsValidator : AbstractValidator<GetProgramBriefs>
    {
        public GetProgramBriefsValidator(ILookupsCache lookupsCache)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new ICollegeUserValidator(lookupsCache));

            RuleFor(x => x.ApplicationCycleId)
                .NotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var applicationCycles = await lookupsCache.GetApplicationCycles();
                    return applicationCycles.Any(a => a.Id == y);
                })
                .WithMessage("'{PropertyName}' does not exist.");

            RuleFor(x => x.Params)
                .SetValidator(new GetProgramBriefOptionsValidator(lookupsCache));
        }
    }
}
