using System.Linq;
using FluentValidation;
using Ocas.Domestic.Apply.Admin.Messages;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages
{
    public class GetCollegeApplicationCyclesValidator : AbstractValidator<GetCollegeApplicationCycles>
    {
        public GetCollegeApplicationCyclesValidator(ILookupsCache lookupsCache)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IIdentityUserValidator());

            RuleFor(x => x.CollegeId)
                .NotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var colleges = await lookupsCache.GetColleges(Constants.Localization.FallbackLocalization);
                    return colleges.Any(c => c.Id == y);
                })
                .WithMessage(y => $"'{{PropertyName}}' does not exist: {y.CollegeId}");
        }
    }
}
