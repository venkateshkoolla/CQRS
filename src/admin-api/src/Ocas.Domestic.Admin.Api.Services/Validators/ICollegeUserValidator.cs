using System.Linq;
using FluentValidation;
using Ocas.Domestic.Apply.Admin.Messages;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Validators
{
    public class ICollegeUserValidator : AbstractValidator<ICollegeUser>
    {
        public ICollegeUserValidator(ILookupsCache lookupsCache)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IIdentityUserValidator());

            RuleFor(x => x.CollegeId)
                .NotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var colleges = await lookupsCache.GetColleges(Constants.Localization.EnglishCanada);
                    return colleges.Any(c => c.Id == y);
                })
                .WithMessage("'{PropertyName}' does not exist.");
        }
    }
}
