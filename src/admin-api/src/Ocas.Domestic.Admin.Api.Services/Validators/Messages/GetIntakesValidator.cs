using System.Linq;
using FluentValidation;
using Ocas.Domestic.Apply.Admin;
using Ocas.Domestic.Apply.Admin.Api.Services;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators;
using Ocas.Domestic.Apply.Admin.Messages;

namespace Ocas.Domestic.Admin.Api.Services.Validators.Messages
{
    public class GetIntakesValidator : AbstractValidator<GetIntakes>
    {
        public GetIntakesValidator(ILookupsCache lookupsCache, IUserAuthorization userAuthorization)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IIdentityUserValidator());

            RuleFor(x => x.CollegeId)
                .NullableGuidNotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var colleges = await lookupsCache.GetColleges(Apply.Constants.Localization.EnglishCanada);
                    return colleges.Any(c => c.Id == y);
                })
                .WithMessage("'{PropertyName}' does not exist.")
                .Unless(x => !x.CollegeId.HasValue && userAuthorization.IsOcasUser(x.User));

            RuleFor(x => x.ApplicationCycleId)
                 .NotEmpty()
                 .MustAsync(async (y, _) =>
                 {
                     var appCycles = await lookupsCache.GetApplicationCycles();
                     return appCycles.Any(x => x.Id == y);
                 })
                .WithMessage("'{PropertyName}' does not exist.");
        }
    }
}
