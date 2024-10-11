using System.Linq;
using FluentValidation;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Messages;

namespace Ocas.Domestic.Apply.Api.Services.Validators.Messages
{
    public class GetCollegeTemplateValidator : AbstractValidator<GetCollegeTemplate>
    {
        public GetCollegeTemplateValidator(ILookupsCache lookupsCache)
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
                .WithMessage(y => $"'{{PropertyName}}' does not exist: {y.CollegeId}");

            RuleFor(x => x.Key)
                .IsInEnum();
        }
    }
}
