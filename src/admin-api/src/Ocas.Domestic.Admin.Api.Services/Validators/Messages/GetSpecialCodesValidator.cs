using System.Linq;
using FluentValidation;
using Ocas.Domestic.Apply.Admin.Messages;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages
{
    public class GetSpecialCodesValidator : AbstractValidator<GetSpecialCodes>
    {
        public GetSpecialCodesValidator(ILookupsCache lookupsCache)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IIdentityUserValidator());

            RuleFor(x => x.CollegeApplicationCycleId)
                .NotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var collegeAppCycles = await lookupsCache.GetCollegeApplicationCycles();
                    return collegeAppCycles.Any(c => c.Id == y);
                })
                .WithMessage("'{PropertyName}' does not exist.");

            RuleFor(v => v.Params.Search)
                .MaximumLength(100)
                .When(y => !string.IsNullOrWhiteSpace(y.Params.Search));
        }
    }
}
